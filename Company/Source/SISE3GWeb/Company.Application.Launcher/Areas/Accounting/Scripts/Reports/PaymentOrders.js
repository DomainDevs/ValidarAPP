/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/
var status;
var reportNamePaymentOrders = Resources.PaymentOrders.toUpperCase();
var typeProcessPaymentOrders = 2152;
var processTableRunning;
var isRunning = false;
var timePaymentOrders = window.setInterval(getMassiveReportPaymentOrders, 6000);
var PaymentOrdersType = "";

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/
//refresca la grilla la primera vez cuando arranca la página
getMassiveReportPaymentOrders();

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#BranchPaymentOrders").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchPaymentOrders").removeAttr("disabled");
}

// Controla que la fecha final sea mayor a la inicial
$('#StartDatePaymentOrders').blur(function () {
    if ($("#EndDatePaymentOrders").val() != "") {
        if (compare_dates($('#StartDatePaymentOrders').val(), $("#EndDatePaymentOrders").val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#StartDatePaymentOrders").val('');
        } else {
            $("#StartDatePaymentOrders").val($('#StartDatePaymentOrders').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#EndDatePaymentOrders').blur(function () {
    if ($("#StartDatePaymentOrders").val() != "") {
        if (compare_dates($("#StartDatePaymentOrders").val(), $('#EndDatePaymentOrders').val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#EndDatePaymentOrders").val('');
        } else {
            $("#EndDatePaymentOrders").val($('#EndDatePaymentOrders').val());
            $("#alertForm").UifAlert('hide');
        }
    }
});

//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRow(data);
});

$("#MassiveReportPaymentOrders").click(function () {
    $("#alertForm").UifAlert('hide');
    
    $("#ReportFormPaymentOrders").validate();
    if ($("#ReportFormPaymentOrders").valid()) {
        if ($("#StartDatePaymentOrders").val() != "" && $("#EndDatePaymentOrders").val() != "") {

            if (IsDate($("#StartDatePaymentOrders").val()) == false || IsDate($("#EndDatePaymentOrders").val()) == false) {
                $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");

            } else {
                if (compare_dates($("#StartDatePaymentOrders").val(), $("#EndDatePaymentOrders").val())) {
                    $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
                } else {

                    lockScreen();

                    setTimeout(function () {
                        getTotalRecordsPaymentOrders();
                    }, 300);
                }
            }
        }
    }
});


//SI del modal
$("#btnModalPaymentOrder").click(function () {

    $('#modalMassiveTotRecords').modal('hide');

    if ($('#BranchPaymentOrders').val() != "") {
        branchId = $('#BranchPaymentOrders').val();
    }

    status = $('input:radio[name=options]:checked').val();

    $.ajax({
        url: ACC_ROOT + "Reports/PaymentOrdersReports",
        data: {
            "dateFrom": $("#StartDatePaymentOrders").val(), "dateTo": $("#EndDatePaymentOrders").val(),
            "branchId": branchId, "operation": status,
            "reportType": typeProcessPaymentOrders
        },
        success: function (data) {
            
            if (data.success && data.result == 0) {

                getMassiveReportPaymentOrders();

                if (!isRunning) {
                    timePaymentOrders = window.setInterval(getMassiveReportPaymentOrders, 6000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#CancelPaymentOrders').click(function () {
    $('#BranchPaymentOrders').val("");
    $('#StartDatePaymentOrders').val("");
    $('#EndDatePaymentOrders').val("");
});

/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/
function getTotalRecordsPaymentOrders() {
    totRecords = 0;
    recordsProcessed = 0;
    var branchId = -1;
  
    var status = $('input:radio[name=options]:checked').val();

    if ($('#BranchPaymentOrders').val() != "") {
        branchId = $('#BranchPaymentOrders').val();
    }

    $.ajax({
        //async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsPaymentOrders",
        data: {
            "dateFrom": $("#StartDatePaymentOrders").val(), "dateTo": $("#EndDatePaymentOrders").val(),
            "branchId": branchId, "operation": status
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
};

//Valida la fila seleccionada(lapiz edit)
function whenSelectRow(data) {

    $("#alertForm").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");
    var status = $('input:radio[name=options]:checked').val();

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {

        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {

            generateFileReport(data.ProcessId, data.RecordsNumber, data.Description);
            
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkPaymentOrders(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#StartDatePaymentOrders").val("");
    $("#EndDatePaymentOrders").val("");
}; 

function getMassiveReportPaymentOrders() {

    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNamePaymentOrders;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunning = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunning != undefined) {
        if (validateProcessReport(processTableRunning, processTableRunning.length)) {
            clearInterval(timePaymentOrders);
            isRunning = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunning = true;
        }
    }
};

function SetDownloadLinkPaymentOrders(fileName) {
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

                    getMassiveReportPaymentOrders();
                    if (!isRunning) {
                        timePaymentOrders = window.setInterval(getMassiveReportPaymentOrders, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
};
