/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var reportNameIntermediaryTax = Resources.AgentTax.toUpperCase();
var typeProcessIntermediaryTax = 2157;
var processTableRunning;
var isRunning = false;
var timeIntermediaryTax = window.setInterval(getMassiveReportIntermediaryTax, 6000);

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/

//refresca la grilla la primera vez cuando arranca la página
getMassiveReportIntermediaryTax();

//BOTÓN INICIAR
$("#MassiveIntermediaryTax").click(function () {

    $("#alertForm").UifAlert('hide');

    $("#ReportFormIntermediaryTax").validate();
    if ($("#ReportFormIntermediaryTax").valid()) {
        if ($("#YearIntermediaryTax").val() != "" && $("#MonthIntermediaryTax").val() != "") {

            lockScreen();

            setTimeout(function () {
                getTotalRecordsMassiveIntermediaryTax();
            }, 300);
        }
    }
});

//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRow(data);
});

//Botón (SI) del modal de confirmación
$("#btnModalIntermediaryTax").click(function () {
    $('#modalMassiveTotRecords').modal('hide');

    $.ajax({
        url: ACC_ROOT + "Reports/AgentTaxReports",
        data: {
            "year": $("#YearIntermediaryTax").val(), "month": $("#MonthIntermediaryTax").val(),
            "reportType": typeProcessIntermediaryTax
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportIntermediaryTax();

                if (!isRunning) {
                    timeIntermediaryTax = window.setInterval(getMassiveReportIntermediaryTax, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#CancelIntermediaryTax').click(function () {
    $('#MonthIntermediaryTax').val("");
    $('#YearIntermediaryTax').val("");
});


/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/

//Genera el archivo una vez terminado el proceso inicial
function generateFileReportIntermediaryTax(processId, records, reportDescription) {

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

                    getMassiveReportIntermediaryTax();
                    if (!isRunning) {
                        timeIntermediaryTax = window.setInterval(getMassiveReportIntermediaryTax, 6000);
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
            generateFileReportIntermediaryTax(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        setDownloadLinkIntermediaryTax(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#YearIntermediaryTax").val("");
    $("#MonthIntermediaryTax").val("");
}

//Obtiene el total de registros a procesar
function getTotalRecordsMassiveIntermediaryTax() {

    $.ajax({

        url: ACC_ROOT + "Reports/GetTotalRecordsAgentTax",
        data: {
            "year": $("#YearIntermediaryTax").val(), "month": $("#MonthIntermediaryTax").val()
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
function getMassiveReportIntermediaryTax() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameIntermediaryTax;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timeIntermediaryTax);
            isRunning = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunning = true;
        }
    }

};


//Permite descargar el archivo ya generado
function setDownloadLinkIntermediaryTax(fileName) {
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