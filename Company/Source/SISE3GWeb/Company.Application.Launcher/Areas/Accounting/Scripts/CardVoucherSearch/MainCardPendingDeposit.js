/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                               DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                                                 */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/

var status = -1;

/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                                          ACCIONES / EVENTOS                                                              */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#SearchCardBranchCardPendingDeposit").attr("disabled", "disabled");
    }, 300);

}
else {
    $("#SearchCardBranchCardPendingDeposit").removeAttr("disabled");
}

//Valida que no ingresen una fecha invalida.
$("#StartDateCardPendingDeposit").blur(function () {
    $("#alertCardVoucherPendingDeposit").UifAlert('hide');
    if ($("#StartDateCardPendingDeposit").val() != '') {

        if (IsDate($("#StartDateCardPendingDeposit").val()) == true) {
            if ($("#EndDateCardPendingDeposit").val() != '') {
                if (CompareDates($("#StartDateCardPendingDeposit").val(), $("#EndDateCardPendingDeposit").val())) {
                    $("#StartDateCardPendingDeposit").val("");
                }
            }
        }
        else {
            $("#alertCardVoucherPendingDeposit").UifAlert('show', Resources.InvalidDates, "warning");
            $("#StartDateCardPendingDeposit").val("");
        }
    }
});


//Valida que no ingresen una fecha invalida.
$("#EndDateCardPendingDeposit").blur(function () {
    $("#alertCardVoucherPendingDeposit").UifAlert('hide');

    if ($("#EndDateCardPendingDeposit").val() != '') {
        if (IsDate($("#EndDateCardPendingDeposit").val()) == true) {

            if ($("#StartDateCardPendingDeposit").val() != '') {
                if (CompareDates($("#StartDateCardPendingDeposit").val(), $("#EndDateCardPendingDeposit").val())) {
                    $("#EndDateCardPendingDeposit").val("");
                }
            }
        }
        else {
            $("#alertCardVoucherPendingDeposit").UifAlert('show', Resources.InvalidDates, "warning");
            $("#EndDateCardPendingDeposit").val("");
        }
    }
});


$("#SearchCardPendingDeposit").click(function () {
    $("#alertCardVoucherPendingDeposit").UifAlert('hide');
    $("#MainCardPendingDepositForm").validate();

    if ($("#MainCardPendingDepositForm").valid()) {

        status = Resources.PaymentStatusActive;

        $("#ReportCardPendingDeposit").show();

        var creditCardTypeCode = "";
        var voucher = "";
        var documentNumber = "";
        var technicalTransaction = "";
        var branchCode = "";
        var startDate = "";
        var endDate = "";

        creditCardTypeCode = $('#CreditCardTypeCardPendingDeposit').val();
        voucher = $('#CardVoucherNumberCardPendingDeposit').val();
        documentNumber = $('#CardCardNumberCardPendingDeposit').val();
        technicalTransaction = $("#CardBillNumberCardPendingDeposit").val();
        branchCode = $("#SearchCardBranchCardPendingDeposit").val();
        startDate = $("#StartDateCardPendingDeposit").val();
        endDate = $("#EndDateCardPendingDeposit").val();

        var controller = ACC_ROOT + "CardVoucherSearch/GetCardVoucher?creditCardTypeCode=" + creditCardTypeCode
                      + "&voucher=" + voucher + "&documentNumber=" + documentNumber + "&technicalTransaction=" + technicalTransaction
                      + "&branchCode=" + branchCode + "&startDate=" + startDate + "&endDate=" + endDate + "&status=" + status;

        $("#CardVoucherCardPendingDeposit").UifDataTable({ source: controller });
    }
});

$("#CleanCardPendingDeposit").click(function () {
    $("#alertCardVoucherPendingDeposit").UifAlert('hide');
    cleanFieldsCardPendingDeposit();
    $("#CreditCardTypeCardPendingDeposit").UifSelect();
    $('#CreditCardTypeCardPendingDeposit-error').remove();

});

$("#ReportCardPendingDeposit").click(function () {
    $("#alertCardVoucherPendingDeposit").UifAlert('hide');
    if ($("#CreditCardTypeCardPendingDeposit").val() != "") {
        status = Resources.PaymentStatusActive;
        loadReportCardPendingDeposit($("#CreditCardTypeCardPendingDeposit").val(),
                                     $("#CardVoucherNumberCardPendingDeposit").val(),
                                     $("#CardCardNumberCardPendingDeposit").val(),
                                     $("#CardBillNumberCardPendingDeposit").val(),
                                     $("#SearchCardBranchCardPendingDeposit").val(),
                                     $("#StartDateCardPendingDeposit").val(),
                                     $("#EndDateCardPendingDeposit").val(), status);
        //Muestra reporte cargado
        showReportCardPendingDeposit();
    }
    else {
        $("#alertCardVoucherPendingDeposit").UifAlert('show', Resources.InternalBallotCardDialogMessageEdit, "danger");
    }
});

/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES                                                                                */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
function cleanFieldsCardPendingDeposit() {
    $("#alertCardVoucherPendingDeposit").UifAlert('hide');
    $("#CreditCardTypeCardPendingDeposit").val("");
    $("#CardVoucherNumberCardPendingDeposit").val("");
    $("#CardCardNumberCardPendingDeposit").val("");
    $("#CardBillNumberCardPendingDeposit").val("");
    $("#SearchCardBranchCardPendingDeposit").val("");
    $("#StartDateCardPendingDeposit").val("");
    $("#EndDateCardPendingDeposit").val("");
    $("#CardVoucherCardPendingDeposit").dataTable().fnClearTable();
    $("#btnReject").hide();
}

function showReportCardPendingDeposit() {
    $("#alertCardVoucherPendingDeposit").UifAlert('hide');
    window.open(ACC_ROOT + "Report/ShowCardsDepositingPendingReport", 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

//Carga reporte
function loadReportCardPendingDeposit(creditCardTypeCode, voucher, documentNumber, technicalTransaction, branchCode, startDate, endDate, status) {
    $("#alertCardVoucherPendingDeposit").UifAlert('hide');

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Report/LoadCardsDepositingPendingReport",
        data: {
            "creditCardTypeCode": creditCardTypeCode, "voucher": voucher, "documentNumber": documentNumber, "billCode": technicalTransaction,
            "branchCode": branchCode, "startDate": startDate, "endDate": endDate, "status": status
        },
        success: function (data) { }
    });
}

