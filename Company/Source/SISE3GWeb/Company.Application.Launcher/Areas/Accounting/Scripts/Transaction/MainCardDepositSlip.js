
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
var AccountCurrencyVariable;
var ReceivingBankVariable;
var percentageMinQuota = $("#ViewBagPercentageParameter").val();
var userId = $("#ViewBagUserId").val();
var currencyId = 0;

var oPaymentBallotModel = {
    PaymentBallotId: 0,
    PaymentBallotNumber: null,
    PaymentAccountNumber: null,
    PaymentBallotBankId: 0,
    PaymentBallotAmount: 0,
    PaymentBallotBankAmount: 0,
    PaymentCurrency: null,    
    PaymentBankDate: null,
    PaymentStatus: 0,

    PaymentTicketBallotModels: []
};

var oPaymentTicketBallotModel = {
    PaymentTicketBallotId: 0
};

var IssuingBankId = -1;

//FORMATOS/#CARACTERES
$("#BankTotalDepositSlipt").blur(function () {
    var bankTotal = $("#BankTotalDepositSlipt").val();
    $("#BankTotalDepositSlipt").val("$ " + NumberFormatDecimal(bankTotal, "2", ".", ","));
});

$("#AmountBallotDepositSlipt").blur(function () {
    var amountBallotDeposit = $("#AmountBallotDepositSlipt").val();
    $("#AmountBallotDepositSlipt").val("$ " + NumberFormatDecimal(amountBallotDeposit, "2", ".", ","));
});

$("#CommissionDepositSlipt").blur(function () {
    var commissionDeposit = $("#CommissionDepositSlipt").val();
    $("#CommissionDepositSlipt").val("$ " + NumberFormatDecimal(commissionDeposit, "2", ".", ","));
});

$("#TotalBallotDepositSlipt").blur(function () {
    var totalBallotDeposit = $("#TotalBallotDepositSlipt").val();
    $("#TotalBallotDepositSlipt").val("$ " + NumberFormatDecimal(totalBallotDeposit, "2", ".", ","));
});

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#BranchDepositSlipt").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchDepositSlipt").removeAttr("disabled");
}


$("#AceptDepositSlipt").click(function () {
    var percentage;
    var totReceivedMin;
    var totReceivedMax;

    $("#formSaveDepositSlipt").validate();

    if ($("#formSaveDepositSlipt").valid()) {
        if ($("#BankBallotNumberDepositSlipt").val() != "" && $("#BankDateDepositSlipt").val() != "" && $("#BankTotalDepositSlipt").val() != "") {

            $("#alertBanBallot").UifAlert('hide');

            if (gridValidation() == true) {

                percentage = parseFloat(percentageMinQuota) / 100;
                totReceivedMin = parseFloat(ClearFormatCurrency($("#TotalBallotDepositSlipt").val()).replace(",", ".")) - (parseFloat(ClearFormatCurrency($("#TotalBallotDepositSlipt").val()).replace(",", ".")) * percentage);
                totReceivedMax = parseFloat(ClearFormatCurrency($("#TotalBallotDepositSlipt").val()).replace(",", ".")) + (parseFloat(ClearFormatCurrency($("#TotalBallotDepositSlipt").val()).replace(",", ".")) * percentage);

                if ((parseFloat(ClearFormatCurrency($("#BankTotalDepositSlipt").val()).replace(",", ".")) >= totReceivedMin) && (parseFloat(ClearFormatCurrency($("#BankTotalDepositSlipt").val()).replace(",", ".")) <= totReceivedMax)) {
                    
                    lockScreen();

                    setTimeout(function () {    

                        $.ajax({
                        type: "POST",
                        url: ACC_ROOT + "Transaction/SavePaymentBallotRequest",
                        data: JSON.stringify({ "frmPaymentBallot": setDataBallot(), "userId": userId, "typeId": 2 }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {

                            unlockScreen();

                            //EXCEPCION ROLLBACK                            
                            if (data.success == false) {

                                $("#alertBanBallot").UifAlert('show', data.result, "danger");                               
                                        
                            } else {
                                $("#BallotId").val(("0000000" + data[0].Id));
                                $("#TechnicalTransaction").val(("0000000" + data[0].TechnicalTransaction));
                                $("#BallotNumber").val(data[0].BallotId);
                                $("#totAmountBallot").val("$" + data[0].Total);
                                $("#BillingDateBallot").val(data[0].Date);
                                $("#user").val(data[0].User);

                                var totAmountBallot = $("#totAmountBallot").text();
                                $("#totAmountBallot").text("$ " + NumberFormatDecimal(totAmountBallot, "2", ".", ","));

                                if (data[0].ShowMessage == "False") {
                                    $("#accountingMessageDiv").hide();
                                } else {
                                    $("#accountingMessageDiv").show();
                                }
                                $("#accountingMessage").val(data[0].Message);
                                
                                $('#DepositSlipSaveSuccessModal').UifModal('showLocal', Resources.DepositSlip + " " +  Resources.SuccessfullySaved);
                            }
                            clearObject();
                            cleanSearchInformation();
                            cleanBallotInformation();
                        }
                        });//fin ajax

                    }, 1200);
                }
                else {
                    if ((parseFloat(ClearFormatCurrency($("#BankTotalDepositSlipt").val()).replace(",", ".")) < totReceivedMin)) {
                        $("#alertBanBallot").UifAlert('show', Resources.BankAndBallotAmountDiffer, "danger");
                    }
                    if ((parseFloat(ClearFormatCurrency($("#BankTotalDepositSlipt").val()).replace(",", ".")) > totReceivedMax)) {
                        $("#alertBanBallot").UifAlert('show', Resources.BankAndBallotAmountDiffer, "danger");
                    }
                }
            } else {
                $("#alertBanBallot").UifAlert('show', Resources.SaveMessageBallot, "danger");
            }
        }
        else {
            $("#alertBanBallot").UifAlert('show', Resources.RequiredFieldsMissing, "warning");
        }
    }
});


$('body').delegate('#InternalBallotDepositSlipt tbody tr', "click", function () {
    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    }
    else {
        $(this).siblings('.selected').removeClass('selected');
        $(this).addClass('selected');
    }
    totalBallotCardDeposit();
});

