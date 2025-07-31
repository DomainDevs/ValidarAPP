var massiveRecordCode = 0;
var recordsNumber = 0;
var reportTypeDescription;
var urlFileName;
var dateFrom;
var dateTo;
var reportName;
var processType = 1;
var totalRecords = 0;
var recordsProcessed = 0;
var processTableRunningReinsureReport;
var tabletemporal;
var isRunningReinsureReport = false;
var time;

$("#pnlDates").hide();
$("#pnlAccount").hide();

$('#dateFrom').on('focusout', function (event, date) {
    $("#alertPrint").UifAlert('hide');
    if ($("#dateTo").val() != "") {
        if (CompareDates($('#dateFrom').val(), $("#dateTo").val()) == 1) {
            $("#alertPrint").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#dateFrom").val('');
        }
    } else {
        $("#alertPrint").UifAlert('show', Resources.MessageInputDatesFileds, "danger");
    }
});

$('#dateTo').on('focusout', function (event, date) {
    $("#alertPrint").UifAlert('hide');
    if ($("#dateFrom").val() != "") {
        if (CompareDates($("#dateFrom").val(), $('#dateTo').val()) == 1) {
            $("#alertPrint").UifAlert('show', Resources.ValidateDateFrom, "danger");
            $("#dateTo").val('');
        }
    } else {
        $("#alertPrint").UifAlert('show', Resources.MessageInputDatesFileds, "danger");
    }
});

$('#selectTypeProcess').on('itemSelected', function (event, selectedItem) {
    $("#alertPrint").UifAlert('hide');
    $("#tableMassiveProcess").UifDataTable('clear');
    if (selectedItem.Id != "") {
        reportName = selectedItem.Text;
        getReinsureReport();
        if (selectedItem.Id == 3 || selectedItem.Id == 10 || selectedItem.Id == 11 || selectedItem.Id == 12) {
            $("#pnlAccount").show();
            $("#pnlDates").hide();
        } else {
            $("#pnlAccount").hide();
            $("#pnlDates").show();
        }
    } else {
        $("#pnlDates").hide();
        $("#pnlAccount").hide();
    }
});

$("#btnReinsuranceMassiveReport").click(function () {
    $("#alertPrint").UifAlert('hide');
    $("#ReinsuranceForm").validate();
    if ($("#ReinsuranceForm").valid()) {
        if ($("#selectTypeProcess").val() != "") {
            lockScreen();
            if ($('#pnlAccount').is(":visible")) {
                if ($("#year").val() == "" || $("#monthReinsurance").val() == "") {
                    $("#alertPrint").UifAlert('show', Resources.ConfirmaData, "warning");
                } else {
                    dateFrom = "01/" + $("#monthReinsurance").val() + "/" + $("#year").val();
                    dateTo = "01/01/2050";
                    getTotalRecordsMassiveReport();
                }
            } else {
                if ($("#dateFrom").val() != "" && $("#dateTo").val() != "") {
                    if (IsDate($("#dateFrom").val()) == false || IsDate($("#dateTo").val()) == false) {
                        $("#alertPrint").UifAlert('show', Resources.InvalidDates, "warning");
                    } else {
                        if (CompareDates($("#dateFrom").val(), $("#dateTo").val()) == 2) {
                            if ($("#dateFrom").val() == $("#dateTo").val()) {
                                dateFrom = $("#dateFrom").val();
                                dateTo = $("#dateTo").val();
                                getTotalRecordsMassiveReport();
                            } else {
                                $("#alertPrint").UifAlert('show', Resources.ValidateDateFrom, "warning");
                            }
                        } else {
                            dateFrom = $("#dateFrom").val();
                            dateTo = $("#dateTo").val();
                            getTotalRecordsMassiveReport();
                        }
                    }
                } else {
                    $("#alertPrint").UifAlert('show', Resources.ConfirmaData, "warning");
                }
            }
        }
        else {
            $("#alertPrint").UifAlert('show', Resources.MessageSelectProcessType, "warning");
        }
    }
});

$("#btnModalMassiveReinsured").click(function () {
    $('#modalMassiveTotRecordsReinsured').modal('hide');
    lockScreen();
    processTableRunningReinsureReport = null;
    processType = 0;

    var promise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: REINS_ROOT + 'Process/ReinsuranceReports',
            data: JSON.stringify({ dateFrom: dateFrom, dateTo: dateTo, reportType: $("#selectTypeProcess").val() }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        }).done(function (data) {
            resolve(data);
        }).fail(function (response) {
            reject(response);
        });
    });
    promise.then(function (data) {
        if (data.success) {
            processType = 0;
            getReinsureReport();
        }
        else {
            $("#alertPrint").UifAlert('show', data.result, "danger");
        }
        unlockScreen();
    }, function (response) {
        $("#alertPrint").UifAlert('show', response.result, "danger");
        unlockScreen();
    });

});

$("#btnModalMassiveNoReinsured").click(function () {
    $('#modalMassiveTotRecordsReinsured').modal('hide');
});

