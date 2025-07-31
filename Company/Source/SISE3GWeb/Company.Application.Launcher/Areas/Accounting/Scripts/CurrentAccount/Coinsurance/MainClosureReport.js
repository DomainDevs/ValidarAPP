var timeClosure;
var timeClosureFinal;
var individualId = -1;
var processType = 1;
var reportName = "";
var massiveRecordId = 0;
var recordsNumber = 0;
var reportTypeDescription = "";

var processTableRunningClosure;
var isRunningClosure = false;
var isReinsure = -1;
var timeClosure = 0;

$("#rowCurrency").hide();
$("#btnGenerateTemplateChechingAccount").hide();

////////////////////////////////////////////
/// Autocomplete documento coaseguradora ///
////////////////////////////////////////////
$("#CoinsuranceDocumentNumber").on('itemSelected', function (event, selectedItem) {
    $("#alertForm").UifAlert('hide');
    individualId = selectedItem.Id;
    if (individualId > 0) {
        $("#CoinsuranceDocumentNumber").val(selectedItem.TributaryIdCardNo);
        $("#CoinsuranceName").val(selectedItem.Description);
    }
    else {
        $("#CoinsuranceDocumentNumber").val("");
        $("#CoinsuranceName").val("");
    }
});

/////////////////////////////////////////
/// Autocomplete nombre coaseguradora ///
/////////////////////////////////////////
$("#CoinsuranceName").on('itemSelected', function (event, selectedItem) {
    $("#alertForm").UifAlert('hide');
    individualId = selectedItem.Id;
    if (individualId > 0) {
        $("#CoinsuranceDocumentNumber").val(selectedItem.TributaryIdCardNo);
        $("#CoinsuranceName").val(selectedItem.Description);
    }
    else {
        $("#CoinsuranceDocumentNumber").val("");
        $("#CoinsuranceName").val("");
    }
});

//////////////////////////////////////
/// Fecha generación desde         ///
//////////////////////////////////////
$('#DateFromMainClosuredReport').on("datepicker.change", function (event, date) {
    $("#alertForm").UifAlert('hide');
    if (IsDate($('#DateFromMainClosuredReport').val())) {
        if ($("#DateToMainClosuredReport").val() != "") {
            if (CompareDates($('#DateFromMainClosuredReport').val(), $("#DateToMainClosuredReport").val()) == true) {
                $("#alertForm").UifAlert('show', Resources.MessageValidateDateFrom, "warning");
                $('#DateFromMainClosuredReport').val('');
                return true;
            }
        }
    }
    else {
        $("#alertForm").UifAlert('show', Resources.EntryDateFrom, "warning");
    }

    setTimeout(function () {
        $("#alertForm").UifAlert('hide');
    }, 3000);
});

//////////////////////////////////////
/// Fecha generación hasta         ///
//////////////////////////////////////
$('#DateToMainClosuredReport').on("datepicker.change", function (event, date) {
    $("#alertForm").UifAlert('hide');
    if (IsDate($('#DateToMainClosuredReport').val())) {
        if ($('#DateFromMainClosuredReport').val() != "") {
            if (CompareDates($('#DateFromMainClosuredReport').val(), $("#DateToMainClosuredReport").val()) == true) {
                $("#alertForm").UifAlert('show', Resources.MessageValidateDateTo, "warning");
                $("#DateToMainClosuredReport").val('');
                return true;
            }
        }
    }
    else {
        $("#alertForm").UifAlert('show', Resources.EntryDateTo, "warning");
    }

    setTimeout(function () {
        $("#alertForm").UifAlert('hide');
    }, 3000);
});

////////////////////////////////////////////
/// Selección una compañía coaseguradora ///
////////////////////////////////////////////
$('#optionCoinsurance').click(function () {
    if ($(this).is(':checked')) {
        $('#CoinsuranceDocumentNumber').attr("disabled", false);
        $('#CoinsuranceName').attr("disabled", false);
    }
});

////////////////////////////////////////////////////
/// Selección todas las compañías coaseguradoras ///
////////////////////////////////////////////////////
$('#optionAllCoinsurance').click(function () {
    if ($(this).is(':checked')) {
        $('#CoinsuranceDocumentNumber').attr("disabled", "disabled");
        $('#CoinsuranceName').attr("disabled", "disabled");
    }
    $('#CoinsuranceDocumentNumber').val("");
    $('#CoinsuranceName').val("");
});

$("#btnMassiveReportChechingAccount").click(function () {
    $("#alertForm").UifAlert('hide');

    $("#formGenerateReport").validate();
    if ($("#formGenerateReport").valid()) {
        if ($("#optionCoinsurance").is(':checked')) {  //para una compañia coaseguradora

            if ($("#CoinsuranceDocumentNumber").val() == "" || $("#CoinsuranceName").val() == "") {
                $("#alertForm").UifAlert('show', Resources.EntryCoinsurance, "warning");
                return;
            }
        }
        getTotalRecordsMassive();
    }
});

