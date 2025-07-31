var timeReportPR = window.setInterval(RefreshMassiveReportsPR, 6000);
var isRunningReportPR;
var processTableRunningReportPR;

RefreshMassiveReportsPR();


$("#PRReportGenerate").on("click", function () {
   $("#alertFormPR").UifAlert('hide');

    $("#PRReportForm").validate();
    if ($("#PRReportForm").valid()) {

        lockScreen();

        setTimeout(function () {
            GetTotalRecordPR();
        }, 300);
    }
});

$("#PRReportCancel").on("click", function () {
    $('#PRReportDateFrom').val("");
    $('#PRReportDateTo').val("");
    $("#alertFormPR").UifAlert('hide');
});

$("#PRBtnModalConfirm").on("click", function () {
    $('#modalConfirmPR').modal('hide');

    processTableRunningReportPR = undefined;
    clearInterval(timeReportPR);

    $.ajax({
        url: ACC_ROOT + "Reports/RetentionPolicyReports",
        data: { "dateFrom": $("#PRReportDateFrom").val(), "dateTo": $("#PRReportDateTo").val(), "branchId": $("#PRReportBranch").val() },
        success: function (data) {
            if (data.success && data.result == 0) {
                RefreshMassiveReportsPR();

                if (!isRunningReportPR) {
                    timeReportPR = window.setInterval(RefreshMassiveReportsPR, 6000);
                }
            } else {
                $("#alertFormPR").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
   $("#alertFormPR").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunningReportPR = undefined;
            clearInterval(timeReportPR);

            GenerateFilePR(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkPR(data.UrlFile);
    } else {
        $("#alertFormPR").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }
});

$("#PRReportDateFrom").blur(function () {
    if ($("#PRReportDateFrom").val() != '') {

        if (IsDate($("#PRReportDateFrom").val()) == true) {
            if ($("#PRReportDateTo").val() != '') {
                if (CompareDates($("#PRReportDateFrom").val(), $("#PRReportDateTo").val())) {
                    $("#PRReportDateFrom").val(getCurrentDate);
                }
            }
        } else {
            $("#alert").UifAlert('show', Resources.BillingInvalidDate, "danger");
            $("#PRReportDateFrom").val("");
        }
    }
});

$("#PRReportDateTo").blur(function () {
    if ($("#PRReportDateTo").val() != '') {

        if (IsDate($("#PRReportDateTo").val()) == true) {
            if ($("#PRReportDateFrom").val() != '') {
                if (!CompareDates($("#PRReportDateTo").val(), $("#PRReportDateFrom").val())) {
                    $("#PRReportDateTo").val(getCurrentDate);
                }
            }
        } else {
            $("#alert").UifAlert('show', Resources.BillingInvalidDate, "danger");
            $("#PRReportDateTo").val("");
        }
    }
});


$("#PRReportBranch").on('itemSelected', function (event, selectedItem) {
    $("#alertFormPR").UifAlert('hide');
});

/*FUNCIONES*/
/*Total de Registros*/
function GetTotalRecordPR() {
    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsRetentionPolicy",
        data: { "dateFrom": $("#PRReportDateFrom").val(), "dateTo": $("#PRReportDateTo").val(), "branchId": $("#PRReportBranch").val() },
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalConfirmPR').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertFormPR").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertFormPR").UifAlert('show', Resources.MessageInternalError, "danger");
            }
            unlockScreen();
        }
    });
}

/*Consulta el proceso Masivo*/
function RefreshMassiveReportsPR() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.PoliciesRetention.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningReportPR = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningReportPR != undefined) {
        if (validateProcessReport(processTableRunningReportPR, processTableRunningReportPR.length)) {
            clearInterval(timeReportPR);
            isRunningReportPR = false;
            $("#alertFormPR").UifAlert('hide');
        } else {
            isRunningReportPR = true;
        }
    }
};

/*Genera el archivo*/
function GenerateFilePR(processId, records, reportDescription) {
    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertFormPR").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    RefreshMassiveReportsPR();
                    if (!isRunningReportPR) {
                        timeReportPR = window.setInterval(RefreshMassiveReportsPR, 6000);
                    }
                }
            } else {
                $("#alertFormPR").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

/*Descarga el archivo*/
function SetDownloadLinkPR(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertFormPR", "selectFileTypePartial");
}