
/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/
var status;
var agentId = 0;
var agentName = $('#PRALReportAgentName').val();
var insuredId = 0;
var insuredName = $('#PRALReportAgentName').val();
var rangeId = 0;

var timePremiumReceivable = 0;
var reportNamePremium = "";
var processTableRunningPremium;
var isRunningPremium = false;

var businessTypeId = "0";
var branchId = "0";
var lineBusinessId = "0";
var cutoffDate = "0";
var dateFrom = "0";
var dateTo = "0";
var dueDate = "0";
var prefixId = "0";
var reportDescriptionType = "";


/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  ACCIONES / EVENTOS                                                      */
/*--------------------------------------------------------------------------------------------------------------------------*/

/////////////////////////////////////////////
//  Dropdown Seleccion del tipo de reporte //
////////////////////////////////////////////
$('#PRALReportOperationTypeReport').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {

        clearFields();

        reportNamePremium = selectedItem.Text;
        /*Id: 1 Antigüedad de Deuda  Id:2 Estado de Cuenta  Id: 3 Recaudos  Id : Detalle de Cobro por Pagador*/
        if (selectedItem.Id == 1) {
            $("#row2").show();
            $("#row3").show();
            $("#PRALReportDueDate").val("");
            $("#PRALReportStartDate").val("");
            $("#PRALReportEndDate").val("");

            $("#divDueDate").show();
            $("#divDateFrom").hide();
            $("#divDateTo").hide();
            $("#divRanges").show();
            $("#divAgentNumber").show();
            $("#divAgent").show();
            $("#divDateToCollection").hide();
            loadRange();

        }
        else if (selectedItem.Id == 2) {
            $("#row3").show();
            $("#row2").hide();

            $("#PRALReportDueDate").val("");
            $("#PRALReportStartDate").val("");
            $("#PRALReportInsuredByDocumentNumber").val("");
            $("#PRALReportInsuredName").val("");

            $("#divDueDate").show();
            $("#divDateFrom").hide();
            $("#divDateTo").hide();

            $("#divRanges").hide();
            $("#divAgentNumber").show();
            $("#divAgent").show();
            $("#divDateToCollection").hide();
        }
        else if (selectedItem.Id == 3) {
            $("#row2").show();
            $("#row3").show();
            $("#divRanges").hide();
            $("#PRALReportDueDate").val("");
            $("#PRALReportStartDate").val("");
            $("#PRALReportEndDate").val("");
            $("#PRALReportInsuredByDocumentNumber").val("");
            $("#PRALReportInsuredName").val("");
            $("#PRALReportDateFrom").val("");

            $("#divDateToCollection").show();
            $("#divDueDate").hide();
            $("#divDateFrom").show();
            $("#divDateTo").hide();
            $("#divAgentNumber").show();
            $("#divAgent").show();
        }
        else if (selectedItem.Id == 4) {
            $("#row2").hide();
            $("#row3").show();
            $("#divRanges").hide();
            $("#PRALReportDueDate").val("");
            $("#PRALReportStartDate").val("");
            $("#PRALReportEndDate").val("");
            $("#PRALReportInsuredByDocumentNumber").val("");
            $("#PRALReportInsuredName").val("");

            $("#divDueDate").hide();
            $("#divDateFrom").show();
            $("#divDateTo").show();
            $("#divAgentNumber").hide();
            $("#divAgent").hide();
            $("#divDateToCollection").hide();
        }

        processTableRunningPremium = $('#tableMassiveProcessReport').UifDataTable('getData');
        clearInterval(timePremiumReceivable);
        isRunningPremium = false;
        timePremiumReceivable = window.setInterval(getMassiveReportPremiumReceivable, 9000);
        getMassiveReportPremiumReceivable();
    }
    else {
        processTableRunningPremium = $('#tableMassiveProcessReport').UifDataTable('getData');
        reportNamePremium = "0";
        clearInterval(timePremiumReceivable);
        isRunningPremium = false;
        $("#tableMassiveProcessReport").dataTable().fnClearTable();
        $("#divDueDate").hide();
        $("#divDateFrom").hide();
        $("#divDateTo").hide();
        $("#divAgentNumber").hide();
        $("#divAgent").hide();
        $("#divDateToCollection").hide();
        $("#divRanges").hide();
        $("#row2").hide();
        $("#row3").show();
    }
});

