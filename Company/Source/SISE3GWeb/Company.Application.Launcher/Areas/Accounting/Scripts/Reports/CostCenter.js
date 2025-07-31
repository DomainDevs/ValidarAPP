/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/
var accountNumberFromCostCenter = null;
var accountNumberToCostCenter = null;
var costCenter = null;
var status;
var reportNameCostCenter = Resources.CostCenter.toUpperCase();
var typeProcessCostCenter = 2199;
var processTableRunning;
var isRunning = false;
var timeCostCenter = window.setInterval(getMassiveReportCostCenter, 6000);

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/
$('#AccountingNumberFromCostCenter').on('itemSelected', function (event, selectedItem) {
    accountNumberFromCostCenter = selectedItem.AccountingNumber;
    $('#AccountingNumberFromCostCenter').val(accountNumberFromCostCenter);
});

$("#AccountingNumberFromCostCenter").blur(function () {
    setTimeout(function () {
        $("#AccountingNumberFromCostCenter").val(accountNumberFromCostCenter);
    }, 50);
});

$('#AccountingNumberToCostCenter').on('itemSelected', function (event, selectedItem) {
    accountNumberToCostCenter = selectedItem.AccountingNumber;
    $('#AccountingNumberToCostCenter').val(accountNumberToCostCenter);
});

$("#AccountingNumberToCostCenter").blur(function () {
    setTimeout(function () {
        $("#AccountingNumberToCostCenter").val(accountNumberToCostCenter);
    }, 50);
});

//refresca la grilla la primera vez cuando arranca la página
getMassiveReportCostCenter();

setTimeout(function () {
    $("#AccountingNumberFromCostCenter").attr("disabled", true);
    $("#AccountingNumberToCostCenter").attr("disabled", true);
}, 1000);

var OptionsType = $('input:radio[name=options]:checked').val();

$('#RangeAccountsCostCenter').click(function () {
    OptionsType = $('input:radio[name=options]:checked').val();

    if (OptionsType == "3") {
        $('#TransactionEntriesTextCostCenter').prop('disabled', true);
        $('#AccountingNumberFromCostCenter').prop('disabled', false);
        $('#AccountingNumberToCostCenter').prop('disabled', false);
        $('#TransactionEntriesTextCostCenter').val("");
    }
    else {
        $('#TransactionEntriesTextCostCenter').prop('disabled', false);
        $('#AccountingNumberFromCostCenter').prop('disabled', true);
        $('#AccountingNumberToCostCenter').prop('disabled', true);
    }
});

$('#TransactionEntriesCostCenter').click(function () {
    OptionsType = $('input:radio[name=options]:checked').val();

    if (OptionsType == "2") {
        $('#TransactionEntriesTextCostCenter').prop('disabled', false);
        $('#AccountingNumberFromCostCenter').prop('disabled', true);
        $('#AccountingNumberToCostCenter').prop('disabled', true);
        $('#AccountingNumberFromCostCenter').val("");
        $('#AccountingNumberToCostCenter').val("");
    }
    else {
        $('#TransactionEntriesTextCostCenter').prop('disabled', true);
        $('#AccountingNumberFromCostCenter').prop('disabled', false);
        $('#AccountingNumberToCostCenter').prop('disabled', false);
    }
});


$('#checkAllCostCenter').click(function () {
    var sortingFolio = ($('#checkAllCostCenter').hasClass('glyphicon glyphicon-check')) ? 1 : 0;
  
    if (sortingFolio == 0) {
        $("#CostCenterReport").prop('disabled', false);
    }
    else {
        $("#CostCenterReport").prop('disabled', true);
    }
});


$("#checkAllCostCenter").on('click', function (event, selectedItem) {

    $("#alert").UifAlert('hide');
    if ($("#checkAllCostCenter").is(':checked')) {

        $("#CostCenterReport").prop('disabled', true);
        $("#CostCenterReport").val(0);
        
    } else {
        $("#CostCenterReport").prop('disabled', false);
        $("#CostCenterReport").val("");
        
    }
});

