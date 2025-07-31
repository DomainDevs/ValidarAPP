var timeReportRB = window.setInterval(RefreshMassiveReportsRB, 6000);
var isRunningReportAE
var processTableRunningReportAE;

RefreshMassiveReportsRB();

$("#RBReportGenerate").click(function () {
    $("#alertRB").UifAlert('hide');

    $("#RBReportForm").validate();
    if ($("#RBReportForm").valid()) {

        lockScreen();

        setTimeout(function () {
            GetTotalRecordBR();
        }, 300);
    }
});

$("#RBReportCancel").click(function () {
    $('#DateFromRB').val("");
    $('#DateToRB').val("");
    $("#alertRB").UifAlert('hide');
});

$("#DateFromRB").blur(function () {
    if ($("#DateFromRB").val() != '') {

        if (IsDate($("#DateFromRB").val()) == true) {
            if ($("#DateToRB").val() != '') {
                if (CompareDates($("#DateFromRB").val(), $("#DateToRB").val())) {
                    $("#DateFromRB").val(getCurrentDate);
                }
            }
        } else {
            $("#alert").UifAlert('show', Resources.BillingInvalidDate, "danger");
            $("#DateFromRB").val("");
        }
    }
});

$("#DateToRB").blur(function () {
    if ($("#DateToRB").val() != '') {

        if (IsDate($("#DateToRB").val()) == true) {
            if ($("#DateFromRB").val() != '') {
                if (!CompareDates($("#DateToRB").val(), $("#DateFromRB").val())) {
                    $("#DateToRB").val(getCurrentDate);
                }
            }
        } else {
            $("#alert").UifAlert('show', Resources.BillingInvalidDate, "danger");
            $("#DateToRB").val("");
        }
    }
});

$("#RBModalConfirm").click(function () {
    $('#modalConfirmRB').modal('hide');

    processTableRunningReportAE = undefined;
    clearInterval(timeReportRB);

    $.ajax({
        url: ACC_ROOT + "Reports/ResultBoardReports",
        data: {
            "branchId": $("#RBReportBranch").val(), "dateFrom": $("#DateFromRB").val(),
            "dateTo": $("#DateToRB").val()
        },
        success: function (data) {
            if (data.success && data.result == 0) {
               RefreshMassiveReportsRB();

                if (!isRunningReportAE) {
                    timeReportRB = window.setInterval(RefreshMassiveReportsRB, 6000);
                }
            } else {
                $("#alertRB").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

/*Generación y descarga del archivo*/
$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertRB").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunningReportAE = undefined;
            clearInterval(timeReportRB);

           GenerateFileRB(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkAE(data.UrlFile);
    } else {
        $("#alertRB").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }
});


/*Total de Registros*/
function GetTotalRecordBR() {
    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsResultBoard",
        data: {
            "branchId": $("#RBReportBranch").val(), "dateFrom": $("#DateFromRB").val(),
            "dateTo": $("#DateToRB").val()
        },
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalConfirmRB').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertRB").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertRB").UifAlert('show', Resources.MessageInternalError, "danger");
            }
            unlockScreen();
        }
    });
}

/*Consulta el proceso Masivo*/
function RefreshMassiveReportsRB() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.ResultBoard.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningReportAE = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningReportAE != undefined) {
        if (validateProcessReport(processTableRunningReportAE, processTableRunningReportAE.length)) {
            clearInterval(timeReportRB);
            isRunningReportAE = false;
            $("#alertRB").UifAlert('hide');
        } else {
            isRunningReportAE = true;
        }
    }
};

/*Genera el archivo*/
function GenerateFileRB(processId, records, reportDescription) {
    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertRB").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    RefreshMassiveReportsRB();
                    if (!isRunningReportAE) {
                        timeReportRB = window.setInterval(RefreshMassiveReportsRB, 6000);
                    }
                }
            } else {
                $("#alertRB").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

/*Descarga el archivo*/
function SetDownloadLinkAE(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertRB", "selectFileTypePartial");
}