$('#InternalBallotDepositSlipt').on('selectAll', function (event) {
    totalBallotCardDeposit();
});

$('#BranchDepositSlipt').on('itemSelected', function (event, selectedItem) {

    if ($('#BranchDepositSlipt').val() != "") {

        var controller = ACC_ROOT + "CheckControl/GetBankBranchsByBranchId?branchId=" + $("#BranchDepositSlipt").val();
        $("#ReceivingBankDepositSlipt").UifSelect({ source: controller });
        $("#AccountNumberDepositSlipt").UifSelect();
    }
});

$('#ReceivingBankDepositSlipt').on('itemSelected', function (event, selectedItem) {
    //hay bancos con código 0
    if ($('#ReceivingBankDepositSlipt').val() != "") {

        var controller = ACC_ROOT + "CheckControl/GetAccountByBankIdByBranchId?bankId=" + $('#ReceivingBankDepositSlipt').val() + '&branchId=' + $("#BranchDepositSlipt").val();        
        $("#AccountNumberDepositSlipt").UifSelect({ source: controller });
    }
});

$('#AccountNumberDepositSlipt').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        $.ajax({
            url: ACC_ROOT + "CheckInternalDepositSlip/GetAccountCurrencyByBankId",
            data: { "bankId": $("#ReceivingBankDepositSlipt").val(), "accountNumber": $("#AccountNumberDepositSlipt option:selected").text() },
            success: function (data) {
                $("#AccountCurrencyDepositSlipt").val(data.Currency.Description);
                currencyId = data.Currency.Id;
            }
        });
    }
    else {
        $("#AccountCurrencyDepositSlipt").val('');
    }
});

$("#SearchDepositSlipt").click(function () {
    $("#formCardDepositSlip").validate();

    if ($("#formCardDepositSlip").valid()) {

        $("#InternalBallotDepositSlipt").UifDataTable();
        IssuingBankId = $("#ReceivingBankDepositSlipt").val();
        ReceivingBankVariable = IssuingBankId;

        var controller = ACC_ROOT + "Transaction/GetCreditCardDepositBallots?creditCardTypeCode=" + $('#CreditCardTypeDepositSlipt').val()
                            + "&paymentTicketCode=" + $("#InternalBallotNumber").val()
                            + "&bankCode=" + $("#ReceivingBankDepositSlipt").val()
                            + "&accountNumberId=" + $("#AccountNumberDepositSlipt").val()
                            + "&accountNumber=" + $("#AccountNumberDepositSlipt option:selected").html()
                            + "&branch=" + $("#BranchDepositSlipt").val();
        $("#InternalBallotDepositSlipt").UifDataTable({ source: controller });

        cleanBallotInformation();
    }
    setTimeout(function () {
        $("#alertDepositList").UifAlert('hide');
    }, 3000);
});

$("#CleanDepositSlipt").click(function () {
    cleanBallotInformation();
    cleanSearchInformation();
    clearObject();
});


$("#CancelDepositSlipt").click(function () {
    cleanBallotInformation();
    cleanSearchInformation();
    clearObject();
});

