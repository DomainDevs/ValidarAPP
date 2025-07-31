var timeReportFb = window.setInterval(getMassiveReportFb, 6000);
var isRunningReportFb;
var processTableRunningReportFb;

getMassiveReportFb();

$("#FBReportGenerate").on("click", function () {
    $("#alertFb").UifAlert('hide');

    $("#FBReportForm").validate();
    if ($("#FBReportForm").valid()) {
        if (validateUnChekAllFb()) {
            $("#alertFb").UifAlert('show', Resources.WarningSelectOneAccount, "warning");
        } else {

            lockScreen();

            setTimeout(function () {
                getTotalRecordFb();
            }, 300);
        }
    }
});

$("#FBReportCancel").on("click", function () {
    $('#FBReportYear').val("");
    $('#FBReportMonthFrom').val("");
    $('#FBReportMonthTo').val("");
    $("#alertFb").UifAlert('hide');
});

$("#FBReportModal").on("click", function () {
    $('#modalMassiveTotRecordsFb').modal('hide');

    processTableRunningReportFb = undefined;
    clearInterval(timeReportFb);

    var paramterModel = SetModelReportFb();
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Reports/BalanceSheetReports",
        data: JSON.stringify({ "parameter": paramterModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success && data.result == 0) {
                getMassiveReportFb();

                if (!isRunningReportFb) {
                    timeReportFb = window.setInterval(getMassiveReportFb, 6000);
                }
            } else {
                $("#alertFb").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
});

$("#FBReportMonthFrom").on('itemSelected', function (event, selectedItem) {
    $("#alertFb").UifAlert('hide');
    if ($("#FBReportMonthTo").val() != '') {
        if (parseInt($("#FBReportMonthFrom").val()) != parseInt($("#FBReportMonthTo").val())) {
            if (parseInt($("#FBReportMonthFrom").val()) > parseInt($("#FBReportMonthTo").val())) {
                $("#alertFb").UifAlert('show', Resources.MessageValidateMonthFrom, "warning");
                $("#FBReportMonthFrom").val('');
            }
        }
    }
});

$("#FBReportMonthTo").on('itemSelected', function (event, selectedItem) {
    $("#alertFb").UifAlert('hide');
    if ($("#FBReportMonthFrom").val() != '') {
        if (parseInt($("#FBReportMonthTo").val()) != parseInt($("#FBReportMonthFrom").val())) {
            if (parseInt($("#FBReportMonthTo").val()) < parseInt($("#FBReportMonthFrom").val())) {
                $("#alertFb").UifAlert('show', Resources.MessageValidateMonthTo, "warning");
                $("#FBReportMonthTo").val('');
            }
        }
    }
});

$('#tableMassiveProcessReport').on('rowEdit', function (event, data, position) {
    $("#alertFb").UifAlert('hide');
    var progresNum = data.Progress.split(" ");
    var isGenerateIndex = data.UrlFile.indexOf("/");

    if (progresNum[0] == "100.00" && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#AccountingReportsForm").validate();
        if ($("#AccountingReportsForm").valid()) {
            processTableRunningReportFb = undefined;
            clearInterval(timeReportFb);

            generateFileFb(data.ProcessId, data.RecordsNumber, data.Description);
        }

    } else if (isGenerateIndex > 0) {
        SetDownloadLinkFb(data.UrlFile);
    } else {
        $("#alertFb").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }

});

$('#checkAllFb').click(function () {
    $("#alertFb").UifAlert('hide');
    if ($('#checkAllFb').hasClass('glyphicon glyphicon-check')) {
        uncheckFb('#checkAllFb');

        if ($('#checkAssetsFb').hasClass('glyphicon glyphicon-check')) {
            uncheckFb('#checkAssetsFb');
        }
        if ($('#checkLiabilitiesFb').hasClass('glyphicon glyphicon-check')) {
            uncheckFb('#checkLiabilitiesFb');
        }
        if ($('#checkPatrimonyFb').hasClass('glyphicon glyphicon-check')) {
            uncheckFb('#checkPatrimonyFb');
        }
        if ($('#checkIncomeFb').hasClass('glyphicon glyphicon-check')) {
            uncheckFb('#checkIncomeFb');
        }
        if ($('#checkExpensesFb').hasClass('glyphicon glyphicon-check')) {
            uncheckFb('#checkExpensesFb');
        }
    }
    else {
        checkFb('#checkAllFb');
        if ($('#checkAssetsFb').hasClass('glyphicon glyphicon-unchecked')) {
            checkFb('#checkAssetsFb');
        }
        if ($('#checkLiabilitiesFb').hasClass('glyphicon glyphicon-unchecked')) {
            checkFb('#checkLiabilitiesFb');
        }
        if ($('#checkPatrimonyFb').hasClass('glyphicon glyphicon-unchecked')) {
            checkFb('#checkPatrimonyFb');
        }
        if ($('#checkIncomeFb').hasClass('glyphicon glyphicon-unchecked')) {
            checkFb('#checkIncomeFb');
        }
        if ($('#checkExpensesFb').hasClass('glyphicon glyphicon-unchecked')) {
            checkFb('#checkExpensesFb');
        }
    }
});

$('#checkAssetsFb').click(function () {
    $("#alertFb").UifAlert('hide');
    if ($('#checkAssetsFb').hasClass('glyphicon glyphicon-check')) {
        uncheckFb('#checkAssetsFb');
        uncheckFb('#checkAllFb');
    }
    else {
        checkFb('#checkAssetsFb');
        if (validateChekAllFb()) {
            checkFb('#checkAllFb');
        }
    }
});

$('#checkLiabilitiesFb').click(function () {
    $("#alertFb").UifAlert('hide');
    if ($('#checkLiabilitiesFb').hasClass('glyphicon glyphicon-check')) {
        uncheckFb('#checkLiabilitiesFb');
        uncheckFb('#checkAllFb');
    }
    else {
        checkFb('#checkLiabilitiesFb');
        if (validateChekAllFb()) {
            checkFb('#checkAllFb');
        }
    }
});

$('#checkPatrimonyFb').click(function () {
    $("#alertFb").UifAlert('hide');
    if ($('#checkPatrimonyFb').hasClass('glyphicon glyphicon-check')) {
        uncheckFb('#checkPatrimonyFb');
        uncheckFb('#checkAllFb');
    }
    else {
        checkFb('#checkPatrimonyFb');
        if (validateChekAllFb()) {
            checkFb('#checkAllFb');
        }
    }
});

$('#checkIncomeFb').click(function () {
    $("#alertFb").UifAlert('hide');
    if ($('#checkIncomeFb').hasClass('glyphicon glyphicon-check')) {
        uncheckFb('#checkIncomeFb');
        uncheckFb('#checkAllFb');
    }
    else {
        checkFb('#checkIncomeFb');
        if (validateChekAllFb()) {
            checkFb('#checkAllFb');
        }
    }
});

$('#checkExpensesFb').click(function () {
    $("#alertFb").UifAlert('hide');
    if ($('#checkExpensesFb').hasClass('glyphicon glyphicon-check')) {
        uncheckFb('#checkExpensesFb');
        uncheckFb('#checkAllFb');
    }
    else {
        checkFb('#checkExpensesFb');
        if (validateChekAllFb()) {
            checkFb('#checkAllFb');
        }
    }
});


function getTotalRecordFb() {
    var paramterModel = SetModelReportFb();
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Reports/GetTotalRecordsBalanceSheet",
        data: JSON.stringify({ "parameter": paramterModel }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.success) {
                var msj = Resources.MsgMassiveTotRecords + ': ' + data.result.records + ' ' + Resources.MsgWantContinue;
                $('#pnlModalTotRecords').text(msj);
                if (data.result.records > 0) {
                    $('#modalMassiveTotRecordsFb').appendTo("body").UifModal('showLocal');
                } else {
                    msj = Resources.MsgTotRecordsToProcess;
                    $("#alertFb").UifAlert('show', msj, "warning");
                }
            }
            else {
                $("#alertFb").UifAlert('show', Resources.MessageInternalError, "danger");
            }
            unlockScreen();
        }
    });
}