//////////////////////////////////////////////////////
// Combo cierres mensuales                         ///
//////////////////////////////////////////////////////
$("#MonthlyClosings").on('itemSelected', function (event, selectedItem) {
    $("#alertForm").UifAlert('hide');
    $("#tableMassiveProcessMain").UifDataTable('clear')
    // Se obtiene las monedas
    if (selectedItem.Id > 0) {
        reportName = $("#MonthlyClosings").UifSelect("getSelectedText");

        $('#DateFromMainClosuredReport').val("");
        $('#DateToMainClosuredReport').val("");
        individualId = -1;
        $('#Currency').val("");

        if (selectedItem.Id == 2) {
            $("#rowCurrency").show();
        }
        else {
            $("#rowCurrency").hide();
        }

        processTableRunningClosure = undefined;
        clearInterval(timeClosure);
        timeClosure = window.setInterval(getClosureReport, 6000);
        getClosureReport();
    }
    else {
        $("#btnGenerateTemplateChechingAccount").hide();
    }
});

$('#DateFromMainClosuredReport').on("change.datepicker", function (event, date) {
    if (IsDate($('#DateFromMainClosuredReport').val())) {
        $("#formGenerateReport").valid();
    }
});

$('#DateToMainClosuredReport').on("change.datepicker", function (event, date) {
    if (IsDate($('#DateToMainClosuredReport').val())) {
        $("#formGenerateReport").valid();
    }
});

$("#btnModalCheckingAccountProcessReport").click(function () {
    $('#modalCheckingAccountMassiveReport').modal('hide');
    processTableRunningClosure = undefined;
    clearInterval(timeClosure);

    var orderedBy = $('input:radio[name=options]:checked').val();
    var currencyId = -1;
    if ($('#Currency').val() != "") {
        currencyId = $('#Currency').val();
    }
    reportName = $("#MonthlyClosings").UifSelect("getSelectedText");

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Agent/LoadCoinsuranceReports",
        data: JSON.stringify(
            {
                "reportType": $("#MonthlyClosings").val(), "dateFrom": $("#DateFromMainClosuredReport").val(),
                "dateTo": $("#DateToMainClosuredReport").val(), "companyId": individualId,
                "sortOrder": orderedBy, "currencyId": currencyId, "reportTypeDescription": reportName
            }
        ),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success) {
                $("#alertForm").UifAlert('show', Resources.MessageViewReport, "success");
                getClosureReport();

                if (!isRunningClosure) {
                    timeClosure = window.setInterval(getClosureReport, 6000);
                }
            }
            else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });

    CleanFieldsClosuresReport();
});

//Generar archivo resultante
$('#tableMassiveProcessMain').on('rowEdit', function (event, data, position) {
    whenSelectRowClosure(data);
});

function getTotalRecordsMassive() {

    recordsProcessed = 0;

    var currencyId = -1;
    if ($('#Currency').val() != "") {
        currencyId = $('#Currency').val();
    }

    $.ajax({
        async: false,
        url: ACC_ROOT + "Agent/GetTotalRecordsMassive",
        data: {
            "reportType": $("#MonthlyClosings").val(), "dateFrom": $("#DateFromMainClosuredReport").val(),
            "dateTo": $("#DateToMainClosuredReport").val(), "companyId": individualId,
            "currencyId": currencyId
        },
        success: function (data) {
            if (data.success) {
                if (data.result.records > 0) {
                    $('#modalCheckingAccountMassiveReport').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertForm").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertForm").UifAlert('show', Resources.MessageInternalError, "danger");
            }
        }
    });
}

//Permite descargar el archivo ya generado
function SetDownloadLinkClosure(fileName) {
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "");
        window.open(url + fileName);
    }
    else {
        window.open(ACC_ROOT + fileName);
    }
}

function whenSelectRowClosure(data) {

    $("#alertForm").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {

        processTableRunningClosure = undefined;
        clearInterval(timeClosure);
        generateFileClosureReport(data.ProcessId, data.RecordsNumber, data.Description);

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkClosure(data.UrlFile);
    } else {
        $("#alertForm").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

    $("#DateFromMainClosuredReport").val("");
    $("#DateToMainClosuredReport").val("");
}

//Genera archivo
function generateFileClosureReport(processId, records, reportDescription) {

    $.ajax({
        url: ACC_ROOT + "Agent/GenerateStructureReport",
        data: {
            "reportType": $("#MonthlyClosings").val(),
            "processId": processId,
            "reportTypeDescription": "%",
            "exportFormatType": 0,
            "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertForm").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {

                    getClosureReport();
                    if (!isRunningClosure) {
                        timeClosure = window.setInterval(getClosureReport, 6000);
                    }
                }
            } else {
                $("#alertForm").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

//Refresca o detiene la grilla de avance de proceso
function getClosureReport() {

    var controller = ACC_ROOT + "Agent/GetMassiveReportProcess?reportName=" + reportName;
    $("#tableMassiveProcessMain").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningClosure = $('#tableMassiveProcessMain').UifDataTable('getData');
    }, 1000);

    if (processTableRunningClosure != undefined) {
        if (validateProcessReport(processTableRunningClosure, processTableRunningClosure.length)) {
            clearInterval(timeClosure);
            isRunningClosure = false;
            $("#alertForm").UifAlert('hide');
        } else {
            isRunningClosure = true;
        }
    }
};

//////////////////////////////////////////////////////////
/// Limpia los campos de generación reporte de cierres ///
//////////////////////////////////////////////////////////
function CleanFieldsClosuresReport() {
    $("#Curency").val('');
    $("#DateFromMainClosuredReport").val('');
    $("#DateToMainClosuredReport").val('');
    $("#CoinsuranceDocumentNumber").val('');
    $("#CoinsuranceName").val('');
    individualId = -1;
    $("#MonthlyClosings").focus();
}