$("#btnCancelReinsure").click(function () {
    $("#pnlDates").hide();
    $("#pnlAccount").hide();
    $("#alertPrint").UifAlert('hide');
    $("#tableMassiveProcess").UifDataTable('clear');
    $("#selectTypeProcess").val("");
    $("#dateFrom").val("");
    $("#dateTo").val("");
    $("#year").val("");
    $("#monthReinsurance").val("");
    $("#selectFileTypeReinsurance").val("");
});

$('#tableMassiveProcess').on('rowEdit', function (event, data, position) {
    if (data.RecordsNumber == data.RecordsProcessed && data.RecordsNumber == 0) {
        $("#alertPrint").UifAlert('show', Resources.NoRecordsFound, "warning");
    } else {
        massiveRecordCode = data.Id;
        recordsNumber = data.RecordsNumber;
        reportTypeDescription = data.Description;
        urlFileName = data.UrlFile;
        whenSelectReport(data);
    }
});

function generateFileReport() {
    var promise = new Promise(function (resolve, reject) {
        $.ajax({
            type: 'POST',
            url: REINS_ROOT + "Process/GenerateStructureReport",
            data: JSON.stringify({
                processId: massiveRecordCode,
                reportTypeDescription: reportTypeDescription,
                exportFormatType: $("#selectFileTypeReinsurance").val(),
                recordsNumber: recordsNumber
            }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            resolve(data);
        }).fail(function (response) {
            reject(response);
        });
    });
    promise.then(function (data) {
        if (data.success) {
            if (data.result == "-1") {
                $("#alertPrint").UifAlert('show', Resources.MessageNotDataTemplateParametrization, "warning");
            }
            else {
                getReinsureReport();
            }
        } else {
            $("#alertPrint").UifAlert('show', data.result, "danger");
        }
    }, function (response) {
        $("#alertPrint").UifAlert('show', response.result, "danger");
    });
}

function SetDownloadLinkReinsurance(fileName) {
    if (REINS_ROOT.length > 13) {
        var url = REINS_ROOT.replace("Reinsurance/", "")
        window.open(url + fileName);
    }
    else {
        window.open(REINS_ROOT + fileName);
    }
}

function getTotalRecordsMassiveReport() {
    totalRecords = 0;
    recordsProcessed = 0;

    var promise = new Promise(function (resolve, reject) {
        $.ajax({
            type: 'POST',
            url: REINS_ROOT + "Process/GetTotalRecordsMassiveReport",
            data: JSON.stringify({
                dateFrom: dateFrom,
                dateTo: dateTo,
                reportType: $("#selectTypeProcess").val()
            }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        }).done(function (data) {
            resolve(data);
        }).fail(function (response) {
            reject(response);
        });
    });

    promise.then(function (data) {
        if (data.success) {
            totalRecords = data.result;
            $('#pnlModalTotRecordsReinsured').text(Resources.MsgMassiveTotRecordsAprox + ': ' + totalRecords + ' .' + Resources.MsgWantContinue);
            if (totalRecords > 0) {
                $('#modalMassiveTotRecordsReinsured').appendTo("body").UifModal('showLocal');
            } else {
                $("#alertPrint").UifAlert('show', Resources.MsgTotRecordsToProcess, "warning");
            }
        }
        else {
            $("#alertPrint").UifAlert('show', data.result, "danger");
        }
        unlockScreen();
    }, function (response) {
        $("#alertPrint").UifAlert('show', response.result, "danger");
        unlockScreen();
    });
}

function whenSelectReport(data) {
    $("#alertPrint").UifAlert('hide');
    var isGenerateIndex = data.UrlFile.indexOf("/");
    if (data.RecordsNumber == data.RecordsProcessed && data.RecordsNumber > 0 && isGenerateIndex == -1 && data.UrlFile != Resources.GenerateDocument) {
        $("#reportForm").validate();
        if ($("#reportForm").valid()) {
            generateFileReport();
        }
    }
    else if (isGenerateIndex > 0) {
        SetDownloadLinkReinsurance(data.UrlFile);
    }
    else {
        $("#alertPrint").UifAlert('show', Resources.GenerationReportProcess, "danger");
    }
}

function getReinsureReport() {
    var promise = new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: REINS_ROOT + "Process/GetMassiveReportProcess",
            data: JSON.stringify({ reportName: reportName }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        }).done(function (data) {
            resolve(data);
        }).fail(function (response) {
            reject(response);
        });
    });
    promise.then(function (data) {
        if (data.success) {
            if (data.result.length > 0) {
                clearTimeout(time);
                $("#tableMassiveProcess").UifDataTable({ sourceData: data.result });
                processTableRunningReinsureReport = data.result;
                if (processTableRunningReinsureReport != null) {
                    if (validateProcessReport(processTableRunningReinsureReport, processTableRunningReinsureReport.length)) {
                        clearTimeout(time);
                        isRunningReinsureReport = false;
                        $("#alertPrint").UifAlert('hide');
                    }
                    else {
                        isRunningReinsureReport = true;
                        time = setTimeout(() => getReinsureReport(), 10000);
                    }
                }
            } else {
                $("#alertPrint").UifAlert('show', Resources.NoRecordsFound, "warning");
                time = setTimeout(() => getReinsureReport(), 2500);
            }
        } else {
            $("#alertPrint").UifAlert('show', data.result, "warning");
        }
    }, function (response) {
        $("#alertPrint").UifAlert('show', response.result, "warning");
    });
};
