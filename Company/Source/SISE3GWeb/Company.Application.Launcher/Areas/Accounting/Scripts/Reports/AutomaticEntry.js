var timeReportAE = window.setInterval(RefreshMassiveReportsAE, 6000);
var isRunningReportAE;
var processTableRunningReportAE;

RefreshMassiveReportsAE();


$("#AEReportGenerate").on("click", function () {
    $("#alertAE").UifAlert('hide');

    $("#AEReportForm").validate();
    if ($("#AEReportForm").valid()) {

        lockScreen();

        setTimeout(function () {
            GetTotalRecordAE();
        }, 300);
    }
});

$("#AEReportCancel").on("click", function () {
    $('#DateFromRB').val("");
    $('#DateToRB').val("");
    $("#alertAE").UifAlert('hide');
});
    
$("#AEModalConfirm").on("click", function () {
    $('#modalConfirmAE').modal('hide');

    processTableRunningReportAE = undefined;
    clearInterval(timeReportAE);

    $.ajax({
        url: ACC_ROOT + "Reports/AutomaticEntrReports",
        data: { "year": $("#AEReportYear").val(), "month": $("#AEReportMonth").val() },
        success: function (data) {
            if (data.success && data.result == 0) {
                RefreshMassiveReportsAE();

                if (!isRunningReportAE) {
                    timeReportAE = window.setInterval(RefreshMassiveReportsAE, 6000);
                }
            } else {
                $("#alertAE").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertAE").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunningReportAE = undefined;
            clearInterval(timeReportAE);

            GenerateFileAE(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkAE(data.UrlFile);
    } else {
        $("#alertAE").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }
});


/*FUNCIONES*/
/*Total de Registros*/
function GetTotalRecordAE() {
    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsAutomaticEntry",
        data: { "year": $("#AEReportYear").val(), "month": $("#AEReportMonth").val()},
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalConfirmAE').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertAE").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertAE").UifAlert('show', Resources.MessageInternalError, "danger");
            }

            unlockScreen();
        }
    });
}

/*Consulta el proceso Masivo*/
function RefreshMassiveReportsAE() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.AutomaticEntry.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningReportAE = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningReportAE != undefined) {
        if (validateProcessReport(processTableRunningReportAE, processTableRunningReportAE.length)) {
            clearInterval(timeReportAE);
            isRunningReportAE = false;
            $("#alertAE").UifAlert('hide');
        } else {
            isRunningReportAE = true;
        }
    }
};

/*Genera el archivo*/
function GenerateFileAE(processId, records, reportDescription) {
    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertAE").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    RefreshMassiveReportsAE();
                    if (!isRunningReportAE) {
                        timeReportAE = window.setInterval(RefreshMassiveReportsAE, 6000);
                    }
                }
            } else {
                $("#alertAE").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
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
    reportFileExist(url_path, "alertAE", "selectFileTypePartial");
}
