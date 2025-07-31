/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var reportNameProviderTax = Resources.ProviderTax.toUpperCase();
var typeProcessProviderTax = 2160;
var processTableRunning;
var isRunning = false;
var timeProviderTax = window.setInterval(getMassiveReportProviderTax, 6000);

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/

//refresca la grilla la primera vez cuando arranca la página
getMassiveReportProviderTax();

//BOTÓN INICIAR
$("#MassiveProviderTax").click(function () {

    $("#alertForm").UifAlert('hide');

    $("#ReportFormProviderTax").validate();
    if ($("#ReportFormProviderTax").valid()) {
        if ($("#YearProviderTax").val() != "" && $("#MonthProviderTax").val() != "") {

            lockScreen();

            setTimeout(function () {
                getTotalRecordsMassiveProviderTax();
            }, 300);
        }
    }
});

//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRow(data);
});

//Botón (SI) del modal de confirmación
$("#btnModalProviderTax").click(function () {
    $('#modalMassiveTotRecords').modal('hide');

    $.ajax({
        url: ACC_ROOT + "Reports/ProviderTaxReports",
        data: {
            "year": $("#YearProviderTax").val(), "month": $("#MonthProviderTax").val(),
            "reportType": typeProcessProviderTax
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportProviderTax();

                if (!isRunning) {
                    timeProviderTax = window.setInterval(getMassiveReportProviderTax, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});


$('#CancelProviderTax').click(function () {
    $('#MonthProviderTax').val("");
    $('#YearProviderTax').val("");
});

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/

//Genera el archivo una vez terminado el proceso inicial
function generateFileReportProviderTax(processId, records, reportDescription) {

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

                    getMassiveReportProviderTax();
                    if (!isRunning) {
                        timeProviderTax = window.setInterval(getMassiveReportProviderTax, 6000);
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
            generateFileReportProviderTax(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        setDownloadLinkProviderTax(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#YearProviderTax").val("");
    $("#MonthProviderTax").val("");
}

//Obtiene el total de registros a procesar
function getTotalRecordsMassiveProviderTax() {

    $.ajax({

        url: ACC_ROOT + "Reports/GetTotalRecordsProviderTax",
        data: {
            "year": $("#YearProviderTax").val(), "month": $("#MonthProviderTax").val()
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
function getMassiveReportProviderTax() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameProviderTax;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timeProviderTax);
            isRunning = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunning = true;
        }
    }

};

//Permite descargar el archivo ya generado
function setDownloadLinkProviderTax(fileName) {
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
