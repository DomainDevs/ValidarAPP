/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var reportNameDebtorsByPrime = Resources.DebtorsByPrime.toUpperCase();
var typeProcessDebtorsByPrime = 2194;
var processTableRunningDebtorsByPrime;
var isRunningDebtorsByPrime = false;
var timeDebtorsByPrime = window.setInterval(getMassiveReportDebtorsByPrime, 6000);
var agentId = "";
var branchId = 0;
var prefixId = 0;

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/

getMassiveReportDebtorsByPrime();


$('#CancelDebtorsByPrime').click(function () {
    $('#BranchPortfolioCollected').val("");
    $('#PrefixPortfolioCollected').val("");
    $('#StartDatePortfolioCollected').val("");
    $('#EndDatePortfolioCollected').val("");
    $("#selectFileTypePartial").val("");
    $("#InsuredPortfolioCollected").val("");
    $("#InsuredPortfolioCollected").val("");
    $("#BrokerPortfolioCollected").val("");
    $("#alertForm").UifAlert('hide');
});

$("#MassiveReportDebtorsByPrime").click(function () {

    $("#alertForm").UifAlert('hide');

    $("#ReportFormDebtorsByPrime").validate();

    if ($("#ReportFormDebtorsByPrime").valid()) {
        if ($("#EndDateDebtorsByPrime").val() != "") {

            if (IsDate($("#EndDateDebtorsByPrime").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");

            } else {
                lockScreen();

                setTimeout(function () {
                    getTotalRecordsMassiveDebtorsByPrime();
                }, 300);
               }
            }
        }
});



//Agente
$('#AgentNameDebtorsByPrime').blur(function () {

    if ($('#AgentNameDebtorsByPrime').val() == "" ) {
        agentId = "";
    }
});

//AUTOCOMPLETES ASEGURADO DOCUMENTO
$('#AgentNameDebtorsByPrime').on('itemSelected', function (event, selectedItem) {
    agentId = selectedItem.Id;
});

//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRowDebtorsByPrime(data);
});

$("#btnModalDebtorsByPrime").click(function () {

    $('#modalMassiveTotRecords').modal('hide');

    if ($('#BranchDebtorsByPrime').val() != "") {
        branchId = $('#BranchDebtorsByPrime').val();
    }

    if ($('#PrefixDebtorsByPrime').val() != "") {
        prefixId = $('#PrefixDebtorsByPrime').val();
    }

    $.ajax({
        url: ACC_ROOT + "Reports/DebtorsByPrimeReports",
        data: {
            "branchId": branchId, "prefixId": prefixId, 
            "dateTo": $("#EndDateDebtorsByPrime").val(),"agentId": agentId,
            "reportType": typeProcessDebtorsByPrime,"currencyId":$("#CurrencyDebtorsByPrime").val()
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportDebtorsByPrime();

                if (!isRunningDebtorsByPrime) {
                    timeDebtorsByPrime = window.setInterval(getMassiveReportDebtorsByPrime, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});


/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/


function getMassiveReportDebtorsByPrime() {

    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameDebtorsByPrime;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningDebtorsByPrime = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningDebtorsByPrime != undefined) {
        if (validateProcessReport(processTableRunningDebtorsByPrime, processTableRunningDebtorsByPrime.length)) {
            clearInterval(timeDebtorsByPrime);
            isRunningDebtorsByPrime = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunningDebtorsByPrime = true;
        }
    }
};


//Valida la fila seleccionada(lapiz edit)
function whenSelectRowDebtorsByPrime(data) {

    $("#alertForm").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {

        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            generateFileReportDebtorsByPrime(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkDebtorsByPrime(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#EndDateDebtorsByPrime").val("");
}


//Genera el archivo una vez terminado el proceso inicial
function generateFileReportDebtorsByPrime(processId, records, reportDescription) {

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
                    getMassiveReportDebtorsByPrime();
                    if (!isRunningDebtorsByPrime) {
                        timeDebtorsByPrime = window.setInterval(getMassiveReportDebtorsByPrime, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function getTotalRecordsMassiveDebtorsByPrime() {

    if ($('#BranchDebtorsByPrime').val() != "") {
        branchId = $('#BranchDebtorsByPrime').val();
    }

    if ($('#PrefixDebtorsByPrime').val() != "") {
        prefixId = $('#PrefixDebtorsByPrime').val();
    }

    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalDebtorsByPrime",
        data: {
            "branchId": branchId, "prefixId": prefixId, 
            "dateTo": $("#EndDateDebtorsByPrime").val(), "agentId": agentId, "currencyId": $("#CurrencyDebtorsByPrime").val()
        },
        success: function (data) {

            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
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

function SetDownloadLinkDebtorsByPrime(fileName) {
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
function generateFileReportDebtorsByPrime(processId, records, reportDescription) {

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

                    getMassiveReportDebtorsByPrime();
                    if (!isRunningDebtorsByPrime) {
                        timeDebtorsByPrime = window.setInterval(getMassiveReportDebtorsByPrime, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
};

