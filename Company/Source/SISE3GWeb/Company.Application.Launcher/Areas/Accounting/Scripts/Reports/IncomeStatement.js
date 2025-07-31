/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var reportNameIncomeStatement = Resources.IncomeStatement.toUpperCase();
var typeProcessIncomeStatement = 2184;
var processTableRunning;
var isRunning = false;
var timeIncomeStatement = window.setInterval(getMassiveReportIncomeStatement, 6000);
/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/

//refresca la grilla la primera vez cuando arranca la página
getMassiveReportIncomeStatement();

//BOTÓN INICIAR
$("#MassiveIncomeStatement").click(function () {

    $("#alertForm").UifAlert('hide');

    $("#ReportFormIncomeStatement").validate();
    if ($("#ReportFormIncomeStatement").valid()) {
        if ($("#MonthIncomeStatement").val() != "" && $("#YearIncomeStatement").val() != "") {

            lockScreen();

            setTimeout(function () {
                getTotalRecordsMassiveIncomeStatement();
            }, 300);
        }
    }
});

//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRow(data);
});

//Botón (SI) del modal de confirmación
$("#btnModalIncomeStatement").click(function () {
    $('#modalMassiveTotRecords').modal('hide');

    $.ajax({
        url: ACC_ROOT + "Reports/IncomeStatementReports",
        data: {
            "month": $("#MonthIncomeStatement").val(), "year": $("#YearIncomeStatement").val(),
            "reportType": typeProcessIncomeStatement
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportIncomeStatement();

                if (!isRunning) {
                    timeIncomeStatement = window.setInterval(getMassiveReportIncomeStatement, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#CancelIncomeStatement').click(function () {
    $('#MonthIncomeStatement').val("");
    $('#YearIncomeStatement').val("");
    
});

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/

//Genera el archivo una vez terminado el proceso inicial
function generateFileReportIncomeStatement(processId, records, reportDescription) {

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

                    getMassiveReportIncomeStatement();
                    if (!isRunning) {
                        timeIncomeStatement = window.setInterval(getMassiveReportIncomeStatement, 6000);
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
            generateFileReportIncomeStatement(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        setDownloadLinkIncomeStatement(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#YearIncomeStatement").val("");
    $("#MonthIncomeStatement").val("");

}

//Obtiene el total de registros a procesar
function getTotalRecordsMassiveIncomeStatement() {

    $.ajax({

        url: ACC_ROOT + "Reports/GetTotalRecordsIncomeStatement",
        data: {
            "year": $("#YearIncomeStatement").val(), "month": $("#MonthIncomeStatement").val()
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
function getMassiveReportIncomeStatement() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameIncomeStatement;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timeIncomeStatement);
            isRunning = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunning = true;
        }
    }

};

//Permite descargar el archivo ya generado
function setDownloadLinkIncomeStatement(fileName) {
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