$("#BankDateDepositSlipt").blur(function () {
    if ($("#BankDateDepositSlipt").val() != '') {
        if (IsDate($("#BankDateDepositSlipt").val()) == true) {
            if (CompareDates($("#BankDateDepositSlipt").val(), getCurrentDate()) == 2)
                $("#BankDateDepositSlipt").val(getCurrentDate());
        }

        else {
            $("#BankDateDepositSlipt").val(getCurrentDate());
        }
    }
});

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
function setDataBallot() {
    oPaymentBallotModel.PaymentBallotId = 0;
    oPaymentBallotModel.PaymentBallotNumber = $("#BankBallotNumberDepositSlipt").val();
    oPaymentBallotModel.PaymentAccountNumber = $("#AccountNumberDepositSlipt option:selected").text();
    oPaymentBallotModel.PaymentBallotBankId = $("#ReceivingBankDepositSlipt").val();
    oPaymentBallotModel.PaymentBallotAmount = parseFloat(((ClearFormatCurrency($("#TotalBallotDepositSlipt").val()).replace(",", ".")).trim()));
    oPaymentBallotModel.PaymentBallotBankAmount = parseFloat(((ClearFormatCurrency($("#BankTotalDepositSlipt").val()).replace(",", ".")).trim()));
    oPaymentBallotModel.PaymentCurrency = currencyId; //-1;
    oPaymentBallotModel.PaymentBankDate = $("#BankDateDepositSlipt").val();
    oPaymentBallotModel.PaymentStatus = 1;

    var ticketToBallotRow;
    var i = 0;
    var ids = 0;
    var ticketToBallotRow = $("#InternalBallotDepositSlipt").UifDataTable("getSelected");

    for (var i in ticketToBallotRow) {

        oPaymentTicketBallotModel = {
            PaymentTicketBallotId: 0
        };

        oPaymentTicketBallotModel.PaymentTicketBallotId = ticketToBallotRow[i].PaymentTicketCode;
        oPaymentBallotModel.PaymentTicketBallotModels.push(oPaymentTicketBallotModel);
    }

    return oPaymentBallotModel;
}

function gridValidation() {
    var result = false;

    var ids = $("#InternalBallotDepositSlipt").UifDataTable("getSelected");

    if (ids != null)
        result = true;

    return result;
}

function totalBallotCardDeposit() {
    var ballotAmount = 0;
    var depositBallotAmount;
    var taxAmount;
    var selRowIds = $("#InternalBallotDepositSlipt").UifDataTable("getSelected");
    var commission = 0;
    var total = 0;


    for (var i in selRowIds) {
        depositBallotAmount = $.trim(ClearFormatCurrency(selRowIds[i].AmountDeposit));
        taxAmount = $.trim(ClearFormatCurrency(selRowIds[i].Commission));

        ballotAmount += parseFloat(depositBallotAmount);
        commission += parseFloat(taxAmount);

        total = (commission + ballotAmount);
        i++;
    }

    $("#CommissionDepositSlipt").val(commission);
    $("#CommissionDepositSlipt").blur();
    $("#AmountBallotDepositSlipt").val(ballotAmount);
    $("#AmountBallotDepositSlipt").blur();
    $("#TotalBallotDepositSlipt").val(total);
    $("#TotalBallotDepositSlipt").blur();
}

function cleanSearchInformation() {
    $("#CreditCardTypeDepositSlipt").val(-1);
    $("#ReceivingBankDepositSlipt").val(-1);
    IssuingBankId = -1;
    $("#ReceivingBankVariable").val(-1);
    $("#AccountCurrencyDepositSlipt").val('');
    $("#AccountCurrencyDepositSliptCd").val(-1);
    $("#InternalBallotNumber").val("");
    $("#BranchDepositSlipt").val(-1);
    $("#InternalBallotDepositSlipt").dataTable().fnClearTable();

    $("#AccountNumberDepositSlipt").UifSelect();
}

function cleanBallotInformation() {
    $("#BankBallotNumberDepositSlipt").val('');
    $("#BankDateDepositSlipt").val('');
    $("#BankTotalDepositSlipt").val('');
    $("#AmountBallotDepositSlipt").val('');
    $("#CommissionDepositSlipt").val('');
    $("#TotalBallotDepositSlipt").val('');
}

function clearObject() {
    oPaymentBallotModel = {
        PaymentBallotId: 0,
        PaymentBallotNumber: null,
        PaymentAccountNumber: null,
        PaymentBallotBankId: 0,
        PaymentBallotAmount: 0,
        PaymentBallotBankAmount: 0,
        PaymentCurrency: null,
        PaymentStatus: 0,
        PaymentBankDate: null,

        PaymentTicketBallotModels: []
    };

    oPaymentTicketBallotModel = {
        PaymentTicketBallotId: 0
    };
}

