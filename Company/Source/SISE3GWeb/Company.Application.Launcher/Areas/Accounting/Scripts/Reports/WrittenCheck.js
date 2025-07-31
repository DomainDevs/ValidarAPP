/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var reportNameWrittenCheck = Resources.WrittenCheck.toUpperCase();
var typeProcessWrittenCheck = 2185;
var processTableRunning;
var isRunning = false;
var timeWrittenCheck = window.setInterval(getMassiveReportWrittenCheck, 6000);

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/
//refresca la grilla la primera vez cuando arranca la página
getMassiveReportWrittenCheck();

// Controla que la fecha final sea mayor a la inicial
$('#StartDateWrittenCheck').blur(function () {
    if ($("#EndDateWrittenCheck").val() != "") {
        if (compare_dates($('#StartDateWrittenCheck').val(), $("#EndDateWrittenCheck").val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#StartDateWrittenCheck").val('');
        } else {
            $("#StartDateWrittenCheck").val($('#StartDateWrittenCheck').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#EndDateWrittenCheck').blur(function () {
    if ($("#StartDateWrittenCheck").val() != "") {
        if (compare_dates($("#StartDateWrittenCheck").val(), $('#EndDateWrittenCheck').val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#EndDateWrittenCheck").val('');
        } else {
            $("#EndDateWrittenCheck").val($('#EndDateWrittenCheck').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});

//BOTÓN INICIAR
$("#MassiveWrittenCheck").click(function () {

    $("#alertForm").UifAlert('hide');

    $("#ReportFormWrittenCheck").validate();
    if ($("#ReportFormWrittenCheck").valid()) {
        if ($("#StartDateWrittenCheck").val() != "" && $("#EndDateWrittenCheck").val() != "") {

            if (IsDate($("#StartDateWrittenCheck").val()) == false || IsDate($("#EndDateWrittenCheck").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");

            } else {
                if (compare_dates($("#StartDateWrittenCheck").val(), $("#EndDateWrittenCheck").val())) {
                    $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                } else {

                    lockScreen();

                    setTimeout(function () {
                        getTotalRecordsMassiveWrittenCheck();
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
$("#btnModalWrittenCheck").click(function () {
    $('#modalMassiveTotRecords').modal('hide');

    $.ajax({
        url: ACC_ROOT + "Reports/WrittenCheckReports",
        data: {
            "dateFrom": $("#StartDateWrittenCheck").val(), "dateTo": $("#EndDateWrittenCheck").val(),
            "reportType": typeProcessWrittenCheck
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportWrittenCheck();

                if (!isRunning) {
                    timeWrittenCheck = window.setInterval(getMassiveReportWrittenCheck, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

//BOTON LIMPIAR
$('#CancelWrittenCheck').click(function () {
    $('#StartDateWrittenCheck').val("");
    $('#EndDateWrittenCheck').val("");
    $("#selectFileTypePartial").val("");
    $("#alertForm").UifAlert('hide');
});


/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/

//Genera el archivo una vez terminado el proceso inicial
function generateFileReportWrittenCheck(processId, records, reportDescription) {

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

                    getMassiveReportWrittenCheck();
                    if (!isRunning) {
                        timeWrittenCheck = window.setInterval(getMassiveReportWrittenCheck, 6000);
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
            generateFileReportWrittenCheck(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        setDownloadLinkWrittenCheck(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#StartDateWrittenCheck").val("");
    $("#EndDateWrittenCheck").val("");
}

//Obtiene el total de registros a procesar
function getTotalRecordsMassiveWrittenCheck() {

    $.ajax({

        url: ACC_ROOT + "Reports/GetTotalRecordsWrittenCheck",
        data: {
            "dateFrom": $("#StartDateWrittenCheck").val(), "dateTo": $("#EndDateWrittenCheck").val()
        },
        success: function (data) {
            if (data.success) {
                var totRecords = data.result.records;
                var msj = Resources.MsgMassiveTotRecords + ': ' + totRecords + ' ' + Resources.MsgWantContinue;
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
function getMassiveReportWrittenCheck() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameWrittenCheck;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timeWrittenCheck);
            isRunning = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunning = true;
        }
    }

}

//Permite descargar el archivo ya generado
function setDownloadLinkWrittenCheck(fileName) {
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
