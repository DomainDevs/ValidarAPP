$(() => {
    new ExchangeDifference();
});

var time;

class ExchangeDifference extends Uif2.Page {
    getInitialState() {
        CheckDate();
    }
    bindEvents() {
        $("#processExchangeDifference").on('click', this.GenerateExchangeDifference);
        $("#StartDate").on('blur', this.CheckValidStartDate);
        $("#EndDate").on('blur', this.CheckValidEndDate);
        $("#StartDate").on('datepicker.change', this.ValidateStartDate);
        $("#EndDate").on('datepicker.change', this.ValidateEndDate);
        $("#printExchangeDifference").on('click', this.PrintReport);
        $("#accountExchangeDifference").on('click', this.AccountExchangeDifference);
    }
    GenerateExchangeDifference() {
        var year = $("#Year").val();
        CheckClosedModule(year).then(function (checkData) {
            lockScreen();
            if (checkData.result) {
                $("#ExchangeDifferenceAlert").UifAlert('hide');
                $("#ExchangeDifferenceForm").validate();
                if ($("#ExchangeDifferenceForm").valid()) {
                    var startDate = $("#StartDate").val();
                    var endDate = $("#EndDate").val();
                    GetExchangeDifference(startDate, endDate).then(function (exchangeDifferenceData) {
                        if (exchangeDifferenceData > 0) {
                            $("#ExchangeDifferenceAlert").UifAlert('show', Resources.MessageFinalizedProcess, "success");
                            $("#printExchangeDifference").attr("disabled", false);
                        }
                    });
                }
            } else {
                $("#ExchangeDifferenceAlert").UifAlert('show', Resources.Month_Not_Closed_Warning, "warning");
            }
            $.unblockUI();
        });
    }
    CheckValidStartDate() {
        if ($("#StartDate").val() != '') {

            if (IsDate($("#StartDate").val()) == true) {
                if ($("#EndDate").val() != '') {
                    if (CompareDates($("#StartDate").val(), $("#EndDate").val())) {
                        $("#StartDate").val(getCurrentDate);
                    }
                }
            } else {
                $("#ExchangeDifferenceAlert").UifAlert('show', Resources.InvalidDates, "danger");
                $("#StartDate").val("");
            }
        }
    }
    CheckValidEndDate() {
        if ($("#EndDate").val() != '') {

            if (IsDate($("#EndDate").val()) == true) {
                if ($("#StartDate").val() != '') {
                    if (!CompareDates($("#EndDate").val(), $("#StartDate").val())) {
                        $("#EndDate").val(getCurrentDate);
                    }
                }
            } else {
                $("#ExchangeDifferenceAlert").UifAlert('show', Resources.InvalidDates, "danger");
                $("#EndDate").val("");
            }
        }
    }
    ValidateStartDate() {
        if ($("#EndDate").val() != "") {

            if (compare_dates($('#StartDate').val(), $("#EndDate").val())) {

                $("#ExchangeDifferenceAlert").UifAlert('show', Resources.ValidateDateTo, "warning");

                $("#StartDate").val('');
            } else {
                $("#StartDate").val($('#StartDate').val());
            }
        }
    }
    ValidateEndDate() {
        if ($("#StartDate").val() != "") {
            if (compare_dates($("#StartDate").val(), $('#EndDate').val())) {

                $("#ExchangeDifferenceAlert").UifAlert('show', Resources.ValidateDateFrom, "warning");

                $("#EndDate").val('');
            } else {
                $("#EndDateBillEndDateSearch").val($('#EndDate').val());
            }
        }
    }
    PrintReport() {
        PrintExchangeDifferenceReport().then(function (printData) {
            if (printData) {
                ShowExchangeDifferenceReport();
                $("#accountExchangeDifference").attr("disabled", false);
            }
        });
    }
    AccountExchangeDifference() {
        lockScreen();
        AccountExchangeDifferenceRecords().then(function (entryNumber) {
            if (entryNumber > 0) {
                $("#ExchangeDifferenceAlert").UifAlert('show', Resources.MessageAccountingEntry + " " + entryNumber, "success");
            } else {
                $("#ExchangeDifferenceAlert").UifAlert('show', Resources.EntrySavedFailed, "danger");
            }
        });
        $.unblockUI();
    }
}

function GetExchangeDifference(startDate, endDate) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACL_ROOT + "AccountingClosing/GetExchangeDifference",
            data: JSON.stringify({
                "startDate": startDate,
                "endDate": endDate
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (exchangeDifferenceData) {
                resolve(exchangeDifferenceData);
            }
        });
    });
}

function CheckClosedModule(year) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACL_ROOT + "AccountingClosing/CheckClosedModules",
            data: JSON.stringify({
                "year": year
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (checkData) {
                resolve(checkData);
            }
        });
    });
}

function getActualDate() {
    if ($("#ViewBagImputationType").val() == undefined && $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
        $("#ViewBagBillControlId").val() == undefined) {
        $.ajax({
            url: ACL_ROOT + "Base/GetDate",
            success: function (data) {
                $("#Year").val(data.substring(6, 10));
            }
        });
    }
}

function CheckDate() {
    time = setInterval(function () {
        if ($("#Year").val() == "") {
            getActualDate();
        } else {
            clearTimeout(time);
        }
    }, 300);
}

function PrintExchangeDifferenceReport() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACL_ROOT + "AccountingClosing/GenerateExchangeDifferenceReport",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (printData) {
                resolve(printData);
            }
        });
    });
}

function ShowExchangeDifferenceReport() {
    window.open(ACL_ROOT + "AccountingClosing/ShowExchangeDifferenceReport", 'Windows',
        'fullscreen=yes, scrollbars=auto');
}

function AccountExchangeDifferenceRecords() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: ACL_ROOT + "AccountingClosing/AccountExchangeDifferenceRecords",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (entryNumber) {
                resolve(entryNumber);
            }
        });
    });
}

