/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//  DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

//Variables Globales

var bankId = -1;
var startDate = "";
var endDate = "";
var paymentTicketId = -1;
var creditCardTypeId = -1;
var branchId = -1;
var issuingBankCodeAut = -1;
var creditableCreditCardSearch = -1;
var ncreditableCreditCardSearch = 0;
var errorNumber = 0;
var Alert = 0;
var deletedCardSearch = [];

var idsUpdateCardDeposit = [];

var oTblChecks = {
    PaymentTicket: []
};

var oPaymentTicket = {
    Id: null,
    Branch: null,
    BranchName: null,
    Bank: null,
    BankDescription: null,
    AccountNumber: null,
    CashAmount: null,
    Checks: null,
    DatePayment: null,
    PaymentMethodId: null,
    Currency: null,
    Amount: null,
    CommissionAmount: null,
    PaymentId: null,
    Select: null,
    PaymentTicketId: null,
    DeleteRecords: null,
    PaymentTicketItemId: null,
    VoucherNumber: null
};


function cleanObjectCardSearch() {
    oPaymentTicket = {
        Id: null,
        Branch: null,
        BranchName: null,
        Bank: null,
        BankDescription: null,
        AccountNumber: null,
        CashAmount: null,
        Checks: null,
        DatePayment: null,
        PaymentMethodId: null,
        Currency: null,
        Amount: null,
        CommissionAmount: null,
        PaymentId: null,
        Select: null,
        PaymentTicketId: null,
        PaymentTicketItemId: null
    };
    var ids = 0;
}

function setCardDataFieldsAEmpty() {
    $("#alertCardVoucher").UifAlert('hide');
    $("#SearchCardsCardDepositSlipsSearch").dataTable().fnClearTable();
    $("#StartDateCardDepositSlipsSearch").val("");
    $("#EndDateCardDepositSlipsSearch").val("");
    $("#InternalBallotNumberCardDepositSlipsSearch").val("");
    $("#BankCardDepositSlipsSearch").val("");
    $("#CreditCardTypeCardDepositSlipsSearch").val("");
    $("#BranchCardDepositSlipsSearch").val("");
    bankId = -1;
    startDate = "";
    endDate = "";
    paymentTicketId = -1;
    creditCardTypeId = -1;
    branchId = -1;
    $("#IssuingBankCardInternalDeposit").val("");
    $("#VoucherNumberCardInternalDeposit").val("");
}

