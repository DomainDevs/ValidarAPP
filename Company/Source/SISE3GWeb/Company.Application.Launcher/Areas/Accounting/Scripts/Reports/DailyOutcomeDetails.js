/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var oParameterModel = {
    Id: 0,
    Name: "",
    Description: "",
    IsObject: false,
    BranchDailyOutcomeDetails: 0,
    DateFrom: null,
    DateTo: null
};

var reportNameDailyOutcomeDetails = Resources.DailyOutcomeDetails.toUpperCase();
var typeProcessDailyOutcome = 2109;
var processTableRunning;
var isRunning = false;
var timeDailyOutcome = window.setInterval(getMassiveReportDailyOutcomeDetails, 6000);

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/
//refresca la grilla la primera vez cuando arranca la página
getMassiveReportDailyOutcomeDetails();

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#BranchDailyOutcomeDetails").attr("disabled", "disabled");
    }, 300);

}
else {
    $("#BranchDailyOutcomeDetails").removeAttr("disabled");
}

// Controla que la fecha final sea mayor a la inicial
$('#StartDateDailyOutcomeDetails').blur(function () {
    if ($("#EndDateDailyOutcomeDetails").val() != "") {
        if (compare_dates($('#StartDateDailyOutcomeDetails').val(), $("#EndDateDailyOutcomeDetails").val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#StartDateDailyOutcomeDetails").val('');
        } else {
            $("#StartDateDailyOutcomeDetails").val($('#StartDateDailyOutcomeDetails').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#EndDateDailyOutcomeDetails').blur(function () {
    if ($("#StartDateDailyOutcomeDetails").val() != "") {
        if (compare_dates($("#StartDateDailyOutcomeDetails").val(), $('#EndDateDailyOutcomeDetails').val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#EndDateDailyOutcomeDetails").val('');
        } else {
            $("#EndDateDailyOutcomeDetails").val($('#EndDateDailyOutcomeDetails').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});

//BOTÓN INICIAR
$('#MassiveDailyOutcomeDetails').click(function () {
    
    $("#alertForm").UifAlert('hide');

    $("#ReportFormDailyOutcomeDetails").validate();
    if ($("#ReportFormDailyOutcomeDetails").valid()) {

        if ($("#StartDateDailyOutcomeDetails").val() != "" && $("#EndDateDailyOutcomeDetails").val() != "") {

            if (IsDate($("#StartDateDailyOutcomeDetails").val()) == false || IsDate($("#EndDateDailyOutcomeDetails").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");

            } else {
                if (compare_dates($("#StartDateDailyOutcomeDetails").val(), $("#EndDateDailyOutcomeDetails").val())) {
                    $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                } else {

                    lockScreen();

                    setTimeout(function () {
                        getTotalRecordsMassiveDailyOutcomeDetails();
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

//Botón (SI) del modal de confirmación
$("#btnModalDailyOutcomeDetails").click(function () {
    $('#modalMassiveTotRecords').modal('hide');
  
    $.ajax({
        url: ACC_ROOT + "Reports/DailyOutcomeDetailsReports",
        data: {
            "dateFrom": $("#StartDateDailyOutcomeDetails").val(), "dateTo": $("#EndDateDailyOutcomeDetails").val(),
            "branchId": $('#BranchDailyOutcomeDetails').val(), "reportType": typeProcessDailyOutcome
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportDailyOutcomeDetails();

                if (!isRunning) {
                    timeDailyOutcome = window.setInterval(getMassiveReportDailyOutcomeDetails, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

//BOTON LIMPIAR
$('#CancelDailyOutcomeDetails').click(function () {
    $('#BranchDailyOutcomeDetails').val("");
    $('#StartDateDailyOutcomeDetails').val("");
    $('#EndDateDailyOutcomeDetails').val("");
    $("#selectFileTypePartial").val("");
    $("#alertForm").UifAlert('hide');
});


/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/

//Genera el archivo una vez terminado el proceso inicial
function generateFileReportDailyOutcomeDetails(processId, records, reportDescription) {

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

                    getMassiveReportDailyOutcomeDetails();
                    if (!isRunning) {
                        timeDailyOutcome = window.setInterval(getMassiveReportDailyOutcomeDetails, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

//Valida la fila seleccionada(lapiz edit)
function whenSelectRow(data) {
    
    $("#alertForm").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {

        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            generateFileReportDailyOutcomeDetails(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        setDownloadLinkDailyOutcomeDetails(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#StartDateDailyOutcomeDetails").val("");
    $("#EndDateDailyOutcomeDetails").val("");
}

//Obtiene el total de registros a procesar
function getTotalRecordsMassiveDailyOutcomeDetails() {

    $.ajax({

        url: ACC_ROOT + "Reports/GetTotalRecordsDailyOutcomeDetails",
        data: {
            "dateFrom": $("#StartDateDailyOutcomeDetails").val(), "dateTo": $("#EndDateDailyOutcomeDetails").val(),
            "branchId": $("#BranchDailyOutcomeDetails").val()
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
}

//Refresca o detiene la grilla de avance de proceso
function getMassiveReportDailyOutcomeDetails() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameDailyOutcomeDetails;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timeDailyOutcome);
            isRunning = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunning = true;
        }
    }

};


//Permite descargar el archivo ya generado
function setDownloadLinkDailyOutcomeDetails(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertForm", "BranchDailyOutcomeDetails");
}