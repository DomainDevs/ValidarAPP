/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var reportNameBillingMovementsList = Resources.BillingMovementList.toUpperCase();
var typeProcessBillingMovements = 2086;
var processTableRunningBillingMovements;
var isRunningBillingMovements = false;
var timeBillingMovement = window.setInterval(getMassiveReportBillingMovementsList, 6000);

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#BillingMovementsBranch").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BillingMovementsBranch").removeAttr("disabled");
}

getMassiveReportBillingMovementsList();

// Controla que la fecha final sea mayor a la inicial
$('#StartDateBillingMovements').blur(function () {
    if ($("#EndDateBillingMovements").val() != "") {
        if (compare_dates($('#StartDateBillingMovements').val(), $("#EndDateBillingMovements").val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#StartDateBillingMovements").val('');
        } else {
            $("#StartDateBillingMovements").val($('#StartDateBillingMovements').val());
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#EndDateBillingMovements').blur(function () {
    if ($("#StartDateBillingMovements").val() != "") {
        if (compare_dates($("#StartDateBillingMovements").val(), $('#EndDateBillingMovements').val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#EndDateBillingMovements").val('');
        } else {
            $("#EndDateBillingMovements").val($('#EndDateBillingMovements').val());
        }
    }
});

$('#BillingMovementsBranch').on('itemSelected', function (event, selectedItem) {
    $("#alertForm").UifAlert('hide');
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "Billing/GetCashierByBranchId?branchId=" + selectedItem.Id;
        $("#BalanceCashierBillingMovements").UifSelect({ source: controller });
    }
});

//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRow(data);
});

$("#MassiveReportBillingMovements").click(function () {
    $("#alertForm").UifAlert('hide');

    $("#ReportFormBillingMovements").validate();
    if ($("#ReportFormBillingMovements").valid()) {
        if ($("#StartDateBillingMovements").val() != "" && $("#EndDateBillingMovements").val() != "") {

            if (IsDate($("#StartDateBillingMovements").val()) == false || IsDate($("#EndDateBillingMovements").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");

            } else {
                if (compare_dates($("#StartDateBillingMovements").val(), $("#EndDateBillingMovements").val())) {
                    $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                } else {
                    lockScreen();

                    setTimeout(function () {
                        getTotalRecordsMassiveBilling();
                    }, 300);
                   
                }
            }
        }
    }
});


//SI del modal
$("#btnModalBillingMovements").click(function () {
    
    $('#modalMassiveTotRecords').modal('hide');
    

    if ($('#BillingMovementsBranch').val() != "") {
        branchId = $('#BillingMovementsBranch').val();
    }

    $.ajax({
        url: ACC_ROOT + "Reports/BillingMovementsReports",
        data: {
            "dateFrom": $("#StartDateBillingMovements").val(), "dateTo": $("#EndDateBillingMovements").val(),
            "branchId": branchId, "cashierId": $('#BalanceCashierBillingMovements').val(), 
            "reportType": typeProcessBillingMovements
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportBillingMovementsList();

                if (!isRunningBillingMovements) {
                    timeBillingMovement = window.setInterval(getMassiveReportBillingMovementsList, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#CancelReportBillingMovement').click(function () {
    $('#BillingMovementsBranch').val("");
    $('#BalanceCashierBillingMovements').val("");
    $('#StartDateBillingMovements').val("");
    $('#EndDateBillingMovements').val("");
    $("#alertForm").UifAlert('hide');
    $("#selectFileTypePartial").val("");
});


/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/

function getTotalRecordsMassiveBilling() {
    totRecords = 0;
    recordsProcessed = 0;

    var currencyId = -1;
    var branchId = -1;
    var cashierId = -1;

    if ($('#BillingMovementsBranch').val() != "") {
        branchId = $('#BillingMovementsBranch').val();
    }


    $.ajax({
        //async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsBillingMovements",
        data: {
            "dateFrom": $("#StartDateBillingMovements").val(), "dateTo": $("#EndDateBillingMovements").val(),
            "branchId": branchId, "cashierId": $('#BalanceCashierBillingMovements').val()
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

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {

        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            generateFileReport(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkBillingMovement(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#StartDateBillingMovements").val("");
    $("#EndDateBillingMovements").val("");
}

function getMassiveReportBillingMovementsList() {

    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameBillingMovementsList;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningBillingMovements = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningBillingMovements != undefined) {
        
        if (validateProcessReport(processTableRunningBillingMovements, processTableRunningBillingMovements.length)) {
            clearInterval(timeBillingMovement);
            isRunningBillingMovements = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunningBillingMovements = true;
        }
    }
};

function SetDownloadLinkBillingMovement(fileName) {
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

                    getMassiveReportBillingMovementsList();
                    if (!isRunningBillingMovements) {
                        timeBillingMovement = window.setInterval(getMassiveReportBillingMovementsList, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}