function setTicketCardDepositSearch(optionSaveUpdate) {

    $("#alertCardVoucher").UifAlert('hide');
    var issuingBankCode = $("#ReceivingBankCode").val();
    var issuingBankName = $("#ReceivingBankNameCardInternalDeposit").val();
    var accountNumber = $("#ReceivingAccountNumberCardInternalDeposit").val();
    var cardAmount = $("#CardAmountCardInternalDeposit").val();
    var currencyId = $("#AccountCurrencyCode").val();
    var paymentMethodTypeCode = $("#PaymentMethodTypeCode").val();
    var paymentTicketId = $("#BallotNumberCardInternalDeposit").val();

    if (paymentMethodTypeCode == ncreditableCreditCardSearch) {
        issuingBankCode = "";
        issuingBankName = "";
        accountNumber = "";
        currencyId = "-1";
    }
    //var creditCardTypeCode = $("#CreditCardTypeCode").val();
    var branchCode = $("#BranchCode").val();
    var branchName = $("#BranchNameCardInternalDeposit").val();

    var dataCardBallotInternal = $("#ACardsDialogCardInternalDeposit").UifDataTable("getSelected");

    if (cardAmount == "" && dataCardBallotInternal != null) {
        cardAmount = "0";
        for (var i in dataCardBallotInternal) {
            cleanObjectCardSearch();
            oPaymentTicket.Branch = branchCode;
            oPaymentTicket.BranchName = branchName;
            oPaymentTicket.Bank = issuingBankCode;
            oPaymentTicket.BankDescription = issuingBankName;
            oPaymentTicket.Checks = dataCardBallotInternal[i].CardNumber;
            oPaymentTicket.DatePayment = dataCardBallotInternal[i].CardDate;
            oPaymentTicket.PaymentMethodId = paymentMethodTypeCode;
            oPaymentTicket.Currency = dataCardBallotInternal[i].CurrencyCode;
            oPaymentTicket.Amount = parseFloat(ClearFormatCurrency(dataCardBallotInternal[i].Amount.toString().replace("", ",")));
            oPaymentTicket.CommissionAmount = parseFloat(ClearFormatCurrency(dataCardBallotInternal[i].CommissionAmount.toString().replace("", ",")));
            oPaymentTicket.PaymentId = dataCardBallotInternal[i].PaymentCode;
            oPaymentTicket.AccountNumber = accountNumber;
            oPaymentTicket.CashAmount = parseFloat(cardAmount);
            oPaymentTicket.PaymentTicketId = paymentTicketId;
            oPaymentTicket.PaymentTicketItemId = dataCardBallotInternal[i].PaymentTicketItemCode;
            oPaymentTicket.VoucherNumber = dataCardBallotInternal[i].VoucherNumber;

            oTblChecks.PaymentTicket.push(oPaymentTicket);
        };
    }
    else if (cardAmount != "" && dataCardBallotInternal != null) {
        cardAmount = ClearFormatCurrency(cardAmount.replace("", ","));

        for (var i in dataCardBallotInternal) {
            cleanObjectCardSearch();
            oPaymentTicket.Branch = branchCode;
            oPaymentTicket.BranchName = branchName;
            oPaymentTicket.Bank = issuingBankCode;
            oPaymentTicket.BankDescription = issuingBankName;
            oPaymentTicket.Checks = dataCardBallotInternal[i].CardNumber;
            oPaymentTicket.DatePayment = dataCardBallotInternal[i].CardDate;
            oPaymentTicket.PaymentMethodId = paymentMethodTypeCode;
            oPaymentTicket.Currency = dataCardBallotInternal[i].CurrencyCode;
            oPaymentTicket.Amount = parseFloat(ClearFormatCurrency(dataCardBallotInternal[i].Amount.toString().replace("", ",")));
            oPaymentTicket.CommissionAmount = parseFloat(ClearFormatCurrency(dataCardBallotInternal[i].CommissionAmount.toString().replace("", ",")));
            oPaymentTicket.PaymentId = dataCardBallotInternal[i].PaymentCode;
            oPaymentTicket.AccountNumber = accountNumber;
            oPaymentTicket.CashAmount = 0;
            oPaymentTicket.PaymentTicketId = paymentTicketId;
            oPaymentTicket.PaymentTicketItemId = dataCardBallotInternal[i].PaymentTicketItemCode;
            oPaymentTicket.VoucherNumber = dataCardBallotInternal[i].VoucherNumber;

            oTblChecks.PaymentTicket.push(oPaymentTicket);
        };
    }
    /*
    else if (cardAmount != "" && dataCardBallotInternal != null) {
        cardAmount = ClearFormatCurrency(cardAmount.toString().replace("", ","));
        paymentMethodTypeCode = Resources.ParamPaymentMethodCash;

        cleanObjectCardSearch();
        oPaymentTicket.Branch = branchCode;
        oPaymentTicket.BranchName = branchName;
        oPaymentTicket.Bank = issuingBankCode;
        oPaymentTicket.BankDescription = issuingBankName;
        oPaymentTicket.PaymentMethodId = paymentMethodTypeCode;
        oPaymentTicket.Currency = currencyId;
        oPaymentTicket.AccountNumber = accountNumber;
        oPaymentTicket.CashAmount = parseFloat(cardAmount);
        oPaymentTicket.PaymentTicketId = paymentTicketId;
        oPaymentTicket.PaymentTicketItemId = rowid.PaymentTicketItemCode;
        oTblChecks.PaymentTicket.push(oPaymentTicket);
    }//else
    */
    return oTblChecks;
};

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#BranchCardDepositSlipsSearch").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchCardDepositSlipsSearch").removeAttr("disabled");
}

$("#CleanCardDepositSlipsSearch").click(function () {
    setDataFieldsEmptyCardSearch();
});


$("#SearchCardDepositSlipsSearch").click(function () {
    $("#alertCardVoucher").UifAlert('hide');
    startDate = $("#StartDateCardDepositSlipsSearch").val();
    endDate = $("#EndDateCardDepositSlipsSearch").val();

    if ($("#CreditCardTypeCardDepositSlipsSearch").val() != "") {
        creditCardTypeId = $("#CreditCardTypeCardDepositSlipsSearch").val();
    }
    else {
        creditCardTypeId = -1;
    }

    if ($("#BranchCardDepositSlipsSearch").val() != "") {
        branchId = $("#BranchCardDepositSlipsSearch").val();
    }
    else {
        branchId = -1;
    }

    if ($("#InternalBallotNumberCardDepositSlipsSearch").val() == "") {
        paymentTicketId = -1;
    }
    else {
        paymentTicketId = $("#InternalBallotNumberCardDepositSlipsSearch").val();
    }

    if ($("#BankCardDepositSlipsSearch").val() == "") {
        bankId = -1;
    }
    else {
        bankId = issuingBankCodeAut;
    }

    $("#CancelCardsCardDepositSlipsSearch").hide();
    $("#EditCardsCardDepositSlipsSearch").hide();

    if ((bankId != -1 && $("#BankCardDepositSlipsSearch").val() != "") || startDate != "" || endDate != "" || paymentTicketId != -1 || creditCardTypeId != -1 || branchId != -1) {

        var controller = ACC_ROOT + "CardDepositSlipSearch/SearchInternalBallotCard?bankId=" + bankId
                         + "&startDate=" + startDate + "&endDate="
                         + endDate + "&paymentTicketId=" + paymentTicketId
                         + "&creditCardTypeId=" + creditCardTypeId + "&branchId=" + branchId;

        $("#SearchCardsCardDepositSlipsSearch").UifDataTable({ source: controller });
    }
    else {
        $("#alertCardVoucher").UifAlert('show', Resources.SearchBallotsSearchCriteriaMessage, "danger");
    }
});


