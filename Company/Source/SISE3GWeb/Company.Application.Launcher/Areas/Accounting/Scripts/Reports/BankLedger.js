/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var reportNameBankLedger = Resources.BankLedger.toUpperCase();
var typeProcessBankLedger = 2188;
var processTableRunning;
var isRunning = false;
var timeBankLedger = window.setInterval(getMassiveReportBankLedger, 6000);


/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/

//refresca la grilla la primera vez cuando arranca la página
getMassiveReportBankLedger();

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#BranchBankLedger").attr("disabled", "disabled");
    }, 300);

}
else {
    $("#BranchBankLedger").removeAttr("disabled");
}

// Controla que la fecha final sea mayor a la inicial
$('#StartDateBankLedger').blur(function () {
    if ($("#EndDateBankLedger").val() != "") {
        if (compare_dates($('#StartDateBankLedger').val(), $("#EndDateBankLedger").val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#StartDateBankLedger").val('');
        } else {
            $("#StartDateBankLedger").val($('#StartDateBankLedger').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#EndDateBankLedger').blur(function () {
    if ($("#StartDateBankLedger").val() != "") {
        if (compare_dates($("#StartDateBankLedger").val(), $('#EndDateBankLedger').val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#EndDateBankLedger").val('');
        } else {
            $("#EndDateBankLedger").val($('#EndDateBankLedger').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});

//BOTÓN INICIAR
$("#MassiveBankLedger").click(function () {
    $("#alertForm").UifAlert('hide');
    $("#ReportFormBankLedger").validate();
    if ($("#ReportFormBankLedger").valid()) {
        if ($("#StartDateBankLedger").val() != "" && $("#EndDateBankLedger").val() != "") {

            if (IsDate($("#StartDateBankLedger").val()) == false || IsDate($("#EndDateBankLedger").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");

            } else {
                if (compare_dates($("#StartDateBankLedger").val(), $("#EndDateBankLedger").val())) {
                    $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                } else {

                    lockScreen();

                    setTimeout(function () {
                        getTotalRecordsMassiveBankLedger();
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
$("#btnModalBankLedger").click(function () {
    $('#modalMassiveTotRecords').modal('hide');

    $.ajax({
        url: ACC_ROOT + "Reports/BankLedgerReports",
        data: {
            "branchId": $('#BranchBankLedger').val(), "dateFrom": $("#StartDateBankLedger").val(), "dateTo": $("#EndDateBankLedger").val(),
            "accountNumber": $('#AccountingNumberBankLedger').val(), "reportType": typeProcessBankLedger
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportBankLedger();

                if (!isRunning) {
                    timeBankLedger = window.setInterval(getMassiveReportBankLedger, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

//BOTON LIMPIAR

$('#CancelBankLedger').click(function () {
    $('#StartDateBankLedger').val("");
    $('#EndDateBankLedger').val("");
    $('#AccountingNumberBankLedger').val("");
    $('#BranchBankLedger').val("");
});

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/

//Genera el archivo una vez terminado el proceso inicial
function generateFileReportBankLedger(processId, records, reportDescription) {

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

                    getMassiveReportBankLedger();
                    if (!isRunning) {
                        timeBankLedger = window.setInterval(getMassiveReportBankLedger, 6000);
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
            generateFileReportBankLedger(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        setDownloadLinkBankLedger(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#StartDateBankLedger").val("");
    $("#EndDateBankLedger").val("");
}

//Obtiene el total de registros a procesar
function getTotalRecordsMassiveBankLedger() {

    $.ajax({

        url: ACC_ROOT + "Reports/GetTotalRecordsBankLedger",
        data: {
            "branchId": $('#BranchBankLedger').val(), "dateFrom": $("#StartDateBankLedger").val(), "dateTo": $("#EndDateBankLedger").val(),
            "accountNumber": $('#AccountingNumberBankLedger').val()
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
function getMassiveReportBankLedger() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameBankLedger;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timeBankLedger);
            isRunning = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunning = true;
        }
    }

};

//Permite descargar el archivo ya generado
function setDownloadLinkBankLedger(fileName) {
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
