
var processTableRunningAnalysisReport;
var isRunningAnalysisReport = false;
var timeAnalysis = window.setInterval(getAnalysisTransactionReport, 6000);
var isReinsure = -1;
var totRecords = 0;
getAnalysisTransactionReport();
$("#AnalysisDateFrom").blur(function () {
    $("#alertAnalysisTransaction").UifAlert('hide');
    if ($("#AnalysisDateFrom").val() != '') {
        if (IsDate($("#AnalysisDateFrom").val()) == true) {
            if ($("#AnalysisDateTo").val() != '') {
                if (CompareDates($("#AnalysisDateFrom").val(), $("#AnalysisDateTo").val())) {
                    $("#AnalysisDateFrom").val(getCurrentDate);
                }
            }
        } else {
            $("#alertAnalysisTransaction").UifAlert('show', Resources.InvalidDates, "warning");
            $("#AnalysisDateFrom").val("");
        }
    }
});

$("#AnalysisDateTo").blur(function () {
    $("#alertAnalysisTransaction").UifAlert('hide');
    if ($("#AnalysisDateTo").val() != '') {
        if (IsDate($("#AnalysisDateTo").val()) == true) {
            if ($("#AnalysisDateFrom").val() != '') {
                if (CompareDates($("#AnalysisDateFrom").val(), $("#AnalysisDateTo").val())) {
                    $("#AnalysisDateTo").val(getCurrentDate);
                }
            }
        } else {
            $("#alertAnalysisTransaction").UifAlert('show', Resources.InvalidDates, "warning");
            $("#AnalysisDateTo").val("");
        }
    }
});

$("#btnCleanAnalysisTransactioon").click(function () {
    $('#AnalysisDateFrom').val("");
    $('#AnalysisDateTo').val("");
    $('#AnalysisType').val("");
    $('#AnalysisConcept').val("");
    $('#CurrencyAnalisis').val("");
    $('#AccountingNumber').val("");
    $("#alertAnalysisTransaction").UifAlert('hide');
});

$("#btnPlayAnalysisReport").click(function () {
    $("#alertAnalysisTransaction").UifAlert('hide');
    $("#ReportAnalysisForm").validate();
    if ($("#ReportAnalysisForm").valid()) {

        lockScreen();

        setTimeout(function () {
            getTotalAnalysisRecord();
        }, 300);
    }
});

/*si al modal de confirmacion con la cantidad de registros*/
$("#btnModalConfirmAnalysis").click(function () {
    $('#modalConfirmAnalysisTransaction').modal('hide');

    $.ajax({
        url: ACC_ROOT + "Reports/AnalysisTransactionReport",
        data: {
            "dateFrom": $("#AnalysisDateFrom").val(), "dateTo": $("#AnalysisDateTo").val(),
            "analysisTypeCd": ($("#AnalysisType").val() == "") ? 0 : $("#AnalysisType").val(),
            "analysisConceptCd": ($("#AnalysisConcept").val() == "") ? 0 : $("#AnalysisConcept").val(),
            "currencyCd": $("#CurrencyAnalisis").val(), "accountNumber": $("#AccountingNumber").val()
        },
        success: function (data) {
            if (data.success && data.result == 0) {
                getAnalysisTransactionReport();

                if (!isRunningAnalysisReport) {
                    timeAnalysis = window.setInterval(getAnalysisTransactionReport, 6000);
                }
            }
            else {
                $("#alertAnalysisTransaction").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

//Generar archivo resultante
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    whenSelectRowAnalysis(data);
});

function whenSelectRowAnalysis(data) {

    $("#alertAnalysisTransaction").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {

        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            generateFileReport(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkAnalysis(data.UrlFile);
    } else {
        $("#alertAnalysisTransaction").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }
}

function generateFileReport(processId, records, reportDescription) {

    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId,
            "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(),
            "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertAnalysisTransaction").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                    clearInterval(time);
                }
                else {
                    getAnalysisTransactionReport();
                    if (!isRunningAnalysisReport) {
                        timeAnalysis = window.setInterval(getAnalysisTransactionReport, 6000);
                    }
                }
            } else {
                $("#alertAnalysisTransaction").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function SetDownloadLinkAnalysis(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertAnalysisTransaction", "selectFileTypePartial");
}

function getAnalysisTransactionReport() {

    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.AnalysisAccountTransaction.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningAnalysisReport = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningAnalysisReport != undefined) {
        if (validateProcessReport(processTableRunningAnalysisReport, processTableRunningAnalysisReport.length)) {
            clearInterval(timeAnalysis);
            isRunningAnalysisReport = false;
            $("#alertAnalysisTransaction").UifAlert('hide');
        } else {
            isRunningAnalysisReport = true;
        }
    }
};

function getTotalAnalysisRecord(typeProcess) {

    totRecords = 0;
    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/TotalAnalysisTransactionReport",
        data: {
            "dateFrom": $("#AnalysisDateFrom").val(), "dateTo": $("#AnalysisDateTo").val(),
            "analysisTypeCd": ($("#AnalysisType").val() == "") ? "0" : $("#AnalysisType").val(),
            "analysisConceptCd": ($("#AnalysisConcept").val() == "") ? "0": $("#AnalysisConcept").val(),
            "currencyCd": $("#CurrencyAnalisis").val(), "accountNumber": $("#AccountingNumber").val()
        },
        success: function (data) {
            if (data.success) {
                totRecords = data.result.records;
                var msj = Resources.MsgMassiveTotRecords + ': ' + totRecords + ' .' + Resources.MsgWantContinue;
                $('#pnlModalTotRecordsAnalysis').text(msj);
                if (totRecords > 0) {
                    $('#modalConfirmAnalysisTransaction').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertAnalysisTransaction").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertAnalysisTransaction").UifAlert('show', "", "danger", data.result);
            }
            unlockScreen();
        }
    });
}