$("#CancelCardsCardDepositSlipsSearch").hide();

//BOTÓN ANULAR
$("#CancelCardsCardDepositSlipsSearch").click(function () {
    $("#alertCardVoucher").UifAlert('hide');
    showConfirmCardSearch();
});

$('#AcceptModalCardDepositSlipsSearch').click(function () {
    $("#alertCardVoucher").UifAlert('hide');
    $('#ModalMessage1').UifModal('hide');
    refreshGridCardSearch();
});


//Controla que la fecha final sea mayor a la inicial
$('#StartDateCardDepositSlipsSearch').on('datepicker.change', function (event, date) {
    $("#alertCardVoucher").UifAlert('hide');
    if ($("#EndDateCardDepositSlipsSearch").val() != "") {

        if (compare_dates($('#StartDateCardDepositSlipsSearch').val(), $("#EndDateCardDepositSlipsSearch").val())) {

            $("#alertCardVoucher").UifAlert('show', Resources.ValidateDateTo, "warning");

            $("#StartDateCardDepositSlipsSearch").val('');
        } else {
            $("#StartDateCardDepositSlipsSearch").val($('#StartDateCardDepositSlipsSearch').val());
        }
    }
});

$("#StartDateCardDepositSlipsSearch").blur(function () {
    $("#alertCardVoucher").UifAlert('hide');

    if ($("#StartDateCardDepositSlipsSearch").val() != '') {

        if (IsDate($("#StartDateCardDepositSlipsSearch").val()) == true) {
            if ($("#EndDateCardDepositSlipsSearch").val() != '') {
                if (CompareDates($("#StartDateCardDepositSlipsSearch").val(), $("#EndDateCardDepositSlipsSearch").val())) {
                    $("#alertCardVoucher").UifAlert('show', Resources.ValidateDateTo, "warning");
                    $("#StartDateCardDepositSlipsSearch").val("");
                    return true;
                }
            }
        } else {
            $("#alertCardVoucher").UifAlert('show', Resources.EntryDateProcessFrom, "warning");
            $("#StartDateCardDepositSlipsSearch").val("");
        }
    }
});

//Controla que la fecha final sea mayor a la inicial
$('#EndDateCardDepositSlipsSearch').on('datepicker.change', function (event, date) {
    $("#alertCardVoucher").UifAlert('hide');
    if ($("#StartDateCardDepositSlipsSearch").val() != "") {
        if (compare_dates($("#StartDateCardDepositSlipsSearch").val(), $('#EndDateCardDepositSlipsSearch').val())) {

            $("#alertCardVoucher").UifAlert('show', Resources.ValidateDateFrom, "warning");

            $("#EndDateCardDepositSlipsSearch").val('');
        } else {
            $("#EndDateCardDepositSlipsSearch").val($('#EndDateCardDepositSlipsSearch').val());
        }
    }
});

$("#EndDateCardDepositSlipsSearch").blur(function () {
    $("#alertCardVoucher").UifAlert('hide');

    if ($("#EndDateCardDepositSlipsSearch").val() != '') {

        if (IsDate($("#EndDateCardDepositSlipsSearch").val()) == true) {
            if ($("#StartDateCardDepositSlipsSearch").val() != '') {
                if (CompareDates($("#StartDateCardDepositSlipsSearch").val(), $("#EndDateCardDepositSlipsSearch").val())) {
                    $("#alertCardVoucher").UifAlert('show', Resources.ValidateDateFrom, "warning");
                    $("#EndDateCardDepositSlipsSearch").val("");
                    return true;
                }
            }
        } else {
            $("#alertCardVoucher").UifAlert('show', Resources.EntryDateProcessTo, "warning");
            $("#EndDateCardDepositSlipsSearch").val("");
        }
    }
});


//Valida para la visualización de botones
$('#SearchCardsCardDepositSlipsSearch').on('rowSelected', function (event, selectedRow) {
    $("#alertCardVoucher").UifAlert('hide');
    if (selectedRow != null) {
        var status = selectedRow.Status;
        if (status == 1) {
            $("#CancelCardsCardDepositSlipsSearch").show();
            $("#EditCardsDepositSlipsSearch").show();
        }
        else if (status == 0) {
            $("#CancelCardsCardDepositSlipsSearch").hide();
            $("#EditCardsDepositSlipsSearch").hide();
        }
        else if (status == 3) {
            $("#CancelCardsCardDepositSlipsSearch").hide();
            $("#EditCardsDepositSlipsSearch").hide();
        }
    }
});

