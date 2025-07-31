$(document).ajaxSend(function (event, xhr, settings) {
    if (settings.url.indexOf("GetAccountBankByBranchIdBankIdReport") != -1) {
        settings.url = settings.url + "&param=" + $("#BranchSelectIssuedChecks").val() + "/" + $("#BankSelectIssuedChecks").val();
    }
});


$('#BranchSelectIssuedChecks').on('itemSelected', function (event, selectedItem) {
    $("#alertForm").UifAlert('hide');
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "Parameters/GetBanksByBranchId?branchId=" + selectedItem.Id;
        $("#BankSelectIssuedChecks").UifSelect({ source: controller });
    }
});

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var status;
var reportNameIssuedChecks = Resources.IssuedChecks.toUpperCase();
var typeProcessIssuedChecks = 2125;
var processTableRunning;
var isRunning = false;
var timeIssuedChecks = window.setInterval(getMassiveReportIssuedChecks, 6000);


var issuedChecksType = "";
var accountNumber = "";
var accountBankId = "";

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#BranchSelectIssuedChecks").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchSelectIssuedChecks").removeAttr("disabled");
}

setTimeout(function () {
    GetBanks();
}, 2000);

$('#AccountingNumberIssuedChecks').on('itemSelected', function (event, selectedItem) {
    accountNumber = selectedItem.AccountNumber;
    accountBankId = selectedItem.AccountBankId;
});

$("#AccountingNumberIssuedChecks").blur(function () {
    setTimeout(function () {
        $("#AccountingNumberIssuedChecks").val(accountNumber);
    }, 50);
});

getMassiveReportIssuedChecks();

// Controla que la fecha final sea mayor a la inicial
$('#StartDateIssuedChecks').blur(function () {
    if ($("#EndDateIssuedChecks").val() != "") {
        if (compare_dates($('#StartDateIssuedChecks').val(), $("#EndDateIssuedChecks").val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#StartDateIssuedChecks").val('');
        } else {
            $("#StartDateIssuedChecks").val($('#StartDateIssuedChecks').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#EndDateIssuedChecks').blur(function () {
    if ($("#StartDateIssuedChecks").val() != "") {
        if (compare_dates($("#StartDateIssuedChecks").val(), $('#EndDateIssuedChecks').val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#EndDateIssuedChecks").val('');
        } else {
            $("#EndDateIssuedChecks").val($('#EndDateIssuedChecks').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});

//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRow(data);
});

$("#MassiveReportIssuedChecks").click(function () {
    $("#alertForm").UifAlert('hide');
    
    $("#ReportFormIssuedChecks").validate();
    if ($("#ReportFormIssuedChecks").valid()) {
        if ($("#StartDateIssuedChecks").val() != "" && $("#EndDateIssuedChecks").val() != "") {

            if (IsDate($("#StartDateIssuedChecks").val()) == false || IsDate($("#EndDateIssuedChecks").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");

            } else {
                if (compare_dates($("#StartDateIssuedChecks").val(), $("#EndDateIssuedChecks").val())) {
                    $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                } else {

                    lockScreen();
                    setTimeout(function () {
                        getTotalRecordsIssuedChecks();
                    }, 300);
                }
            }
        }
    }
});


//SI del modal
$("#btnModalIssuedChecks").click(function () {

    $('#modalMassiveTotRecords').modal('hide');

    var bankId = 0;
    var accountNumberbank = 0;
    var branchId = -1;

    if ($('#BranchSelectIssuedChecks').val() != "") {
        branchId = $('#BranchSelectIssuedChecks').val();
    }

    if ($('#BankSelectIssuedChecks').val() != "") {
        bankId = $('#BankSelectIssuedChecks').val();
    }

    if ($('#AccountingNumberIssuedChecks').val() != "") {
        accountNumberbank = $('#AccountingNumberIssuedChecks').val();
    }

    status = $('input:radio[name=options]:checked').val();

    $.ajax({
        url: ACC_ROOT + "Reports/IssuedChecksReports",
        data: {
            "dateFrom": $("#StartDateIssuedChecks").val(), "dateTo": $("#EndDateIssuedChecks").val(),
            "branchId": branchId, "bankId": bankId, "numberAccountBank": accountNumberbank, "operation": status,
            "reportType": typeProcessIssuedChecks
        },
        success: function (data) {
            
            if (data.success && data.result == 0) {

                getMassiveReportIssuedChecks();

                if (!isRunning) {
                    timeIssuedChecks = window.setInterval(getMassiveReportIssuedChecks, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#CancelIssuedChecks').click(function () {
    $('#BranchSelectIssuedChecks').val("");
    $('#BankSelectIssuedChecks').val("");
    $('#AccountingNumberIssuedChecks').val("");
    //$('#AmountIssuedChecks').val("");
    $('#StartDateIssuedChecks').val("");
    $('#EndDateIssuedChecks').val("");

});

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/

function getTotalRecordsIssuedChecks() {
    var totRecords = 0;
    //var recordsProcessed = 0;
    var branchId = -1;
    var bankId = 0;
    var accountNumberbank = 0;
    //var amount = -1;
    var status = $('input:radio[name=options]:checked').val();


    if ($('#BranchSelectIssuedChecks').val() != "") {
        branchId = $('#BranchSelectIssuedChecks').val();
    }

    if ($('#BankSelectIssuedChecks').val() != "") {
        bankId = $('#BankSelectIssuedChecks').val();
    }

    if ($('#AccountingNumberIssuedChecks').val() != "") {
        accountNumberbank = $('#AccountingNumberIssuedChecks').val();
    }

    $.ajax({
        //async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsIssuedChecks",
        data: {
            "dateFrom": $("#StartDateIssuedChecks").val(), "dateTo": $("#EndDateIssuedChecks").val(),
            "branchId": branchId, "bankId": bankId, "numberAccountBank": accountNumberbank, "operation": status
              },//"amount" : amount, 
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
             SetDownloadLinkIssuedChecks(data.UrlFile);
        } else {
            $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
        }

    $("#StartDateIssuedChecks").val("");
    $("#EndDateIssuedChecks").val("");
} 

function getMassiveReportIssuedChecks() {

    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameIssuedChecks;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timeIssuedChecks);
            isRunning = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunning = true;
        }
    }
}

function SetDownloadLinkIssuedChecks(fileName) {
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

                    getMassiveReportIssuedChecks();
                    if (!isRunning) {
                        timeIssuedChecks = window.setInterval(getMassiveReportIssuedChecks, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}


function GetBanks() {
    if ($('#BranchSelectIssuedChecks').val() > 0) {
        var controller = ACC_ROOT + "Parameters/GetBanksByBranchId?branchId=" + $('#BranchSelectIssuedChecks').val();
        $("#BankSelectIssuedChecks").UifSelect({ source: controller });
    }
}