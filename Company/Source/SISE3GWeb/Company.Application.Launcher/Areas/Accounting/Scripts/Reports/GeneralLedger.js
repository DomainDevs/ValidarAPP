var timeReportGL = window.setInterval(RefreshMassiveReportsGL, 6000);
var isRunningReportGL;
var processTableRunningReportGL;
var accountingAccountId = 0;
var branchId = 0;


RefreshMassiveReportsGL();

$("#GLReportGenerate").on("click", function () {
    $("#alertGL").UifAlert('hide');

    $("#GLReportForm").validate();
    if ($("#GLReportForm").valid()) {

        lockScreen();

        setTimeout(function () {
            GetTotalRecordGL();
        }, 300);
    }
});

$("#GLReportCancel").on("click", function () {
    CleanGeneralLedgerReport();
});

$("#GLReportAccountingNumber").on('itemSelected', function (event, selectedItem) {

    accountingAccountId = selectedItem.AccountingAccountId;

});


$("#GLReportAccountingNumber").on('focus', function (event) {
    if ($('#GLReportAccountingNumber').val('') == "") {
        accountingAccountId = 0;
    }   
});

$("#GLReportAccountingNumber").on('blur', function (event) {
    if ($('#GLReportAccountingNumber').val('') == "") {
        accountingAccountId = 0;
    }
});

$("#GLReportModal").on("click", function () {
    $('#modalMassiveTotRecordsGl').modal('hide');

    processTableRunningReportGL = undefined;
    clearInterval(timeReportGL);
    branchId = $("#GLReportBranch").val() == "" ? 0 : $("#GLReportBranch").val();    

    $.ajax({
        url: ACC_ROOT + "Reports/GeneralLedgerReports",
        data: {
            "branchCd": branchId, "year": $("#GLReportYear").val(),
            "monthFrom": $("#GLReportMonthFrom").val(), "accountingAccountId": accountingAccountId
        },
        success: function (data) {
            if (data.success && data.result == 0) {
                RefreshMassiveReportsGL();

                if (!isRunningReportGL) {
                    timeReportGL = window.setInterval(RefreshMassiveReportsGL, 6000);
                }
                CleanGeneralLedgerReport();
            } else {
                $("#alertGL").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertGL").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunningReportGL = undefined;
            clearInterval(timeReportGL);

            GenerateFileGL(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkGL(data.UrlFile);
    } else {
        $("#alertGL").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }
});


/*FUNCIONES*/
/*Total de Registros*/
function GetTotalRecordGL() {

    branchId = $("#GLReportBranch").val() == "" ? 0 : $("#GLReportBranch").val();    
    $.ajax({
        async: false,
        url: ACC_ROOT + "Reports/GetTotalRecordsGeneralLedger",
        data: {
            "branchCd": branchId, "year": $("#GLReportYear").val(),
            "monthFrom": $("#GLReportMonthFrom").val(), "accountingAccountId": accountingAccountId
        },
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecordsGl').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertGL").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertGL").UifAlert('show', Resources.MessageInternalError, "danger");
            }

            unlockScreen();
        }
    });
}

/*Consulta el proceso Masivo*/
function RefreshMassiveReportsGL() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.GeneralLedgerReport.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningReportGL = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningReportGL != undefined) {
        if (validateProcessReport(processTableRunningReportGL, processTableRunningReportGL.length)) {
            clearInterval(timeReportGL);
            isRunningReportGL = false;
            $("#alertGL").UifAlert('hide');
        } else {
            isRunningReportGL = true;
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
                    $("#alertGL").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    RefreshMassiveReportsGL();
                    if (!isRunningReportGL) {
                        timeReportGL = window.setInterval(RefreshMassiveReportsGL, 6000);
                    }
                }
            } else {
                $("#alertGL").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

/*Descarga el archivo*/
function SetDownloadLinkGL(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertGL", "selectFileTypePartial");
}

function CleanGeneralLedgerReport()
{
    $('#GLReportBranch').val("");
    $('#GLReportYear').val("");
    $('#GLReportMonthFrom').val("");
    $('#GLReportAccountingNumber').val("");
    $("#alertGL").UifAlert('hide');
}