$("#EditCardsDepositSlipsSearch").hide();

//BOTÓN EDITAR
$("#EditCardsDepositSlipsSearch").click(function () {
    $("#alertCardVoucher").UifAlert('hide');
    var dataCardBallotInternal = $("#SearchCardsCardDepositSlipsSearch").UifDataTable("getSelected");
    var paymentBallotNumber;
    var paymentTicketCode;
    var paymentMethodTypeCode;
    oTblChecks = {
        PaymentTicket: []
    };

    for (var i in dataCardBallotInternal) {
        if (dataCardBallotInternal != null) {
            paymentBallotNumber = dataCardBallotInternal[i].PaymentBallotNumber;
            paymentTicketCode = dataCardBallotInternal[i].PaymentTicketCode;
            paymentMethodTypeCode = dataCardBallotInternal[i].PaymentMethodTypeCode;

            if (paymentBallotNumber == "") {

                $("#BallotNumberCardInternalDeposit").val(paymentTicketCode);
                $("#PaymentMethodTypeCode").val(paymentMethodTypeCode);
                loadInternalBallotCard();

                $('#modalView').UifModal('showLocal', Resources.InternalBallotCardDialogTitle + ' ' + paymentTicketCode);
            }
            else {

                $("#alertCardInternalDeposit").UifAlert('show', Resources.InternalBallotCardDialogMessageEdit, "danger");
            }
        }
    }

    //Encera los arreglos de log/update/delete
    idsUpdateCardDeposit = [];

    setCardDataFieldsEmpty();
    cleanObjectCardSearch();
});


$('#BankCardDepositSlipsSearch').on('itemSelected', function (event, selectedItem) {
    issuingBankCodeAut = 0;
    issuingBankCodeAut = selectedItem.bankId;
});

$("#btnCancelTicket").click(function () {
    $("#alertCardInternalDeposit").UifAlert('hide');

    setCardDataFieldsAEmpty();
    cleanObjectCardSearch();
});

$('#SearchCardsCardDepositSlipsSearch').on('rowEdit', function (event, data, position) {
    $("#alertCardInternalDeposit").UifAlert('hide');

    paymentId = data.PaymentTicketCode;
    var control = "GetDetailCards?paymentTicketId=" + paymentId;
    $("#DetailCardsCardDepositSlipsSearch").UifDataTable({ source: control });
    $('#modalViewDetail').appendTo("body").UifModal('showLocal', Resources.InternalBallotNumber + ': ' + paymentId);
});


//Actualiza el monto total cuando se elimina un ítem
function AddCardsSelected() {

    $("#alertCardInternalDeposit").UifAlert('hide');

    var ids = $("#ACardsDialogCardInternalDeposit").UifDataTable("getData");
    var totalChecks = 0;
    if (ids.length > 0) {
        for (var i in ids) {
            var rowid = ids[i];
            var valueChecks = parseFloat(ClearFormatCurrency(rowid.IncomeAmount.toString().replace("", ",")));
            totalChecks += valueChecks;
        }//for
        $("#CardAmountCardInternalDeposit").val(totalChecks);
        $("#CardAmountCardInternalDeposit").val(FormatCurrency(FormatDecimal(totalChecks)));
    }
}


function showConfirmCardSearch() {
    $("#alertCardInternalDeposit").UifAlert('hide');
    $("#alertCardVoucher").UifAlert('hide');
    $.UifDialog('confirm', { 'message': 'Está seguro que desea anular la boleta interna?', 'title': 'Boleta Interna' }, function (result) {
        if (result) {
            var dataCardBallotInternal = $("#SearchCardsCardDepositSlipsSearch").UifDataTable("getSelected");

            if (dataCardBallotInternal == null) {
                $("#alertCardVoucher").UifAlert('show', Resources.SelectARecord, "danger");
            }
            if (dataCardBallotInternal != null) {
                cancelBallotCardSearch();
            }
        }
    });
};


function refreshGridCardSearch() {
    $("#alertCardInternalDeposit").UifAlert('hide');
    $("#alertCardVoucher").UifAlert('hide');
    var paymentMethodTypeCode = -1;
    var issuingBankCode = "";
    var voucherNumber = "";
    var currencyCode = "";
    var creditCardType = "";
    var branchCode = "";
    var cardDate = "";

    paymentMethodTypeCode = $("#PaymentMethodTypeCode").val();
    issuingBankCode = issuingBankCodeAut;
    voucherNumber = $("#VoucherNumberCardInternalDeposit").val();
    currencyCode = $("#AccountCurrencyCode").val();
    creditCardType = $("#CreditCardTypeCode").val();
    branchCode = $("#BranchCode").val();
    cardDate = $("#ToDateCardInternalDeposit").val();

    var control = ACC_ROOT + "CardInternalDepositSlip/GetCardsToDepositBallot?paymentMethodTypeCode=" + paymentMethodTypeCode
                     + "&creditCardType=" + creditCardType + "&branchCode=" + branchCode + "&cardDate=" + cardDate
                     + "&currencyCode=" + currencyCode + "&issuingBankCode=" + issuingBankCode + "&voucherNumber=" + voucherNumber;

    $("#CardsDialogCardInternalDeposit").UifDataTable({ source: control });
}