$("#MassiveReportPremiumReceivableAntiquity").click(function () {
    $("#alertForm").UifAlert('hide');

    $("#PRALReportForm").validate();
    if ($("#PRALReportForm").valid()) {
        reportNamePremium = $('#PRALReportOperationTypeReport').UifSelect("getSelectedText");
        lockScreen();

        setTimeout(function () {
            getTotalRecordPremiumRecivable();
        }, 300);
    }
});

//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRowPremiumReceivableAntiquity(data);
});

$("#btnModalPremiumReceivableAntiquity").click(function () {
    $('#modalMassiveTotRecords').modal('hide');

    processTableRunningPremium = $('#tableMassiveProcessReport').UifDataTable('getData');;
    dataReport(reportNamePremium);

    $.ajax({
        url: ACC_ROOT + "Reports/PremiumReceivableReports",
        data: {
            "businessTypeId": businessTypeId, "branchId": branchId, "lineBusinessId": lineBusinessId, "agentId": agentId,
            "insuredId": insuredId, "cutoffDate": cutoffDate, "rangeId": rangeId, "dateFrom": dateFrom, "dateTo": dateTo, "dueDate": dueDate,
            "prefixId": prefixId, "reportType": reportDescriptionType
        },
        success: function (data) {
            if (data.success && data.result == 0) {
                getMassiveReportPremiumReceivable();

                if (!isRunningPremium) {
                    timePremiumReceivable = window.setInterval(getMassiveReportPremiumReceivable, 9000);
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$("#PRALGoTo").hide();

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#PRALReportBranch").attr("disabled", "disabled");
    }, 300);
} else {
    $("#PRALReportBranch").removeAttr("disabled");
}

//Valida que no ingresen una fecha invalida/////////////////////////////////////////////////////////////
$("#PRALReportStartDate").blur(function () {
    $("#alertForm").UifAlert('hide');
    if ($("#PRALReportStartDate").val() != '') {
        if (IsDate($("#PRALReportStartDate").val()) == true) {
            if ($("#PRALReportEndDate").val() != '') {
                if (CompareDates($("#PRALReportStartDate").val(), $("#PRALReportEndDate").val())) {
                    $("#alertForm").UifAlert('show', Resources.MessageValidateDateFrom, "warning");
                    $("#PRALReportStartDate").val("");
                }
            }
        } else {
            $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");
            $("#PRALReportStartDate").val("");
        }
    }
});

$("#PRALReportEndDate").blur(function () {
    $("#alertForm").UifAlert('hide');
    if ($("#PRALReportEndDate").val() != '') {
        if (IsDate($("#PRALReportEndDate").val()) == true) {
            if ($("#PRALReportStartDate").val() != '') {
                if (CompareDates($("#PRALReportStartDate").val(), $("#PRALReportEndDate").val())) {
                    $("#alertForm").UifAlert('show', Resources.MessageValidateDateTo, "warning");
                    $("#PRALReportEndDate").val("");
                }
            }
        } else {
            $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");
            $("#PRALReportEndDate").val("");
        }
    }
});

$("#PRALReportDateTo").blur(function () {
    $("#alertForm").UifAlert('hide');
    if ($("#PRALReportDateTo").val() != '') {
        if (IsDate($("#PRALReportDateTo").val()) == true) {
            if ($("#PRALReportStartDate").val() != '') {
                if (CompareDates($("#PRALReportStartDate").val(), $("#PRALReportDateTo").val())) {
                    $("#alertForm").UifAlert('show', Resources.MessageValidateDateTo, "warning");
                    $("#PRALReportDateTo").val("");
                }
            }
        } else {
            $("#alertForm").UifAlert('show', Resources.InvalidDates, "warning");
            $("#PRALReportDateTo").val("");
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#PRALReportStartDate').on('datepicker.change', function (event, date) {
    $("#alertForm").UifAlert('hide');
    if ($("#PRALReportEndDate").val() != "") {
        if (compare_dates($('#PRALReportStartDate').val(), $("#PRALReportEndDate").val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#PRALReportStartDate").val('');
        } else {
            $("#PRALReportStartDate").val($('#PRALReportStartDate').val());
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#PRALReportEndDate').on('datepicker.change', function (event, date) {
    $("#alertForm").UifAlert('hide');
    if ($("#PRALReportStartDate").val() != "") {
        if (compare_dates($("#PRALReportStartDate").val(), $('#PRALReportEndDate').val())) {
            $("#alertForm").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#PRALReportEndDate").val('');
        } else {
            $("#PRALReportEndDate").val($('#PRALReportEndDate').val());
        }
    }
});

$('#PRALReportInsuredByDocumentNumber').on('itemSelected', function (event, selectedItem) {
    insuredId = selectedItem.Id;
    if (insuredId > 0) {
        $('#PRALReportInsuredByDocumentNumber').val(selectedItem.DocumentNumber)
        $('#PRALReportInsuredName').val(selectedItem.Name)
    }
});

$("#PRALReportInsuredByDocumentNumber").blur(function () {
    if ($("#PRALReportInsuredByDocumentNumber").val() == "") {
        insuredId = 0;
        $('#PRALReportInsuredName').val("");
    }
});

$('#PRALReportInsuredName').on('itemSelected', function (event, selectedItem) {
    insuredId = selectedItem.Id;
    if (insuredId > 0) {
        $('#PRALReportInsuredName').val(selectedItem.Name)
        $('#PRALReportInsuredByDocumentNumber').val(selectedItem.DocumentNumber)
    }
});

$("#PRALReportInsuredName").blur(function () {
    if ($("#PRALReportInsuredName").val() == "") {
        insuredId = 0;
        $('#PRALReportInsuredByDocumentNumber').val("");
    }
});

$('#PRALReportAgentByDocumentNumber').on('itemSelected', function (event, selectedItem) {
    agentId = selectedItem.IndividualId;
    if (agentId > 0) {
        $('#PRALReportAgentByDocumentNumber').val(selectedItem.DocumentNumber)
        $('#PRALReportAgentName').val(selectedItem.Name)
    }
});

$("#PRALReportAgentByDocumentNumber").blur(function () {
    if ($("#PRALReportAgentByDocumentNumber").val() == "") {
        agentId = 0;
        $('#PRALReportAgentName').val("");
    }
});

$('#PRALReportAgentName').on('itemSelected', function (event, selectedItem) {
    agentId = selectedItem.IndividualId;
    if (agentId > 0) {
        $('#PRALReportAgentName').val(selectedItem.Name)
        $('#PRALReportAgentByDocumentNumber').val(selectedItem.DocumentNumber);
    }
});

$("#PRALReportAgentName").blur(function () {
    if ($("#PRALReportAgentName").val() == "") {
        agentId = 0;
        $('#PRALReportAgentByDocumentNumber').val("");
    }
});

$('#CancelPremiumReceivableAntiquity').click(function () {

    $("#PRALReportOperationTypeReport").val("");
    clearFields();
    clearInterval(timePremiumReceivable);
    isRunningPremium = false;

});

$('#PRALReportPrefix').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "Parameters/GetLineBusinessByPrefixId?prefixId=" + selectedItem.Id;
        $("#PRALReportPrefixTechnical").UifSelect({ source: controller });
    }
    else {
        $('#PRALReportPrefixTechnical').UifSelect();
    }
});



/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                  FUNCTION                                                                */
/*--------------------------------------------------------------------------------------------------------------------------*/

function clearFields() {

    $('#PRALReportOperation').val("");
    $('#PRALReportPrefix').val("");
    $('#PRALReportCurrency').val("");
    $('#PRALReportSalesPoint').val("");
    $('#PRALReportStartDate').val("");
    $('#PRALReportEndDate').val("");
    $("#alertForm").UifAlert('hide');
    $("#PRALGoTo").hide()
    $("#PRALReportOption").val("");

    $("#row2").hide();
    $("#row3").hide();
    $("#divRanges").hide();
    $("#PRALReportDueDate").val("");
    $("#PRALReportStartDate").val("");
    $("#PRALReportEndDate").val("");
    $("#PRALReportInsuredByDocumentNumber").val("");
    $("#PRALReportInsuredName").val("");
    $("#PRALReportDateFrom").val("");

    $("#divDateToCollection").hide();
    $("#divDueDate").hide();
    $("#divDateFrom").hide();
    $("#divDateTo").hide();
    $("#divAgentNumber").hide();
    $("#divAgent").hide();

    $("#divDueDate").hide();
    $("#divDateFrom").hide();
    $("#divDateTo").hide();
    $("#divAgentNumber").hide();
    $("#divAgent").hide();
    $("#divDateToCollection").hide();
    $("#PRALReportDateTo").val("");
    $("#PRALReportAgentByDocumentNumber").val("");
    $("#PRALReportAgentName").val("");
    $("#PRALReportPrefixTechnical").UifSelect();
    $("#tableMassiveProcessReport").UifDataTable('clear')

    agentId = 0;
    insuredId = 0;
}

function loadRange() {
    $.ajax({
        url: ACC_ROOT + "Parameters/GetDefaultRange",
    }).done(function (data) {
        if (data != null) {
            filterRangeDefault(data);
        }
    });
}

function filterRangeDefault(range) {
    if (range != null) {
        var rangeCodeDefault = -1;
        for (var i = 0; i <= range.length - 1; i++) {
            if (range[i].IsDefault) {
                rangeCodeDefault = range[i].Id;
            }
        }
        bindgDefault(rangeCodeDefault);
    }
}

function bindgDefault(rangeCodeDefault) {
    var controller = ACC_ROOT + "Parameters/GetSelectListRanges";
    $("#PRALReportRangeList").UifSelect({ source: controller, selectedId: rangeCodeDefault });
}

function whenSelectRowPremiumReceivableAntiquity(data) {
    $("#alertForm").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunningPremium = $('#tableMassiveProcessReport').UifDataTable('getData');;
            generateFilePremiumReceivableReport(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkPremiumReceivable(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }
}

function getTotalRecordPremiumRecivable() {

    dataReport(reportNamePremium);


    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsPremiumReceivable",
        data: {
            "businessTypeId": businessTypeId, "branchId": branchId, "lineBusinessId": lineBusinessId, "agentId": agentId,
            "insuredId": insuredId, "cutoffDate": cutoffDate, "rangeId": rangeId, "dateFrom": dateFrom, "dateTo": dateTo, "dueDate": dueDate,
            "prefixId": prefixId, "reportType": reportDescriptionType
        },
        success: function (data) {

            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecords').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    clearInterval(timePremiumReceivable);
                    $("#alertForm").UifAlert('show', msj, "warning");
                }
            }
            else {
                clearInterval(timePremiumReceivable);
                $("#alertForm").UifAlert('show', Resources.MessageInternalError, "danger");
            }

            unlockScreen();
        }
    });
}

function getMassiveReportPremiumReceivable() {
    //Control de window.setInterval, se realiza la consulta cuando hay datos en la tabla
    processTableRunningPremium = $('#tableMassiveProcessReport').UifDataTable('getData');

    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + reportNamePremium;
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });


    setTimeout(function () {
        processTableRunningPremium = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningPremium.length > 0) {
        if (validateProcessReport(processTableRunningPremium, processTableRunningPremium.length)) {
            clearInterval(timePremiumReceivable); 
            isRunningPremium = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunningPremium = true;
        }
    }
};

function generateFilePremiumReceivableReport(processId, records, reportDescription) {
    reportNamePremium = reportDescription;
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
                    getMassiveReportPremiumReceivable();
                    if (!isRunningPremium) {
                        timePremiumReceivable = window.setInterval(getMassiveReportPremiumReceivable, 9000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function SetDownloadLinkPremiumReceivable(fileName) {
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

function dataReport(report) {
    var dtaDateTo = "";
    if (report == Resources.PayerPaymentDetail) {
        dtaDateTo = ($("#PRALReportEndDate").val() == "") ? "0" : $("#PRALReportEndDate").val();
    }
    else if (report == Resources.Collection) {
        dtaDateTo = ($("#PRALReportDateTo").val() == "") ? "0" : $("#PRALReportDateTo").val();
    }

    businessTypeId = ($("#PRALReportOperation").val() == "") ? "0" : $("#PRALReportOperation").val();
    branchId = ($("#PRALReportBranch").val() == "") ? "0" : $("#PRALReportBranch").val();
    prefixId = ($("#PRALReportPrefix").val() == "") ? "0" : $("#PRALReportPrefix").val();
    cutoffDate = ($("#PRALReportDueDate").val() == "") ? "0" : $("#PRALReportDueDate").val();
    rangeId = ($("#PRALReportRangeList").val() == "") ? "0" : $("#PRALReportRangeList").val();
    dateFrom = ($("#PRALReportStartDate").val() == "") ? "0" : $("#PRALReportStartDate").val();
    dateTo = dtaDateTo;
    dueDate = ($("#PRALReportDueDate").val() == "") ? "0" : $("#PRALReportDueDate").val();
    reportDescriptionType = report;
    lineBusinessId = ($("#PRALReportPrefixTechnical").val() == "") ? "0" : $("#PRALReportPrefixTechnical").val();
}
