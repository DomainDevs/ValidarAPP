var timeReportCBS = window.setInterval(RefreshMassiveReportsCBS, 6000);
var isRunningReportCBS
var processTableRunningReportCBS;
var accountingAccountId = 0;
var comparativeType = 0;

RefreshMassiveReportsCBS();

$("#CBSReportGenerate").on("click", function () {
    $("#alertCbs").UifAlert('hide');

    $("#CBSReportForm").validate();
    if ($("#CBSReportForm").valid()) {

        lockScreen();

        setTimeout(function () {
            GetTotalRecordCBS();
        }, 300);
    }
});

$("#CBSReportCancel").on("click", function () {
    CleanComparativeBalanceReport();
});

$("#CBSReportModal").on("click", function () {
    $('#modalMassiveTotRecordsCbs').modal('hide');

    processTableRunningReportCBS = undefined;
    clearInterval(timeReportCBS);
    
    comparativeType = $('input:radio[name=options]:checked').val() == 'D' ? 0 : 1;

    $.ajax({
        url: ACC_ROOT + "Reports/ComparativeBalanceSheetReports",
        data: {
            "year": $("#CBSReportYear").val(), "monthFrom": $("#CBSReportMonthFrom").val(),
            "accountingAccountId": accountingAccountId, "comparativeType": comparativeType
        },
        success: function (data) {
            if (data.success && data.result == 0) {
                RefreshMassiveReportsCBS();

                if (!isRunningReportCBS) {
                    timeReportCBS = window.setInterval(RefreshMassiveReportsCBS, 6000);
                }
                CleanComparativeBalanceReport();
            } else {
                $("#alertCbs").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertCbs").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunningReportCBS = undefined;
            clearInterval(timeReportCBS);

            GenerateFileGL(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkCBS(data.UrlFile);
    } else {
        $("#alertCbs").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }
});

$('#CBSReportAccountingNumber').on('itemSelected', function (event, selectedItem) {
    accountingAccountId = selectedItem.AccountingAccountId; //se toma el id del reguistro seleccionado
});

    /*FUNCIONES*/
    /*Total de Registros*/
function GetTotalRecordCBS() {
    comparativeType = $('input:radio[name=options]:checked').val() == 'D' ? 0 : 1;
    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsComparativeBalanceSheet",
        data: {
            "year": $("#CBSReportYear").val(), "monthFrom": $("#CBSReportMonthFrom").val(),
            "accountingAccountId": accountingAccountId, "comparativeType": comparativeType
        },
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecordsCbs').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertCbs").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertCbs").UifAlert('show', Resources.MessageInternalError, "danger");
            }
            unlockScreen();
        }
    });
}

/*Consulta el proceso Masivo*/
function RefreshMassiveReportsCBS() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.ComparativeBalanceSheet.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningReportCBS = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningReportCBS != undefined) {
        if (validateProcessReport(processTableRunningReportCBS, processTableRunningReportCBS.length)) {
            clearInterval(timeReportCBS);
            isRunningReportCBS = false;
            $("#alertCbs").UifAlert('hide');
        } else {
            isRunningReportCBS = true;
        }
    }
};

/*Genera el archivo*/
function GenerateFileGL(processId, records, reportDescription) {
    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertCbs").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    RefreshMassiveReportsCBS();
                    if (!isRunningReportCBS) {
                        timeReportCBS = window.setInterval(RefreshMassiveReportsCBS, 6000);
                    }
                }
            } else {
                $("#alertCbs").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

/*Descarga el archivo*/
function SetDownloadLinkCBS(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertCbs", "selectFileTypePartial");
}

function CleanComparativeBalanceReport() {
    $('#CBSReportYear').val("");
    $('#CBSReportMonthFrom').val("");
    $('#CBSReportAccountingNumber').val("");
    $("#alertCbs").UifAlert('hide');
}