//Agrega tarjetas a depositar a tarjetas seleccionadas
function addCard() {
    $("#alertCardInternalDeposit").UifAlert('hide');
    $("#alertCardVoucher").UifAlert('hide');

    var dataBallotsDeposit = $("#CardsDialogCardInternalDeposit").UifDataTable("getSelected");

    if (dataBallotsDeposit != null) {
        for (var i in dataBallotsDeposit) {
            if (DuplicateRecordsCardDeposit(dataBallotsDeposit[i].PaymentCode) == false) {
                $('#ACardsDialogCardInternalDeposit').UifDataTable('addRow', dataBallotsDeposit[i]);

                var valueCards = parseFloat(ClearFormatCurrency(dataBallotsDeposit[i].Amount.replace("", ",")));
                if ($("#CardAmountCardInternalDeposit").val() == "")
                    totalCards = 0;
                else
                    totalCards = parseFloat(ClearFormatCurrency($("#CardAmountCardInternalDeposit").val().replace("", ",")));

                totalCards += valueCards;

                $("#CardAmountCardInternalDeposit").val(totalCards);
                $("#CardAmountCardInternalDeposit").val(FormatCurrency(FormatDecimal(totalCards)));

                var valueCommission = parseFloat(ClearFormatCurrency(dataBallotsDeposit[i].CommissionAmount.replace("", ",")));
                if ($("#CommissionAmountCardInternalDeposit").val() == "")
                    totalCommission = 0;
                else
                    totalCommission = parseFloat(ClearFormatCurrency($("#CommissionAmountCardInternalDeposit").val().replace("", ",")));

                totalCommission += valueCommission;

                $("#CommissionAmountCardInternalDeposit").val(totalCommission);
                $("#CommissionAmountCardInternalDeposit").val(FormatCurrency(FormatDecimal(totalCommission)));
            }
            else {
                $("#alertCardInternalDeposit").UifAlert('show', Resources.MInternalBallotDuplicateRecord, "danger");
            }
        }
    }
}

function cancelBallotCardSearch() {

    $("#alertCardInternalDeposit").UifAlert('hide');
    $("#alertCardVoucher").UifAlert('hide');
    var dataCardBallotInternal = $("#SearchCardsCardDepositSlipsSearch").UifDataTable("getSelected");

    for (var i = 0; i < dataCardBallotInternal.length; i++) {
        var paymentTicketCode = dataCardBallotInternal[i].PaymentTicketCode;
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "DepositSlipSearch/ValidateExternalBallotDeposited",
            data: JSON.stringify({ "paymentTicketCode": paymentTicketCode }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data[0].resp === true) {
                    $('#modalMessage').appendTo("body").modal('show');
                    return true;
                }
                else if (data[0].resp == false) {
                    $.ajax({
                        type: "POST",
                        url: ACC_ROOT + "CardDepositSlipSearch/CancelInternalBallot",
                        async: false,
                        data: { "paymetTicketCode": paymentTicketCode },
                        success: function (data) {
                            startDate = $("#StartDateCardDepositSlipsSearch").val();
                            endDate = $("#EndDateCardDepositSlipsSearch").val();
                            if ($("#InternalBallotNumberCardDepositSlipsSearch").val() == "") {
                                paymentTicketId = -1;
                            }
                            $("#CancelCardsCardDepositSlipsSearch").hide();
                            $("#EditCardsCardDepositSlipsSearch").hide();


                            $.UifDialog('confirm', { 'message': Resources.CancelBallotSuccess }, function (result) {
                                if (result) {
                                    setCardDataFieldsAEmpty();
                                }
                            });
                        }
                    });
                }
            }
        });
    }
}


//Validar que no se agregue duplicados en tarjetas seleccionadas
function DuplicateRecordsCardDeposit(paymentCode) {
    $("#alertCardInternalDeposit").UifAlert('hide');
    $("#alertCardVoucher").UifAlert('hide');

    var result = false;
    var dataSelectedBallots = $("#ACardsDialogCardInternalDeposit").UifDataTable("getData");
    if (dataSelectedBallots.length > 0) {
        for (var f in dataSelectedBallots) {
            if (paymentCode == dataSelectedBallots[f].PaymentCode) {
                result = true;
                break;
            }
            else {
                result = false;
            }
        }
    }

    return result;
}


