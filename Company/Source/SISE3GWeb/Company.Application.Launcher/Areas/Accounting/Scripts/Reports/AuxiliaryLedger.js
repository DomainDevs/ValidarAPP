var timeReportAL = window.setInterval(RefreshMassiveReportsAL, 6000);
var isRunningReportAL;
var processTableRunningReportAL;

RefreshMassiveReportsAL();


$("#ALReportGenerate").on("click", function () {
    $("#alertAL").UifAlert('hide');

    $("#ALReportForm").validate();
    if ($("#ALReportForm").valid()) {

        lockScreen();

        setTimeout(function () {
            GetTotalRecordAL();
        }, 300);
    }
});

$("#ALReportCancel").on("click", function () {
    $('#ALReportDateFrom').val("");
    $('#ALReportDateTo').val("");
    $("#alertAL").UifAlert('hide');
});

$("#ALReportModal").on("click", function () {
    $('#modalMassiveTotRecordsAl').modal('hide');

    processTableRunningReportAL = undefined;
    clearInterval(timeReportAL);

    $.ajax({
        url: ACC_ROOT + "Reports/AuxiliaryLedgerReport",
        data: {
            "brandCd": $("#ALReportBranch").val(), "dateFrom": $("#ALReportDateFrom").val(),
            "dateTo": $("#ALReportDateTo").val(), "accountNumber": $("#ALReportAccountingNumber").val()
        },
        success: function (data) {
            if (data.success && data.result == 0) {
                RefreshMassiveReportsAL();

                if (!isRunningReportAL) {
                    timeReportAL = window.setInterval(RefreshMassiveReportsAL, 6000);
                }
            } else {
                $("#alertAL").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertAL").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunningReportAL = undefined;
            clearInterval(timeReportAL);

            GenerateFileAL(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkAL(data.UrlFile);
    } else {
        $("#alertAL").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }
});

$("#ALReportDateFrom").on("datepicker.change", function (event, date) {
    $("#alertAL").UifAlert('hide');
    if ($("#ALReportDateTo").val() != "") {
        if (IsDate($("#ALReportDateFrom").val()) == true) {
            if (compare_dates($("#ALReportDateFrom").val(), $("#ALReportDateTo").val())) {
                $("#alertAL").UifAlert('show', Resources.ValidateDateTo, 'warning');
                $("#ALReportDateFrom").val('');
            } else {
                $("#ALReportDateFrom").val(date);
            }
        }
        else {
            $("#alertAL").UifAlert('show', Resources.InvalidDates, "warning");
            $("#ALReportDateFrom").val("");
        }
    }
});


$("#ALReportDateTo").on("datepicker.change", function (event, date) {
    $("#alertAL").UifAlert('hide');
    if ($("#ALReportDateTo").val() != '') {
        if (IsDate($("#ALReportDateTo").val()) == true) {
            if ($("#ALReportDateFrom").val() != '') {
                if (CompareDates($("#ALReportDateFrom").val(), $("#ALReportDateTo").val())) {
                    $("#alertAL").UifAlert('show', Resources.ValidateDateTo, 'warning');
                    $("#ALReportDateTo").val('');
                }
            }
        } else {
            $("#alertAL").UifAlert('show', Resources.InvalidDates, "warning");
            $("#ALReportDateTo").val("");
        }
    }
});

/*FUNCIONES*/
/*Total de Registros*/
function GetTotalRecordAL() {
    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalAuxiliaryLedgerReport",
        data: {
            "brandCd": $("#ALReportBranch").val(), "dateFrom": $("#ALReportDateFrom").val(),
            "dateTo": $("#ALReportDateTo").val(), "accountNumber": $("#ALReportAccountingNumber").val()
        },
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecordsAl').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertAL").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertAL").UifAlert('show', Resources.MessageInternalError, "danger");
            }
            unlockScreen();
        }
    });
}

/*Consulta el proceso Masivo*/
function RefreshMassiveReportsAL() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.AuxiliaryLedger.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningReportAL = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningReportAL != undefined) {
        if (validateProcessReport(processTableRunningReportAL, processTableRunningReportAL.length)) {
            clearInterval(timeReportAL);
            isRunningReportAL = false;
            $("#alertAL").UifAlert('hide');
        } else {
            isRunningReportAL = true;
        }
    }
};

/*Genera el archivo*/
function GenerateFileAL(processId, records, reportDescription) {
    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertAL").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    RefreshMassiveReportsAL();
                    if (!isRunningReportAL) {
                        timeReportAL = window.setInterval(RefreshMassiveReportsAL, 6000);
                    }
                }
            } else {
                $("#alertAL").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

/*Descarga el archivo*/
function SetDownloadLinkAL(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertAL", "selectFileTypePartial");
}