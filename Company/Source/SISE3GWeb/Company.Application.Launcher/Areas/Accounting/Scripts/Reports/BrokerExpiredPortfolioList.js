/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/
     
var reportNameBrokerExpiredPortfolio = Resources.BrokerExpiredPortfolio.toUpperCase();
var typeProcessBrokerExpiredPortfolio = 2150;
var processTableRunningBrokerExpiredPortfolio;
var isRunningBrokerExpiredPortfolio = false;
var timeBrokerExpiredPortfolio = window.setInterval(getMassiveReportBrokerExpiredPortfolio, 6000);
var agentId = "";
var agentName = "";
var agentDocumentNumber = "";

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/

getMassiveReportBrokerExpiredPortfolio();

$('#CancelBrokerExpiredPortfolio').click(function () {
    $('#EndDateBrokerExpiredPortfolio').val("");
    $('#AgentNameBrokerExpiredPortfolio').val("");
    $('#AgentDocumentBrokerExpiredPortfolio').val("");
    $("#alertForm").UifAlert('hide');
});

$("#MassiveReportBrokerExpiredPortfolio").click(function () {

    $("#alertForm").UifAlert('hide');
    $("#ReportFormBrokerExpiredPortfolio").validate();

    if ($("#ReportFormBrokerExpiredPortfolio").valid()) {
        if ($("#EndDateBrokerExpiredPortfolio").val() != "") {

            if (IsDate($("#EndDateBrokerExpiredPortfolio").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");
            } else {

                lockScreen();

                setTimeout(function () {
                    getTotalRecordsMassiveBrokerExpiredPortfolio();
                }, 300);
            }
        }
    }
});

//AUTOCOMPLETES AGENTE NOMBRE
$('#AgentNameBrokerExpiredPortfolio').on('itemSelected', function (event, selectedItem) {
    agentId = selectedItem.Id;
    agentName = selectedItem.Name;
    agentDocumentNumber = selectedItem.DocumentNumber;
    $('#AgentNameBrokerExpiredPortfolio').val(agentName);
    $('#AgentDocumentBrokerExpiredPortfolio').val(agentDocumentNumber);
});

//AUTOCOMPLETES AGENTE DOCUMENTO
$('#AgentDocumentBrokerExpiredPortfolio').on('itemSelected', function (event, selectedItem) {
    agentId = selectedItem.Id;
    agentName = selectedItem.Name;
    agentDocumentNumber = selectedItem.DocumentNumber;
    $('#AgentNameBrokerExpiredPortfolio').val(agentName);
    $('#AgentDocumentBrokerExpiredPortfolio').val(agentDocumentNumber);
});


$('#AgentDocumentBrokerExpiredPortfolio').blur(function () {
    setTimeout(function () {
        $('#AgentNameBrokerExpiredPortfolio').val(agentName);
        $('#AgentDocumentBrokerExpiredPortfolio').val(agentDocumentNumber);
    }, 50);    
});

$('#AgentDocumentBrokerExpiredPortfolio').blur(function () {
    setTimeout(function () {
        $('#AgentNameBrokerExpiredPortfolio').val(agentName);
        $('#AgentDocumentBrokerExpiredPortfolio').val(agentDocumentNumber);
    }, 50);
});


//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRowBrokerExpiredPortfolio(data);
});

$("#btnModalBrokerExpiredPortfolio").click(function () {

    $('#modalMassiveTotRecords').modal('hide');

    listBy = $('input:radio[name=options]:checked').val();

    $.ajax({
        url: ACC_ROOT + "Reports/BrokerExpiredPortfolioReports",
        data: {
            "dateTo": $("#EndDateBrokerExpiredPortfolio").val(), "listBy": listBy,
            "agentId": agentId, "reportType": typeProcessBrokerExpiredPortfolio
        },
        success: function (data) {

            if (data.success && data.result == 0) {

                getMassiveReportBrokerExpiredPortfolio();

                if (!isRunningBrokerExpiredPortfolio) {
                    timeBrokerExpiredPortfolio = window.setInterval(getMassiveReportBrokerExpiredPortfolio, 6000);
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


function getMassiveReportBrokerExpiredPortfolio() {

    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNameBrokerExpiredPortfolio;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningBrokerExpiredPortfolio = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningBrokerExpiredPortfolio != undefined) {
        if (validateProcessReport(processTableRunningBrokerExpiredPortfolio, processTableRunningBrokerExpiredPortfolio.length)) {
            clearInterval(timeBrokerExpiredPortfolio);
            isRunningBrokerExpiredPortfolio = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunningBrokerExpiredPortfolio = true;
        }
    }
};


//Valida la fila seleccionada(lapiz edit)
function whenSelectRowBrokerExpiredPortfolio(data) {

    $("#alertForm").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {

        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            generateFileReportBrokerExpiredPortfolio(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkBrokerExpiredPortfolio(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#EndDateBrokerExpiredPortfolio").val("");
}


//Genera el archivo una vez terminado el proceso inicial
function generateFileReportBrokerExpiredPortfolio(processId, records, reportDescription) {

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
                    getMassiveReportBrokerExpiredPortfolio();
                    if (!isRunningBrokerExpiredPortfolio) {
                        timeBrokerExpiredPortfolio = window.setInterval(getMassiveReportBrokerExpiredPortfolio, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function getTotalRecordsMassiveBrokerExpiredPortfolio() {

    listBy = $('input:radio[name=options]:checked').val();

    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsBrokerExpiredPortfolio",
        data: {
            "dateTo": $("#EndDateBrokerExpiredPortfolio").val(), "listBy": listBy,
            "agentId": agentId
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

function SetDownloadLinkBrokerExpiredPortfolio(fileName) {
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