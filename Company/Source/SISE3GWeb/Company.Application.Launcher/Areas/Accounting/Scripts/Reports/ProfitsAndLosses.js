var timeReportProfitsAndLosses = window.setInterval(getMassiveReportProfitsAndLosses, 6000);
var isRunningProfitsAndLossesReport;
var processTableRunninReportProfitsAndLosses;
var ReportTypeId = 0;

$(document).ready(function () {
    var controller = ACC_ROOT + "Common/GetBranchs";
    $("#ProfitsAndLossesBranch").UifSelect({ source: controller });
});

getMassiveReportProfitsAndLosses();

$("#GenerateProfitsAndLosses").on("click", function () {
    $("#alertFormPAL").UifAlert('hide');

    $("#ProfitsAndLossesReportForm").validate();
    if ($("#ProfitsAndLossesReportForm").valid()) {

            getTotalRecordProfitsAndLosses();

    }
});

$("#CancelProfitsAndLosses").on("click", function () {
    $('#ProfitsAndLossesReportYear').val("");
    $('#ProfitsAndLossesMonthFrom').val("");
    $('#ProfitsAndLossesMonthTo').val("");
    $("#alertFormPAL").UifAlert('hide');
});

$("#btnModalProfitsAndLosses").on("click", function () {
    $('#modalMassiveTotRecords').modal('hide');

    processTableRunninReportProfitsAndLosses = undefined;
    clearInterval(timeReportProfitsAndLosses);

    var parameterModel = SetModelReportPAL();
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Reports/ProfitsAndLossesReports",
        data: JSON.stringify({ "parameter": parameterModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success && data.result == 0) {
                getMassiveReportProfitsAndLosses();

                if (!isRunningProfitsAndLossesReport) {
                    timeReportProfitsAndLosses = window.setInterval(getMassiveReportProfitsAndLosses, 6000);
                }
            } else {
                $("#alertFormPAL").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$("#ProfitsAndLossesMonthFrom").on('itemSelected', function (event, selectedItem) {
    $("#alertFormPAL").UifAlert('hide');
    if ($("#ProfitsAndLossesMonthTo").val() != '') {
        if (parseInt($("#ProfitsAndLossesMonthFrom").val()) != parseInt($("#ProfitsAndLossesMonthTo").val())) {
            if (parseInt($("#ProfitsAndLossesMonthFrom").val()) > parseInt($("#ProfitsAndLossesMonthTo").val())) {
                $("#alertFormPAL").UifAlert('show', Resources.MessageValidateMonthFrom, "warning");
                $("#ProfitsAndLossesMonthFrom").val('');
            }
        }
    }
});

$("#ProfitsAndLossesMonthTo").on('itemSelected', function (event, selectedItem) {
    $("#alertFormPAL").UifAlert('hide');
    if ($("#ProfitsAndLossesMonthFrom").val() != '') {
        if (parseInt($("#ProfitsAndLossesMonthTo").val()) != parseInt($("#ProfitsAndLossesMonthFrom").val())) {
            if (parseInt($("#ProfitsAndLossesMonthTo").val()) < parseInt($("#ProfitsAndLossesMonthFrom").val())) {
                $("#alertFormPAL").UifAlert('show', Resources.MessageValidateMonthTo, "warning");
                $("#ProfitsAndLossesMonthTo").val('');
            }
        }
    }
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertFormPAL").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunninReportProfitsAndLosses = undefined;
            clearInterval(timeReportProfitsAndLosses);

            generateFile(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkPAL(data.UrlFile);
    } else {
        $("#alertFormPAL").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

});

$('#checkAccumulatedPAL').click(function () {
    $("#alertFormPAL").UifAlert('hide');
    if ($('#checkAccumulatedPAL').hasClass('glyphicon glyphicon-check')) {
        uncheckPAL('#checkAccumulatedPAL');
    }
    else {
        checkPAL('#checkAccumulatedPAL');
    }
});

$("#ProfitsAndLossesBranch").on('binded', function (event, selectedItem) {
    //$("#ProfitsAndLossesBranch").attr("disabled", "disabled");
});


function getTotalRecordProfitsAndLosses() {
    var parameterModel = SetModelReportPAL();
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Reports/GetTotalRecordsProfitsAndLosses",
        data: JSON.stringify({ "parameter": parameterModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecords').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertFormPAL").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertFormPAL").UifAlert('show', Resources.MessageInternalError, "danger");
            }
        }
    });
}

function getMassiveReportProfitsAndLosses() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.ProfitsAndLosses.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunninReportProfitsAndLosses = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunninReportProfitsAndLosses != undefined) {
        if (validateProcessReport(processTableRunninReportProfitsAndLosses, processTableRunninReportProfitsAndLosses.length)) {
            clearInterval(timeReportProfitsAndLosses);
            isRunningProfitsAndLossesReport = false;
            $("#alertFormPAL").UifAlert('hide');
        } else {
            isRunningProfitsAndLossesReport = true;
        }
    }
};

function generateFile(processId, records, reportDescription) {
    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertFormPAL").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    getMassiveReportProfitsAndLosses();
                    if (!isRunningProfitsAndLossesReport) {
                        timeReportProfitsAndLosses = window.setInterval(getMassiveReportProfitsAndLosses, 6000);
                    }
                }
            } else {
                $("#alertFormPAL").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function SetDownloadLinkPAL(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertFormPAL", "selectFileTypePartial");
}


function checkPAL(checkAccount) {
    $(checkAccount).removeClass("glyphicon glyphicon-unchecked");
    $(checkAccount).addClass("glyphicon glyphicon-check");
}

function uncheckPAL(checkAccount) {
    $(checkAccount).removeClass("glyphicon glyphicon-check");
    $(checkAccount).addClass("glyphicon glyphicon-unchecked");
}


function SetModelReportPAL() {
    return {
        Branch: $("#ProfitsAndLossesBranch").val(),
        Month: $("#ProfitsAndLossesMonthFrom").val(),
        Year: $("#ProfitsAndLossesReportYear").val(),
        MonthTo: $("#ProfitsAndLossesMonthTo").val(),
        Income: $("#ViewBagIncome").val(),
        Expenses: $("#ViewBagExpenses").val(),
        //Operation: $('input:radio[name=options]:checked').val() == 'D' ? 0 : 1,
        Accumulated: ($('#checkAccumulatedPAL').hasClass('glyphicon glyphicon-check')) ? 1 : 0,
        Name: Resources.ProfitsAndLosses
    }
}
