/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var reportNameClientTax = Resources.ClientTax.toUpperCase();
var typeProcessClientTax = 2153;
var processTableRunning;
var isRunning = false;
var timeClientTax = window.setInterval(getMassiveReportClientTax, 6000);

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/

//refresca la grilla la primera vez cuando arranca la página
getMassiveReportClientTax();

//BOTÓN INICIAR
$("#MassiveClientTax").click(function () {
    
    $("#alertForm").UifAlert('hide');

    $("#ReportFormClientTax").validate();
    if ($("#ReportFormClientTax").valid()) {
        if ($("#YearClientTax").val() != "" && $("#MonthClientTax").val() != "") {

            lockScreen();

            setTimeout(function () {
                getTotalRecordsMassiveClientTax();
            }, 300);
        }
    }
});

//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRow(data);
});

//Botón (SI) del modal de confirmación
$("#btnModalClientTax").click(function () {
    $('#modalMassiveTotRecords').modal('hide');

    $.ajax({
        url: ACC_ROOT + "Reports/ClientTaxReports",
        data: {
            "year": $("#YearClientTax").val(), "month": $("#MonthClientTax").val(),
            "reportType": typeProcessClientTax
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportClientTax();

                if (!isRunning) {
                    timeClientTax = window.setInterval(getMassiveReportClientTax, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#CancelClientTax').click(function () {
    $('#MonthClientTax').val("");
    $('#YearClientTax').val("");
});

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/

//Genera el archivo una vez terminado el proceso inicial
function generateFileReportClientTax(processId, records, reportDescription) {

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

                    getMassiveReportClientTax();
                    if (!isRunning) {
                        timeClientTax = window.setInterval(getMassiveReportClientTax, 6000);
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
            generateFileReportClientTax(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        setDownloadLinkClientTax(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#YearClientTax").val("");
    $("#MonthClientTax").val("");
}

//Obtiene el total de registros a procesar
function getTotalRecordsMassiveClientTax() {
    

    $.ajax({

        url: ACC_ROOT + "Reports/GetTotalRecordsClientTax",
        data: {
            "year": $("#YearClientTax").val(), "month": $("#MonthClientTax").val()
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
function getMassiveReportClientTax() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameClientTax;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timeClientTax);
            isRunning = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunning = true;
        }
    }

};

//Permite descargar el archivo ya generado
function setDownloadLinkClientTax(fileName) {
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