function getMassiveReportFb() {
    var controller = ACC_ROOT + "Reports/GetReportProcess?reportName=" + Resources.FinalBalance.toUpperCase();
    $("#tableMassiveProcessReport").UifDataTable({ source: controller });

    setTimeout(function () {
        processTableRunningReportFb = $('#tableMassiveProcessReport').UifDataTable('getData');
    }, 1000);

    if (processTableRunningReportFb != undefined) {
        if (validateProcessReport(processTableRunningReportFb, processTableRunningReportFb.length)) {
            clearInterval(timeReportFb);
            isRunningReportFb = false;
            $("#alertFb").UifAlert('hide');
        } else {
            isRunningReportFb = true;
        }
    }
};

function generateFileFb(processId, records, reportDescription) {
    $.ajax({
        url: ACC_ROOT + "Reports/GenerateStructureReportMassive",
        data: {
            "processId": processId, "reportTypeDescription": reportDescription,
            "exportFormatType": $("#selectFileTypePartial").val(), "recordsNumber": records
        },
        success: function (data) {
            if (data[0].ErrorInfo == null) {
                if (data[0].ExportedFileName == "-1") {
                    $("#alertFb").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
                }
                else {
                    getMassiveReportFb();
                    if (!isRunningReportFb) {
                        timeReportFb = window.setInterval(getMassiveReportFb, 6000);
                    }
                }
            } else {
                $("#alertFb").UifAlert('show', Resources.ErrorStoreProcedure, "danger");
            }
        }
    });
}