$('span').click(function () {
    if ($("#ViewBagControlSpanCostCenter").val() == "true") {
        if ($(this).hasClass("glyphicon glyphicon-unchecked")) {
            $(this).removeClass("glyphicon glyphicon-unchecked");
            $(this).addClass("glyphicon glyphicon-check");
        }
        else if ($(this).hasClass("glyphicon glyphicon-check")) {
            $(this).removeClass("glyphicon glyphicon-check");
            $(this).addClass("glyphicon glyphicon-unchecked");
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#StartDateCostCenter').blur(function () {
    if ($("#EndDateCostCenter").val() != "") {
        if (compare_dates($('#StartDateCostCenter').val(), $("#EndDateCostCenter").val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#StartDateCostCenter").val('');
        } else {
            $("#StartDateCostCenter").val($('#StartDateCostCenter').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#EndDateCostCenter').blur(function () {
    if ($("#StartDateCostCenter").val() != "") {
        if (compare_dates($("#StartDateCostCenter").val(), $('#EndDateCostCenter').val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#EndDateCostCenter").val('');
        } else {
            $("#EndDateCostCenter").val($('#EndDateCostCenter').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});


//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRow(data);
});


//BOTÓN INICIAR
$("#MassiveCostCenter").click(function () {

    $("#alertForm").UifAlert('hide');

    $("#ReportFormCostCenter").validate();
    if ($("#ReportFormCostCenter").valid()) {
        
        if ($("#StartDateCostCenter").val() != "" && $("#EndDateCostCenter").val() != "") {

            if (IsDate($("#StartDateCostCenter").val()) == false || IsDate($("#EndDateCostCenter").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");

            } else {
                if (compare_dates($("#StartDateCostCenter").val(), $("#EndDateCostCenter").val())) {
                    $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                } else {

                    lockScreen();

                    setTimeout(function () {
                        getTotalRecordsMassiveCostCenter();
                    }, 300);
                }
            }
        }
    }
});


//Botón (SI) del modal de confirmación
$("#btnModalCostCenter").click(function () {
    $('#modalMassiveTotRecords').modal('hide');
    
    var status = null;
    var entries = null;
            
    if ($("#checkAllCostCenter").is(':checked')) {
        status = 1;
    }

    if ($('#CostCenterReport').val() != "") {
        costCenter = $('#CostCenterReport').val();
    }

   if ($('#TransactionEntriesTextCostCenter').val() != "") {
        entries = $('#TransactionEntriesTextCostCenter').val();
        status = $('input:radio[name=options]:checked').val();
    }

    if ($('#AccountingNumberFromCostCenter').val() != "") {
          status = $('input:radio[name=options]:checked').val();
    }

   $.ajax({
        url: ACC_ROOT + "Reports/CostCenterReports",
        data: {
            "costCenter": costCenter, "startDate": $("#StartDateCostCenter").val(),
            "endDate": $("#EndDateCostCenter").val(), "entries": entries,
            "accountingNumberFrom": accountNumberFromCostCenter, "accountingNumberTo": accountNumberToCostCenter, "operation": status, "reportType": typeProcessCostCenter
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportCostCenter();

                if (!isRunning) {
                    timeCostCenter = window.setInterval(getMassiveReportCostCenter, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});


$('#CancelCostCenter').click(function () {
    $('#CostCenterReport').val("");
    $('#StartDateCostCenter').val("");
    $('#EndDateCostCenter').val("");
    $('#TransactionEntriesTextCostCenter').val("");
    $('#AccountingNumberFromCostCenter').val("");
    $('#AccountingNumberToCostCenter').val("");
});

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/

//Obtiene el total de registros a procesar
function getTotalRecordsMassiveCostCenter() {
    var totRecords = 0;
    var recordsProcessed = 0;
    var status = null;
    var entries = null;
   
    if ($("#checkAllCostCenter").is(':checked')) {
        status = 1;
    }

    if ($('#CostCenterReport').val() != "") {
        costCenter = $('#CostCenterReport').val();
    }

    if ($('#TransactionEntriesTextCostCenter').val() != "") {
        entries = $('#TransactionEntriesTextCostCenter').val();
        status = $('input:radio[name=options]:checked').val();
    }

    if ($('#AccountingNumberFromCostCenter').val() != "") {
        status = $('input:radio[name=options]:checked').val();
    }

    $.ajax({
        
        url: ACC_ROOT + "Reports/GetTotalRecordsCostCenter",
        data: {
            "costCenter": costCenter, "startDate": $("#StartDateCostCenter").val(),
            "endDate": $("#EndDateCostCenter").val(), "entries": entries,
            "accountingNumberFrom": accountNumberFromCostCenter, "accountingNumberTo": accountNumberToCostCenter, "operation": status
        },
        success: function (data) {
            if (data.success) {
                totRecords = data.result.records;
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
        setDownloadLinkCostCenter(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#CostCenterReport").val("");
    $("#StartDateCostCenter").val("");
    $("#EndDateCostCenter").val("");
    $("#TransactionEntriesTextCostCenter").val("");
    $("#AccountingNumberFromCostCenter").val("");
    $("#AccountingNumberToCostCenter").val("");
}

//Refresca o detiene la grilla de avance de proceso
function getMassiveReportCostCenter() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameCostCenter;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);
    
    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timeCostCenter);
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

                    getMassiveReportCostCenter();
                    if (!isRunning) {
                        timeCostCenter = window.setInterval(getMassiveReportCostCenter, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

//Permite descargar el archivo ya generado
function setDownloadLinkCostCenter(fileName) {
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