function setDataFieldsEmptyCardSearch() {
    $("#alertCardInternalDeposit").UifAlert('hide');
    $("#alertCardVoucher").UifAlert('hide');
    $("#SearchCardsCardDepositSlipsSearch").dataTable().fnClearTable();
    $("#StartDateCardDepositSlipsSearch").val("");
    $("#EndDateCardDepositSlipsSearch").val("");
    $("#InternalBallotNumberCardDepositSlipsSearch").val("");
    $("#BankCardDepositSlipsSearch").val("");
    $("#CreditCardTypeCardDepositSlipsSearch").val("");
    $("#BranchCardDepositSlipsSearch").val("");
    bankId = -1;
    startDate = "";
    endDate = "";
    paymentTicketId = -1;
    creditCardTypeId = -1;
    branchId = -1;
}


function setCardDataFieldsEmpty() {
    $("#alertCardInternalDeposit").UifAlert('hide');
    $("#alertCardVoucher").UifAlert('hide');
    $("#IssuingBankCardInternalDeposit").val("");
    $("#VoucherNumberCardInternalDeposit").val("");
    $("#CardsDialogCardInternalDeposit").dataTable().fnClearTable();
    issuingBankCodeAut = -1;
}

function ValidateFields() {
    $("#alertCardInternalDeposit").UifAlert('hide');
    $("#alertCardVoucher").UifAlert('hide');
    var result = false;


    if ($("#PaymentMethodTypeCode").val() == "4" || $("#PaymentMethodTypeCode").val() == "-1") {
        if ($("#ReceivingAccountNumberCardInternalDeposit").val() == "") {
            result = false;
        }
        else {
            var dataCardBallotInternal = $("#ACardsDialogCardInternalDeposit").UifDataTable("getSelected");
            if (dataCardBallotInternal != null) {
                result = true;
            }
            else {
                result = false;
            }
        }
    }


    if ($("#PaymentMethodTypeCode").val() == "5" || $("#PaymentMethodTypeCode").val() == "11") {
        if ($("#ReceivingAccountNumberCardInternalDeposit").val() == "") {
            result = false;
        }
        else {
            var dataCardBallotInternal = $("#ACardsDialogCardInternalDeposit").UifDataTable("getSelected");
            if (dataCardBallotInternal != null) {
                result = true;
            }
            else {
                result = false;
            }
        }
    }

    return result;
}

function refreshGridSel() {
    $("#alertCardInternalDeposit").UifAlert('hide');
    $("#alertCardVoucher").UifAlert('hide');
    var paymentTicketCode = $("#BallotNumberCardInternalDeposit").val();
    var paymentMethodTypeCode = $("#PaymentMethodTypeCode").val();

    var controller = ACC_ROOT + "CardInternalDepositSlip/GetDetailInternalBallotCard?paymentTicketCode=" + paymentTicketCode
                              + "&paymentMethodTypeCode=" + paymentMethodTypeCode;

    $("#ACardsDialogCardInternalDeposit").UifDataTable({ source: controller });

    setTimeout(function () {
        updateRecordsCardDeposit();
    }, 1500);
}


//Carga reporte
function loadReportCardSearch(paymentTicketId, paymentMethodTypeId) {

    $("#alertCardInternalDeposit").UifAlert('hide');
    $("#alertCardVoucher").UifAlert('hide');

    var controller = ACC_ROOT + "Report/LoadInternalBallotCardReport?paymentTicketCode="
                        + paymentTicketId + "&paymentMethodTypeCode=" + paymentMethodTypeId;
    window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

//Obtiene la fecha del servidor
function getDateCardDeposit() {
    $("#alertCardInternalDeposit").UifAlert('hide');
    $("#alertCardVoucher").UifAlert('hide');

    if ($("#ViewBagImputationType").val() == undefined &&
        $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
        $("#ViewBagBillControlId") == undefined) {

        $.ajax({
            type: "GET",
            url: ACC_ROOT + "Billing/GetDate",
            success: function (data) {
                $("#ToDateCardInternalDeposit").val(data);
            }
        });
    }
}


function loadInternalBallotCard() {
    $("#alertCardInternalDeposit").UifAlert('hide');
    $("#alertCardVoucher").UifAlert('hide');

    $("#CardsDialogCardInternalDeposit").dataTable().fnClearTable;
    $("#ACardsDialogCardInternalDeposit").dataTable().fnClearTable;

    if ($("#BallotNumberCardInternalDeposit").val() != "") {
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "CardInternalDepositSlip/GetInternalBallotCardHeader",
            data: { "paymentTicketCode": $("#BallotNumberCardInternalDeposit").val(), "paymentMethodTypeCode": $("#PaymentMethodTypeCode").val() },
            success: function (data) {
                if (data.length > 0) {
                    $("#CreditCardTypeCode").val(data[0].CreditCardTypeCode);
                    $("#CreditCardTypeNameCardInternalDeposit").val(data[0].CreditCardTypeName);
                    $("#BranchCode").val(data[0].BranchCode);
                    $("#BranchNameCardInternalDeposit").val(data[0].BranchName);
                    if ($("#PaymentMethodTypeCode").val() == "4" || $("#PaymentMethodTypeCode").val() == "-1") {
                        $("#ReceivingBankCode").val(data[0].BankCode);
                        $("#ReceivingBankNameCardInternalDeposit").val(data[0].BankName);
                        $("#ReceivingAccountNumberCardInternalDeposit").val(data[0].AccountNumber);
                        $("#AccountCurrencyCode").val(data[0].CurrencyCode);
                        $("#AccountCurrencyNameCardInternalDeposit").val(data[0].CurrencyName);
                    }
                    else {
                        $("#ReceivingBankCode").val('');
                        $("#ReceivingBankNameCardInternalDeposit").val('');
                        $("#ReceivingAccountNumberCardInternalDeposit").val('');
                        $("#AccountCurrencyCode").val('');
                        $("#AccountCurrencyNameCardInternalDeposit").val('');
                    }

                    $("#CardAmountCardInternalDeposit").val(FormatCurrency(FormatDecimal(data[0].Amount)));
                    $("#CommissionAmountCardInternalDeposit").val(FormatCurrency(FormatDecimal(data[0].CommissionAmount)));

                    refreshGridSel();
                }
                else {
                    $("#CreditCardTypeCode").val('');
                    $("#CreditCardTypeNameCardInternalDeposit").val('');
                    $("#BranchCode").val('');
                    $("#BranchNameCardInternalDeposit").val('');
                    $("#ReceivingBankCode").val('');
                    $("#ReceivingBankNameCardInternalDeposit").val('');
                    $("#ReceivingAccountNumberCardInternalDeposit").val('');
                    $("#AccountCurrencyCode").val('');
                    $("#AccountCurrencyNameCardInternalDeposit").val('');
                    $("#CardAmountCardInternalDeposit").val('');
                    $("#CardsDialog").dataTable().fnClearTable;
                    $("#ACardsDialog").dataTable().fnClearTable;
                }
            }
        });
    }
    else {
        $("#_ReceiptDate").val('');
        $("#_BillingConcept").val('');
        $("#BillingConcept").val('');
        $("#_BillControlCode").val('');
    }
}

