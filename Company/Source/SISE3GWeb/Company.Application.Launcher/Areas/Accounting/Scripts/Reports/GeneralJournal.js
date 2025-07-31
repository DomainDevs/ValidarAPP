var timeReportGeneralJournal = window.setInterval(RefreshMassiveReportsGeneralJournal, 6000);
var isRunningReportGeneralJournal;
var processTableRunningReportGeneralJournal;
var accountingAccountId = 0;
var branchId = 0;

RefreshMassiveReportsGeneralJournal();

//$("#AccountingNumberGJ").on('blur', function (event) {
//    setTimeout(function () {
//        $('#AccountingNumberGJ').val('');
//    }, 50);
//});

$('#AccountingNumberGJ').on('itemSelected', function (event, selectedItem) {
    $("#alertGJ").UifAlert('hide');
    CleanAllFieldsJournal();
    $("#AccountingNumberGJ").val(selectedItem.AccountingNumber);
    accountingAccountId = selectedItem.AccountingAccountId
});

$("#GenerateGeneralJournal").on("click", function () {
    $("#alertGJ").UifAlert('hide');

    $("#ReportFormGeneralJournal").validate();
    if ($("#ReportFormGeneralJournal").valid()) {

        lockScreen();

        setTimeout(function () {
            GetTotalRecordGeneralJournal();
        }, 300);
    }
});

$("#CancelGeneralJournal").on("click", function () {
    $('#MonthGeneralJournal').val("");
    $('#YearGeneralJournal').val("");
    $('#AccountingNumberGJ').val("");
    $("#alertGJ").UifAlert('hide');
});

$("#btnModalGeneralJournal").on("click", function () {
    $('#modalMassiveTotRecords').modal('hide');

    processTableRunningReportGeneralJournal = undefined;
    clearInterval(timeReportGeneralJournal);

    branchId = $("#BranchGeneralJournal").val() == "" ? 0 : $("#BranchGeneralJournal").val();
    
    $.ajax({
        url: ACC_ROOT + "Reports/GeneralJournalReport",
        data: {
            "branch": branchId, "year": $("#YearGeneralJournal").val(),
            "month": $("#MonthGeneralJournal").val(), "accountingAccountId": accountingAccountId
        },
        success: function (data) {
            if (data.success && data.result == 0) {
                RefreshMassiveReportsGeneralJournal();

                if (!isRunningReportGeneralJournal) {
                    timeReportGeneralJournal = window.setInterval(RefreshMassiveReportsGeneralJournal, 6000);
                }
            } else {
                $("#alertGJ").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertGJ").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunningReportGeneralJournal = undefined;
            clearInterval(timeReportGeneralJournal);

            GenerateGeneralJournal(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkGeneralJournal(data.UrlFile);
    } else {
        $("#alertGJ").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }
});

/********************************FUNCIONES**********************************************************/
/*Total de Registros*/
function GetTotalRecordGeneralJournal() {

    branchId = $("#BranchGeneralJournal").val() == "" ? 0 : $("#BranchGeneralJournal").val();
    
    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsGeneralJournal",
        data: {
            "branch": branchId, "year": $("#YearGeneralJournal").val(),
            "month": $("#MonthGeneralJournal").val(), "accountingAccountId": accountingAccountId
        },
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecords').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertGJ").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertGJ").UifAlert('show', Resources.MessageInternalError, "danger");
            }
            unlockScreen();
        }
    });
}

/*Consulta el proceso Masivo*/
function RefreshMassiveReportsGeneralJournal() {

    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.GeneralJournal.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningReportGeneralJournal = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningReportGeneralJournal != undefined) {
        if (validateProcessReport(processTableRunningReportGeneralJournal, processTableRunningReportGeneralJournal.length)) {
            clearInterval(timeReportGeneralJournal);
            isRunningReportGeneralJournal = false;
            $("#alertGJ").UifAlert('hide');
        } else {
            isRunningReportGeneralJournal = true;
        }
    }
};

/*Genera el archivo*/
function GenerateGeneralJournal(processId, records, reportDescription) {

    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertGJ").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    RefreshMassiveReportsGeneralJournal();
                    if (!isRunningReportGeneralJournal) {
                        timeReportGeneralJournal = window.setInterval(RefreshMassiveReportsGeneralJournal, 6000);
                    }
                }
            } else {
                $("#alertGJ").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

/*Descarga el archivo*/
function SetDownloadLinkGeneralJournal(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertGJ", "selectFileTypePartial");
}

function CleanAllFieldsJournal() {
    $('#inputDocumentNumber').val('');
    $('#inputName').val('');
    accountingAccountId = 0 ;
}
