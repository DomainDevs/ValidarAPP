/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var reportNamePendingApplicationList = Resources.PendingApplication;
var typeProcessPendingApplicationList = 2106;
var processTableRunning;
var isRunning = false;
var timePendingApplication = window.setInterval(getMassiveReportPendingApplicationList, 6000);
var isApplied = 0;

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

getMassiveReportPendingApplicationList();

$("#DownloadTemplatePendingApplicationList").hide();
$("#GenerateTemplatePendingApplicationList").hide();

$('#CancelReportPendingApplicationList').click(function () {
    $("#BranchPendingApplicationList").val("");
    $("#StartDatePendingApplicationList").val("");
    $("#EndDatePendingApplicationList").val("");
    $("#selectFileTypePartial").val("");
    $("#alertForm").UifAlert('hide');
});

// Controla que la fecha final sea mayor a la inicial
$('#StartDatePendingApplicationList').blur(function(){
    if ($("#EndDatePendingApplicationList").val() != "") {
        if (compare_dates($('#StartDatePendingApplicationList').val(), $("#EndDatePendingApplicationList").val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#StartDatePendingApplicationList").val('');
        } else {
            $("#StartDatePendingApplicationList").val($('#StartDatePendingApplicationList').val());
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#EndDatePendingApplicationList').blur(function () {
    if ($("#StartDatePendingApplicationList").val() != "") {
        if (compare_dates($("#StartDatePendingApplicationList").val(), $('#EndDatePendingApplicationList').val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#EndDatePendingApplicationList").val('');
        } else {
            $("#EndDatePendingApplicationList").val($('#EndDatePendingApplicationList').val());
        }
    }
});


$("#MassiveReportPendingApplicationList").click(function () {

    $("#alertForm").UifAlert('hide');
    $("#ReportPendingApplicationListForm").validate();

    if ($("#ReportPendingApplicationListForm").valid()) {
        if ($("#StartDatePendingApplicationList").val() != "" && $("#EndDatePendingApplicationList").val() != "") {

            if (IsDate($("#StartDatePendingApplicationList").val()) == false || IsDate($("#EndDatePendingApplicationList").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");

            } else {
                if (compare_dates($("#StartDatePendingApplicationList").val(), $("#EndDatePendingApplicationList").val())) {
                    $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                } else {

                    lockScreen();

                    setTimeout(function () {
                        getTotalRecordsMassivePendingApplicationList();
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

//SI de modal para generar
$("#btnModalPendingApplicationList").click(function () {

    $('#modalMassiveTotRecords').modal('hide');


    if ($('#BranchPendingApplicationList').val() != "") {
        branchId = $('#BranchPendingApplicationList').val();
    }

    $.ajax({
        url: ACC_ROOT + "Reports/PendingApplicationListReports",
        data: {
            "dateFrom": $("#StartDatePendingApplicationList").val(), "dateTo": $("#EndDatePendingApplicationList").val(),
            "branchId": branchId, "isApplied": isApplied, "reportType": typeProcessPendingApplicationList
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportPendingApplicationList();

                if (!isRunning) {
                    timePendingApplication = window.setInterval(getMassiveReportPendingApplicationList, 6000);
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


function getMassiveReportPendingApplicationList() {

    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNamePendingApplicationList;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timePendingApplication);
            isRunning = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunning = true;
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
        SetDownloadLinkPendingApplication(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#StartDatePendingApplicationList").val("");
    $("#EndDatePendingApplicationList").val("");
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

                    getMassiveReportPendingApplicationList();
                    if (!isRunning) {
                        timePendingApplication = window.setInterval(getMassiveReportPendingApplicationList, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function getTotalRecordsMassivePendingApplicationList() {

    if ($('#BranchPendingApplicationList').val() != "") {
       var branchId = $('#BranchPendingApplicationList').val();
    }

    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsPendingApplicationList",
        data: {
            "dateFrom": $("#StartDatePendingApplicationList").val(), "dateTo": $("#EndDatePendingApplicationList").val(),
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


function SetDownloadLinkPendingApplication(fileName) {
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