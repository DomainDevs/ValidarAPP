
/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/
var accountNumberFrom = null;
var accountNumberTo = null;
var status;
var reportNameAccountingAccount = Resources.AccountingAccountConsult.toUpperCase();
var typeProcessAccountingAccount = 2216;
var processTableRunning;
var isRunning = false;
var timeAccountingAccount = window.setInterval(getMassiveReportAccountingAccount, 6000);

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/
$('#AccountingNumberFromAccountingAccount').on('itemSelected', function (event, selectedItem) {
    accountNumberFrom = selectedItem.AccountingNumber;
    $('#AccountingNumberFromAccountingAccount').val(accountNumberFrom);
});

$("#AccountingNumberFromAccountingAccount").blur(function () {
    setTimeout(function () {
        $("#AccountingNumberFromAccountingAccount").val(accountNumberFrom);
    }, 50);
});

$('#AccountingNumberToAccountingAccount').on('itemSelected', function (event, selectedItem) {
    accountNumberTo = selectedItem.AccountingNumber;
    $('#AccountingNumberToAccountingAccount').val(accountNumberTo);
});

$("#AccountingNumberToAccountingAccount").blur(function () {
    setTimeout(function () {
        $("#AccountingNumberToAccountingAccount").val(accountNumberTo);
    }, 50);
});


//refresca la grilla la primera vez cuando arranca la página
getMassiveReportAccountingAccount();

// Validación de fecha Inicial debe ser mayor a fecha final

$('#StartDateAccountingAccount').blur(function () {
    if ($("#EndDateAccountingAccount").val() != "") {
        if (compare_dates($('#StartDateAccountingAccount').val(), $("#EndDateAccountingAccount").val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#StartDateAccountingAccount").val('');
        } else {
            $("#StartDateAccountingAccount").val($('#StartDateAccountingAccount').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});



// Controla que la fecha final sea mayor a la inicial

$('#EndDateAccountingAccount').blur(function () {
    if ($("#StartDateAccountingAccount").val() != "") {
        if (compare_dates($("#StartDateAccountingAccount").val(), $('#EndDateAccountingAccount').val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#EndDateAccountingAccount").val('');
        } else {
            $("#EndDateAccountingAccount").val($('#EndDateAccountingAccount').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});


//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRow(data);
});


//BOTÓN INICIAR
$("#MassiveAccountingAccount").click(function () {

    $("#alertForm").UifAlert('hide');

    $("#ReportFormAccountingAccount").validate();
    if ($("#ReportFormAccountingAccount").valid()) {

        if ($("#StartDateAccountingAccount").val() != "" && $("#EndDateAccountingAccount").val() != "") {

            if (IsDate($("#StartDateAccountingAccount").val()) == false || IsDate($("#EndDateAccountingAccount").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");

            } else {
                if (compare_dates($("#StartDateAccountingAccount").val(), $("#EndDateAccountingAccount").val())) {
                    $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                } else {

                    lockScreen();

                    setTimeout(function () {
                        getTotalRecordsMassiveAccountingAccount();
                    }, 300);
                }
            }
        }
    }
});


//Botón (SI) del modal de confirmación
$("#btnModalAccountingAccount").click(function () {
    $('#modalMassiveTotRecords').modal('hide');
    var status = $('input:radio[name=options]:checked').val();

    $.ajax({
        url: ACC_ROOT + "Reports/AccountingAccountReports",
        data: {
            "startDate": $("#StartDateAccountingAccount").val(), "endDate": $("#EndDateAccountingAccount").val(),
            "accountingNumberFrom": accountNumberFrom, "accountingNumberTo": accountNumberTo, "operation": status, "reportType": typeProcessAccountingAccount
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportAccountingAccount();

                if (!isRunning) {
                    timeAccountingAccount = window.setInterval(getMassiveReportAccountingAccount, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#CancelAccountingAccount').click(function () {
    $('#StartDateAccountingAccount').val("");
    $('#EndDateAccountingAccount').val("");
    $('#AccountingNumberFromAccountingAccount').val("");
    $('#AccountingNumberToAccountingAccount').val("");
});


/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/

//Obtiene el total de registros a procesar
function getTotalRecordsMassiveAccountingAccount() {
    totRecords = 0;
    recordsProcessed = 0;
    var status = $('input:radio[name=options]:checked').val();

    $.ajax({

        url: ACC_ROOT + "Reports/GetTotalRecordsAccountingAccount",
        data: {
            "startDate": $("#StartDateAccountingAccount").val(), "endDate": $("#EndDateAccountingAccount").val(),
            "accountingNumberFrom": accountNumberFrom, "accountingNumberTo": accountNumberTo, "operation": status
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

//Valida la fila seleccionada(lapiz edit)

function whenSelectRow(data) {

    $("#alertForm").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");
    // var status = $('input:radio[name=options]:checked').val();
    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {

        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {

            generateFileReport(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        setDownloadLinkAccountingAccount(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#StartDateAccountingAccount").val("");
    $("#EndDateAccountingAccount").val("");
    $("#AccountingNumberFromAccountingAccount").val("");
    $("#AccountingNumberToAccountingAccount").val("");
}

//Refresca o detiene la grilla de avance de proceso
function getMassiveReportAccountingAccount() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameAccountingAccount;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timeAccountingAccount);
            isRunning = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunning = true;
        }
    }

};

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

                    getMassiveReportAccountingAccount();
                    if (!isRunning) {
                        timeAccountingAccount = window.setInterval(getMassiveReportAccountingAccount, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

//Permite descargar el archivo ya generado
function setDownloadLinkAccountingAccount(fileName) {
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