function SetDownloadLinkFb(fileName) {
    var url_path = '';
    if (ACC_ROOT.length > 13) {
        var url = ACC_ROOT.replace("Accounting/", "")
        url_path = url + fileName;
    }
    else {
        url_path = ACC_ROOT + fileName;
    }
    reportFileExist(url_path, "alertFb", "selectFileTypePartial");
}

function checkFb(checkAccount) {
    $(checkAccount).removeClass("glyphicon glyphicon-unchecked");
    $(checkAccount).addClass("glyphicon glyphicon-check");
}

function uncheckFb(checkAccount) {
    $(checkAccount).removeClass("glyphicon glyphicon-check");
    $(checkAccount).addClass("glyphicon glyphicon-unchecked");
}

function validateChekAllFb() {
    if ($('#checkAssetsFb').hasClass('glyphicon glyphicon-check') && $('#checkLiabilitiesFb').hasClass('glyphicon glyphicon-check') &&
        $('#checkPatrimonyFb').hasClass('glyphicon glyphicon-check') && $('#checkIncomeFb').hasClass('glyphicon glyphicon-check') &&
        $('#checkExpensesFb').hasClass('glyphicon glyphicon-check')) {
        return true;
    }
    else {
        return false
    }
}

function validateUnChekAllFb() {
    if ($('#checkAssetsFb').hasClass('glyphicon glyphicon-unchecked') && $('#checkLiabilitiesFb').hasClass('glyphicon glyphicon-unchecked') &&
        $('#checkPatrimonyFb').hasClass('glyphicon glyphicon-unchecked') && $('#checkIncomeFb').hasClass('glyphicon glyphicon-unchecked') &&
        $('#checkExpensesFb').hasClass('glyphicon glyphicon-unchecked')) {
        return true;
    }
    else {
        return false
    }
}

function SetModelReportFb() {
    return {
        Branch: $("#FBReportBranch").val(),
        Month: $("#FBReportMonthFrom").val(),
        Year: $("#FBReportYear").val(),
        MonthTo: $("#FBReportMonthTo").val(),
        Assets: ($('#checkAssetsFb').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagAssetsFb").val() : 0,
        Liabilities: ($('#checkLiabilitiesFb').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagLiabilitiesFb").val() : 0,
        Patrimony: ($('#checkPatrimonyFb').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagPatrimonyFb").val() : 0,
        Income: ($('#checkIncomeFb').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagIncomeFb").val() : 0,
        Expenses: ($('#checkExpensesFb').hasClass('glyphicon glyphicon-check')) ? $("#ViewBagExpensesFb").val() : 0,
        Accumulated: 1,
        Name: Resources.FinalBalance
    }
}