setTimeout(function () {
    getDateCardDeposit();
}, 500);

//Función no ha sido migrada desde Billing
function deleteRecordsCardDeposit(data) {

    if (data.PaymentTicketItemId != undefined) {
        var paymentTicketId = $("#BallotNumberCardInternalDeposit").val();
        cleanObjectCardSearch();
        oPaymentTicket.PaymentTicketId = paymentTicketId;
        oPaymentTicket.PaymentId = data.PaymentCode;
        oPaymentTicket.PaymentTicketItemId = data.PaymentTicketItemId;
        oTblChecks.PaymentTicket.push(oPaymentTicket);
    }
}

//Función no ha sido migrada desde Billing
function updateRecordsCardDeposit() {

    idsUpdateCardDeposit = [];
    var ids = $("#ACardsDialogCardInternalDeposit").UifDataTable("getData");

    if ($("#ACardsDialogCardInternalDeposit").UifDataTable('getData').length > 0) {
        for (var i in ids) {

            var rowid = ids[i];
            idsUpdateCardDeposit[i] = rowid.PaymentCode;
        }
    }
}


/**********************************************************************************************************/
//ACCIONES DE BOTONES DE MODALES

$(document).ready(function () {

    //LIMPIA
    $("#CleanCardCardInternalDeposit").click(function () {
        setCardDataFieldsEmpty();
        cleanObjectCardSearch();
    });

    //BUSCAR
    $("#SearchCardCardInternalDeposit").click(function () {
        $("#alertCardInternalDeposit").UifAlert('hide');
        $("#alertCardVoucher").UifAlert('hide');
        var errorNumber = 0;

        $("#formIBOne").validate();

        if ($("#formIBOne").valid()) {
            if ($("#ToDateCardInternalDeposit").val() == "") {
                errorNumber = 1;
            }
            if (IsDate($("#ToDateCardInternalDeposit").val()) == false) {
                errorNumber = 1;
            }

            if (errorNumber == 0) {
                Alert = 0;
            } else {
                Alert = 1;
                errorNumber = 0;
            }

            if (Alert != 0) {
                $("#alertCardInternalDeposit").UifAlert('show', Resources.RequiredFieldsMissing, "danger");
            }
            else {
                refreshGridCardSearch();
            }
        }
    });

    //AGREGA
    $('#CardsDialogCardInternalDeposit').on('rowAdd', function (event, data) {
        $("#alertCardInternalDeposit").UifAlert('hide');
        var rowidc = $("#CardsDialogCardInternalDeposit").UifDataTable("getSelected");

        if (rowidc != null) {
            addCard();
        }
        else {
            $("#alertCardInternalDeposit").UifAlert('show', Resources.InternalDepositSlipSaveMessage, "danger");
        }
    });


    //ACEPTAR
    $("#SaveTicketCardInternalDeposit").click(function () {
        $("#alertCardInternalDeposit").UifAlert('hide');
        var errorNumber = 0;

        if (ValidateFields() == true) {
            var checksModel = ticketCardDepositSearch();

            $.ajax({
                type: "POST",
                url: ACC_ROOT + "CheckInternalDepositSlip/DeleteInternalBallot",
                data: JSON.stringify({ "checksModel": checksModel }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
            }).done(function (data) {
                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "CheckInternalDepositSlip/UpdateInternalBallot",
                    data: JSON.stringify({ "checksModel": setTicketCardDepositSearch() }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                }).done(function (data) {

                    var branchName = $("#BranchNameCardInternalDeposit").val();
                    $("#branchNameSuccessModal").val(branchName);
                    $("#paymentTicketCodeSuccessModal").val(data[0].Id);
                    $("#paymentMethodTypeCodeSuccessModal").val(data[0].MethodType);
                    var ballotNumber = ("000000000" + data[0].Id).slice(-10);
                    $("#paymentTicketItemIdSuccessModal").val(ballotNumber);
                    var issuingBankName = $("#ReceivingBankNameCardInternalDeposit").val();
                    $("#receivingBankNameSuccessModal").val(issuingBankName);
                    var accountNumber = $("#ReceivingAccountNumberCardInternalDeposit").val();
                    $("#accountBankSuccessModal").val(accountNumber);
                    $("#dateBallotSuccessModal").val(data[0].Date);
                    var accountCurrency = $("#AccountCurrencyNameCardInternalDeposit").val();
                    $("#accountCurrencySuccessModal").val(accountCurrency);
                    $("#userBallotSuccessModal").val(data[0].User);

                    if ($("#CardAmount").val() == "") {
                        var cardAmount = "0";
                    }
                    else {
                        var cardAmount = $("#CardAmount").val();
                    }

                    $("#totalCardsSuccessModal").val(FormatCurrency(FormatDecimal(data[0].Total)));
                    var totalBallot = 0;
                    totalBallot = parseFloat(data[0].Total);

                    $("#totalBallotSuccessModal").val(FormatCurrency(FormatDecimal(totalBallot)));

                    $('#modalView').UifModal('hide');
                    $('#modalView1').UifModal('showLocal', Resources.InternalBallotCardDialogTitle + ' ' + data[0].Id);
                    setCardDataFieldsAEmpty();
                    oTblChecks = {
                        PaymentTicket: []
                    };
                    var firstEntry = 0;
                });
            });
        }
        else {
            $("#alertCardInternalDeposit").UifAlert('show', Resources.InternalBallotCardValidateSaveMessage, "danger");
        }
    });

    var rowDeleteSelected;

    //SELECCIONAR ELIMINAR DE LA GRILLA Tarjetas Seleccionadas
    $('#ACardsDialogCardInternalDeposit').on('rowDelete', function (event, data, position) {
        index = position;
        rowDeleteSelected = data;
        $('#modalDeleteBallot').appendTo("body").modal('show');
    });


    //ACEPTAR ELIMINAR Tarjetas Seleccionadas
    $("#DeleteModalCardDeposit").on('click', function () {
        $("#alertCardInternalDeposit").UifAlert('hide');

        $('#modalDeleteBallot').modal('hide');
        $('#ACardsDialogCardInternalDeposit').UifDataTable('deleteRow', index);
        deleteRecordsCardDeposit(rowDeleteSelected);
        AddCardsSelected();

    });


    //IMPRIMIR BOLETA
    $("#modalView1").find('#PrintReportSuccessModalCardInternal').click(function () {
        $("#alertCardInternalDeposit").UifAlert('hide');
        //Muestra reporte cargado
        loadReportCardSearch($("#paymentTicketCodeSuccessModal").val(), $("#paymentMethodTypeCodeSuccessModal").val());
        $('#modalView1').UifModal('hide');
    });


    //BANCO EMISOR
    $('#IssuingBankCardInternalDeposit').on('itemSelected', function (event, selectedItem) {
        issuingBankCodeAut = 0;
        issuingBankCodeAut = selectedItem.bankId;
    });

    //BANCO EMISOR PIERDE EL FOCO
    $("#IssuingBankCardInternalDeposit").blur(function () {

        if ($("#IssuingBankCardInternalDeposit").val() == "") {
            issuingBankCodeAut = -1;
        }
    });

    function ticketCardDepositSearch() {
        $("#alertCardVoucher").UifAlert('hide');
        var paymentTicketId = $("#BallotNumberCardInternalDeposit").val();

        $.each($('#ACardsDialogCardInternalDeposit').UifDataTable("getData"), function (i, value) {
            cleanObjectCardSearch();
            oPaymentTicket.PaymentTicketId = paymentTicketId;
            oPaymentTicket.PaymentTicketItemId = 0;
            oPaymentTicket.PaymentId = value.PaymentCode;
            oTblChecks.PaymentTicket.push(oPaymentTicket);
        });
        return oTblChecks;
    };
});
