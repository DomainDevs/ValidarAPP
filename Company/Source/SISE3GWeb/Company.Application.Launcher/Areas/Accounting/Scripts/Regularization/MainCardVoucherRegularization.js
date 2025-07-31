var exchangeRate = 1;
var TransferIssuingBankId = -1;
var CreditCardIssuingBankId = -1;
var DepositVoucherReceivingBankId = -1;
var branchId = 0;
var billIdResult = 0;
var IssuingBankIdCardRegularization = -1;
var RejectedPaymentId = 0;
var SourcePaymentId = 0;
var Numerrors = 0;
var Alert = 0;
var CheckDocumentNumber = 0;
var IssuingAccountNumber = 0;
var idBillControl = 0;
var isOpen = false;
var localCurrencyId = 0;
var idBillConcept = 8;

setTimeout(function () {
    localCurrencyId = $("#ViewBagLocalCurrencyId").val();
}, 500);

var TransferIssuingBankId = -1;
var TransferReceivingBankId = -1;
var totalAmount = 0;
var totAmount = 0;
var amountLocalCurrency = 0;
var amountForeingCurrency = 0;
var payerId = -1;

var CreditCardIssuingBankId = -1;
var DepositVoucherReceivingBankId = -1;
var VoucherNumber = 0;
var params = {};
var result = true;
var validBank = true;

var paySummaryCardRegularization = {
    PaymentTypeId: 0,
    PaymentTypeDescription: "",
    Amount: 0,
    Exchange: 0,
    CurrencyId: 0,
    Currency: null,
    LocalAmount: 0,
    DocumentNumber: 0,
    VoucherNumber: 0,
    CardNumber: 0,
    CardType: 0,
    CardTypeName: null,
    AuthorizationNumber: 0,
    CardsName: null,
    ValidThruMonth: 0,
    ValidThruYear: 0,
    IssuingBankId: -1,
    IssuinBankName: "",
    IssuingBankAccountNumber: "",
    IssuerName: "",
    RecievingBankId: -1,
    RecievingBankName: "",
    RecievingBankAccountNumber: "",
    SerialVoucher: null,
    SerialNumber: null,
    Date: "",
    Tax: 0,
    TaxBase: 0
};

var oBillModel = {
    BillId: 0,
    BillingConceptId: 0,
    BillControlId: 0,
    RegisterDate: null,
    Description: null,
    PaymentsTotal: 0,
    PayerId: -1,
    SourcePaymentId: 0,
    PaymentSummary: [],
    PayerName:""
};

var oPaymentSummaryModel = {
    PaymentId: 0,
    BillId: 0,
    PaymentMethodId: 0,
    Amount: 0,
    CurrencyId: 0,
    LocalAmount: 0,
    ExchangeRate: 0,
    CheckPayments: [],
    CreditPayments: [],
    TransferPayments: [],
    DepositVouchers: [],
    RetentionReceipts: []
};

var oCheckModel = {
    DocumentNumber: null,
    IssuingBankId: -1,
    IssuingAccountNumber: null,
    IssuerName: null,
    Date: null
};

var oCreditModel = {
    CardNumber: null,
    Voucher: null,
    AuthorizationNumber: null,
    CreditCardTypeId: 0,
    IssuingBankId: -1,
    Holder: null,
    ValidThruYear: 0,
    ValidThruMonth: 0,
    TaxBase: 0
};

var oTransferModel = {
    DocumentNumber: null,
    IssuingBankId: -1,
    IssuingAccountNumber: null,
    IssuerName: null,
    ReceivingBankId: -1,
    ReceivingAccountNumber: null,
    Date: null
};

var oDepositVoucherModel = {
    VoucherNumber: null,
    ReceivingAccountBankId: -1,
    ReceivingAccountNumber: null,
    Date: null,
    DepositorName: null
};

var oRetentionReceiptModel = {
    BillNumber: null,
    AuthorizationNumber: null,
    SerialNumber: null,
    VoucherNumber: null,
    SerialVoucherNumber: null,
    Date: null
};


/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//FORMATOS BUTTONS /FECHAS/ #CARACTERES / NÚMEROS-DECIMALES
/*-----------------------------------------------------------------------------------------------------------------------------------------*/


//$("#TotalAmountCardRegularization").attr("maxlength", 18);
$("#CashAmountRegularization").attr("maxlength", 18);
$("#CheckAmount").attr("maxlength", 18);
$("#CreditCardAmount").attr("maxlength", 18);
$("#TransferAmount").attr("maxlength", 18);
$("#DepositVoucherAmount").attr("maxlength", 18);
$("#RetentionReceiptAmount").attr("maxlength", 18);
$("#DocumentNumber").attr("maxlength", 12);
$("#VoucherNumber").attr("maxlength", 12);
$("#CreditCardNumber").attr("maxlength", 16);
$("#AuthorizationNumber").attr("maxlength", 12);
$("#TransferIssuingAccountNumber").attr("maxlength", 12);
$("#CheckDocumentNumber").attr("maxlength", 12);
$("#CheckAccountNumber").attr("maxlength", 12);
$("#CheckHolderName").attr("maxlength", 40);
$("#CreditCardAuthorizationNumber").attr("maxlength", 12);
$("#CreditCardVoucherNumber").attr("maxlength", 12);
$("#CreditCardHolderName").attr("maxlength", 40);
$("#TransferIssuingAccountNumber").attr("maxlength", 12);
$("#TransferHolderName").attr("maxlength", 40);
$("#DepositVoucherDocumentNumberSelect").attr("maxlength", 12);
$("#DepositVoucherHolderName").attr("maxlength", 40);
$("#RetentionReceiptSerialNumber").attr("maxlength", 7);
$("#RetentionReceiptDocumentNumber").attr("maxlength", 12);
$("#RetentionReceiptAuthorizationNumber").attr("maxlength", 10);
$("#RetentionReceiptVoucher").attr("maxlength", 9);
$("#RetentionReceiptSerialVoucher").attr("maxlength", 7);
$("#CreditCardTaxBase").attr("maxlength", 18);
$("#TransferAreaIssuingAccountNumber").attr("maxlength", 16);
$("#TransferDocumentNumberArea").attr("maxlength", 12);
$("#PaymentCreditCardNumber").attr("maxlength", 16);
$("#PaymentCreditCardAuthorizationNumber").attr("maxlength", 12);
$("#PaymentCreditCardVoucherNumber").attr("maxlength", 12);

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
if ($("#ViewBagCardRegularization").val() == "true") {
    //oculta para cheques
    $("#_accept").hide();
    $("#SelectBranchCheckRegularization").hide();

    //muestra para tarjetas
    $("#AcceptCardRegularization").show();

    $("#divCash").hide();
    $("#divCheck").hide();
    $("#divCreditCard").hide();
    $("#divTransfer").hide();
    $("#divDepositVoucher").hide();
    $("#divRetentionReceipt").hide();
}

setCurrencyComboCardRegularization($("#ViewBagLocalCurrencyId").val());
setCurrencyCardRegularization("CashCurrencySelect", "CashExchangeRate");
clearPaymentMethodFieldsCard();


setTimeout(function () {
    getDateCardRegularization();

    var controller = ACC_ROOT + "Common/GetAccountByBankIdSelect?bankId=" + 0;
    $("#AccountNumberReceivingBankSelect").UifSelect({ source: controller });
}, 500);


loadCardVoucherRegularization();


$("#CashLocalAmount").trigger('blur');
$("#CheckLocalAmount").trigger('blur');
$("#CreditCardLocalAmount").trigger('blur');
$("#TransferLocalAmount").trigger('blur');
$("#DepositVoucherLocalAmount").trigger('blur');
$("#RetentionReceiptLocalAmount").trigger('blur');



$('#DocumentNumber').on('itemSelected', function (event, selectedItem) {
    fillPersonAutocompletes(selectedItem);
});

//Autocomplete Nombres
$('#FirstLastName').on('itemSelected', function (event, selectedItem) {
    fillPersonAutocompletes(selectedItem);
});


$("#CashAmountRegularization").blur(function () {
    $("#alert").UifAlert('hide');
    if ($.trim($("#CashAmountRegularization").val()) == "$") {
        $("#CashAmountRegularization").val('');
    } else
        $("#CashAmountRegularization").val($.trim(ClearFormatCurrency($("#CashAmountRegularization").val())));

    setCurrencyNoMsgCardRegularization("CashCurrencySelect", "CashExchangeRate");
    if ($("#CashAmountRegularization").val() != "" && $("#CashExchangeRate").val() != "") {
        $("#CashAmountRegularization").val(FormatCurrency($("#CashAmountRegularization").val()));
        $("#CashLocalAmount").val(parseFloat(ClearFormatCurrency($("#CashAmountRegularization").val().replace("", ",")) * exchangeRate).toFixed(2));
        $("#CashLocalAmount").val(FormatCurrency($("#CashLocalAmount").val()));
    } else {
        if ($("#CashAmountRegularization").val() != "") {
            $("#CashAmountRegularization").val(FormatCurrency($("#CashAmountRegularization").val()));
        }
        $("#CashLocalAmount").val("");
        $("#CashExchangeRate").val("");
    }
});

$("#CheckAmount").blur(function () {
    $("#alert").UifAlert('hide');
    if ($.trim($("#CheckAmount").val()) == "$") {
        $("#CheckAmount").val('');
    } else
        $("#CheckAmount").val($.trim(ClearFormatCurrency($("#CheckAmount").val())));

    setCurrencyNoMsgCardRegularization("CheckCurrencySelect", "CheckExchangeRate");
    if ($("#CheckAmount").val() != "" && $("#CheckExchangeRate").val() != "") {
        $("#CheckLocalAmount").val(parseFloat(ClearFormatCurrency($("#CheckAmount").val().replace("", ",")) * exchangeRate).toFixed(2));
        $("#CheckAmount").val(FormatCurrency($("#CheckAmount").val()));
        $("#CheckLocalAmount").val(FormatCurrency($("#CheckLocalAmount").val()));
    } else {
        if ($("#CheckAmount").val() != "") {
            $("#CheckAmount").val(FormatCurrency($("#CheckAmount").val()));
        }
        $("#CheckLocalAmount").val("");
        $("#CheckExchangeRate").val("");
    }
});

$("#CreditCardAmount").blur(function () {
    $("#alert").UifAlert('hide');
    $("#CreditCardAmount").val($.trim(ClearFormatCurrency($("#CreditCardAmount").val())));
    setCurrencyNoMsgCardRegularization("CreditCardCurrencySelect", "CreditCardExchangeRate");
    $("#CreditCardLocalAmount").val(parseFloat(ClearFormatCurrency($("#CreditCardAmount").val().replace("", ",")) * exchangeRate).toFixed(2));
    $("#CreditCardAmount").val(FormatCurrency($("#CreditCardAmount").val()));
    $("#CreditCardLocalAmount").val(FormatCurrency($("#CreditCardLocalAmount").val()));
});

$("#TransferAmount").blur(function () {
    $("#alert").UifAlert('hide');
    if ($.trim($("#TransferAmount").val()) == "$") {
        $("#TransferAmount").val('');
    } else
        $("#TransferAmount").val($.trim(ClearFormatCurrency($("#TransferAmount").val())));

    setCurrencyNoMsgCardRegularization("TransferCurrencySelect", "TransferExchangeRate");
    if ($("#TransferAmount").val() != "" && $("#TransferExchangeRate").val() != "") {
        $("#TransferLocalAmount").val(parseFloat(ClearFormatCurrency($("#TransferAmount").val().replace("", ",")) * exchangeRate).toFixed(2));
        $("#TransferAmount").val(FormatCurrency($("#TransferAmount").val()));
        $("#TransferLocalAmount").val(FormatCurrency($("#TransferLocalAmount").val()));
    } else {
        if ($("#TransferAmount").val() != "") {
            $("#TransferAmount").val(FormatCurrency($("#TransferAmount").val()));
        }
        $("#TransferLocalAmount").val("");
        $("#TransferExchangeRate").val("");
    }

});

$("#DepositVoucherAmount").blur(function () {
    $("#alert").UifAlert('hide');
    if ($.trim($("#DepositVoucherAmount").val()) == "$") {
        $("#DepositVoucherAmount").val('');
    } else
        $("#DepositVoucherAmount").val($.trim(ClearFormatCurrency($("#DepositVoucherAmount").val())));

    setCurrencyNoMsgCardRegularization("DepositVoucherCurrencySelect", "DepositVoucherExchangeRate");
    if ($("#CashAmountRegularization").val() != "" && $("#CashExchangeRate").val() != "") {
        $("#DepositVoucherLocalAmount").val(parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")) * exchangeRate).toFixed(2));
        $("#DepositVoucherAmount").val(FormatCurrency($("#DepositVoucherAmount").val()));
        $("#DepositVoucherLocalAmount").val(FormatCurrency($("#DepositVoucherLocalAmount").val()));
    } else {
        if ($("#DepositVoucherAmount").val() != "") {
            $("#DepositVoucherAmount").val(FormatCurrency($("#DepositVoucherAmount").val()));
        }
        $("#DepositVoucherLocalAmount").val("");
        $("#DepositVoucherExchangeRate").val("");
    }

});

$("#RetentionReceiptAmount").blur(function () {

    $("#alert").UifAlert('hide');
    if ($.trim($("#RetentionReceiptAmount").val()) == "$") {
        $("#RetentionReceiptAmount").val('');
    } else
        $("#RetentionReceiptAmount").val($.trim(ClearFormatCurrency($("#RetentionReceiptAmount").val())));

    setCurrencyNoMsgCardRegularization("RetentionReceiptCurrencySelet", "RetentionReceiptExchangeRate");
    if ($("#RetentionReceiptAmount").val() != "" && $("#RetentionReceiptExchangeRate").val() != "") {
        $("#RetentionReceiptLocalAmount").val(parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")) * exchangeRate).toFixed(2));
        $("#RetentionReceiptAmount").val(FormatCurrency($("#RetentionReceiptAmount").val()));
        $("#RetentionReceiptLocalAmount").val(FormatCurrency($("#RetentionReceiptLocalAmount").val()));
    } else {
        if ($("#RetentionReceiptAmount").val() != "") {
            $("#RetentionReceiptAmount").val(FormatCurrency($("#RetentionReceiptAmount").val()));
        }
        $("#RetentionReceiptLocalAmount").val("");
        $("#RetentionReceiptExchangeRate").val("");
    }
});

$("#TransferAmountArea").blur(function () {
    $("#alert").UifAlert('hide');
    if ($.trim($("#TransferAmountArea").val()) == "$") {
        $("#TransferAmountArea").val('');
    } else
        $("#TransferAmountArea").val($.trim(ClearFormatCurrency($("#TransferAmountArea").val())));

    setCurrencyNoMsgCardRegularization("TransferCurrencyAreaSlect", "TransferExchangeRateArea");
    if ($("#TransferAmountArea").val() != "" && $("#TransferExchangeRateArea").val() != "") {
        $("#TransferAreaLocalAmount").val(parseFloat(ClearFormatCurrency($("#TransferAmountArea").val().replace("", ",")) * exchangeRate).toFixed(2));
        $("#TransferAmountArea").val(FormatCurrency($("#TransferAmountArea").val()));
        $("#TransferAreaLocalAmount").val(FormatCurrency($("#TransferAreaLocalAmount").val()));
    } else {
        if ($("#TransferAmountArea").val() != "") {
            $("#TransferAmountArea").val(FormatCurrency($("#TransferAmountArea").val()));
        }
        $("#TransferAreaLocalAmount").val("");
        $("#TransferExchangeRateArea").val("");
    }

});

$("#PaymentCreditCardAmount").blur(function () {
    $("#alert").UifAlert('hide');
    if ($.trim($("#PaymentCreditCardAmount").val()) == "$") {
        $("#PaymentCreditCardAmount").val('');
    } else
        $("#PaymentCreditCardAmount").val($.trim(ClearFormatCurrency($("#PaymentCreditCardAmount").val())));

    setCurrencyNoMsgCardRegularization("CreditCardCurrencyPaymentSelet", "PaymentCreditCardExchangeRate");

    if ($("#PaymentCreditCardAmount").val() != "" && $("#PaymentCreditCardExchangeRate").val() != "") {
        $("#PaymentCreditCardLocalAmount").val(parseFloat(ClearFormatCurrency($("#PaymentCreditCardAmount").val().replace("", ",")) * exchangeRate).toFixed(2));
        $("#PaymentCreditCardAmount").val(FormatCurrency($("#PaymentCreditCardAmount").val()));
        $("#PaymentCreditCardLocalAmount").val(FormatCurrency($("#PaymentCreditCardLocalAmount").val()));
    } else {
        if ($("#PaymentCreditCardAmount").val() != "") {
            $("#PaymentCreditCardAmount").val(FormatCurrency($("#PaymentCreditCardAmount").val()));
        }
        $("#PaymentCreditCardLocalAmount").val("");
        $("#PaymentCreditCardExchangeRate").val("");
    }

});

////Valida que no se ingresen cheques postfechados cuando esta seleccionado "Cheque Corriente"
//$("#CheckDateRegularizated").blur(function () {
//    $("#alert").UifAlert('hide');
//    if ($("#CheckDateRegularizated").val() != '') {
//        if (IsDate($("#CheckDateRegularizated").val()) == true) {
//            if (CompareDates($("#CheckDateRegularizated").val(), getCurrentDate()) == 2) {
//                $("#CheckDateRegularizated").val($("#BillingDate").val());
//            }
//        }
//        else {
//            $("#alert").UifAlert('show', Resources.InvalidDates, 'warning');
//            $("#CheckDateRegularizated").val($("#BillingDate").val());
//        }
//    }
//});

//Valida que no ingresen una fecha invalida.
$("#TransferDate").blur(function () {
    $("#alert").UifAlert('hide');
    if ($("#TransferDate").val() != '') {

        if (IsDate($("#TransferDate").val()) == true) {

            if (CompareDates($("#TransferDate").val(), getCurrentDate()) == 2) {
                $("#TransferDate").val($("#BillingDate").val());
            }
        }
        else {

            $("#alert").UifAlert('show', Resources.InvalidDates, 'warning');
            $("#TransferDate").val($("#BillingDate").val());
        }
    }
});


//Valida que no ingresen una fecha invalida en voleta de deposito
$("#DepositVoucherDate").blur(function () {
    $("#alert").UifAlert('hide');
    if ($("#DepositVoucherDate").val() != '') {

        if (IsDate($("#DepositVoucherDate").val()) == true) {

            if (CompareDates($("#DepositVoucherDate").val(), getCurrentDate()) == 2) {
                $("#DepositVoucherDate").val($("#BillingDate").val());
            }
        }
        else {
            $("#alert").UifAlert('show', Resources.InvalidDates, 'warning');
            $("#DepositVoucherDate").val($("#BillingDate").val());
        }
    }
});


//Valida que no ingresen una fecha invalida en recibo de retención.
$("#RetentionReceiptDate").blur(function () {
    $("#alert").UifAlert('hide');
    if ($("#RetentionReceiptDate").val() != '') {

        if (IsDate($("#RetentionReceiptDate").val()) == true) {

            if (CompareDates($("#RetentionReceiptDate").val(), getCurrentDate()) == 2) {
                $("#RetentionReceiptDate").val($("#BillingDate").val());
            }
        }
        else {

            $("#alert").UifAlert('show', Resources.InvalidDates, 'warning');
            $("#RetentionReceiptDate").val($("#BillingDate").val());
        }
    }
});


//Valida que no ingresen una fecha invalida en transfer area.
$("#TransferAreaDate").blur(function () {
    $("#alert").UifAlert('hide');
    if ($("#TransferAreaDate").val() != '') {

        if (IsDate($("#TransferAreaDate").val()) == true) {

            if (CompareDates($("#TransferAreaDate").val(), getCurrentDate()) == 2) {
                $("#TransferAreaDate").val($("#BillingDate").val());
            }
        }
        else {

            $("#alert").UifAlert('show', Resources.InvalidDates, 'warning');
            $("#TransferAreaDate").val($("#BillingDate").val());
        }
    }
});

//Valida que solo se ingresen cheques postfechados con la fecha mayor a la actual
$("#CheckPostDate").siblings('.input-group-addon').off('click');
$("#CheckPostDate").focusout(function () {
    if ($("#CheckPostDate").val() != "__/__/____") {
        validPostDateDateCardRegularization();
        $("#AcceptCardRegularization").focus();
    } else {
        $("#CheckPostDate").focus();
    }
});


$("#PaymentCreditCardHolderName").blur(function () {
    $("#PaymentCreditCardHolderName").val($("#PaymentCreditCardHolderName").val().toUpperCase());
});

$("#CreditCardHolderName").blur(function () {
    $("#CreditCardHolderName").val($("#CreditCardHolderName").val().toUpperCase());
});

$("#CheckHolderName").blur(function () {
    $("#CheckHolderName").val($("#CheckHolderName").val().toUpperCase());
});

$("#TransferHolderName").blur(function () {
    $("#TransferHolderName").val($("#TransferHolderName").val().toUpperCase());
});

$("#DepositVoucherHolderName").blur(function () {
    $("#DepositVoucherHolderName").val($("#DepositVoucherHolderName").val().toUpperCase());
});

$("#TransferAreaHolderName").blur(function () {
    $("#TransferAreaHolderName").val($("#TransferAreaHolderName").val().toUpperCase());
});

//Accion para ejecutar ocultado de paneles de tipos de pago.
$("#PaymentMethodType").on('itemSelected', function (event, selectedItem) {

    clearPaymentMethodFieldsCard();
    switch ($("#PaymentMethodType").val()) {
        case $("#ViewBagParamPaymentMethodCurrentCheck").val():
            //cambio para BE
        case $("#ViewBagParamPaymentMethodPostdatedCheck").val():
        case $("#ViewBagParamPaymentMethodDebit").val():
            //Cheque y debito

            $("#divCash").hide();
            $("#divCheck").show();
            $("#divCreditCard").hide();
            $("#divTransfer").hide();
            $("#divDepositVoucher").hide();
            $("#divRetentionReceipt").hide();
            $("#divPaymentArea").hide();
            $("#divPaymentCard").hide();
            //cheque postfechado
            if ($("#PaymentMethodType").val() == $("#ViewBagParamPaymentMethodPostdatedCheck").val()) {
                $("#CheckPostDate").siblings('.input-group-addon').show();
                $("#CheckPostDate").show();
                $("#CheckDateRegularizated").hide();
                $("#CheckDateRegularizated").siblings('.input-group-addon').hide();
                //cheque corriente
            } else if ($("#PaymentMethodType").val() == $("#ViewBagParamPaymentMethodCurrentCheck").val()) {
                $("#CheckDateRegularizated").show();
                $("#CheckDateRegularizated").siblings('.input-group-addon').show();
                $("#CheckPostDate").hide();
                $("#CheckPostDate").siblings('.input-group-addon').hide();
                //debito
            } else if ($("#PaymentMethodType").val() == $("#ViewBagParamPaymentMethodDebit").val()) {
                $("#CheckDateRegularizated").show();
                $("#CheckDateRegularizated").siblings('.input-group-addon').show();
                $("#CheckPostDate").hide();
                $("#CheckPostDate").siblings('.input-group-addon').hide();
            }
            break;

            //cambio para BE
        case $("#ViewBagParamPaymentMethodDirectConection").val():
        case $("#ViewBagParamPaymentMethodTransfer").val():
            //Transferencia

            $("#divCash").hide();
            $("#divCheck").hide();
            $("#divCreditCard").hide();
            $("#divTransfer").show();
            $("#divDepositVoucher").hide();
            $("#divRetentionReceipt").hide();
            $("#divPaymentArea").hide();
            $("#divPaymentCard").hide();
            break;

            //case "": //cambio para BE
        case $("#ViewBagParamPaymentMethodCreditableCreditCard").val():
        case $("#ViewBagParamPaymentMethodUncreditableCreditCard").val():
        case $("#ViewBagParamPaymentMethodDataphone").val():
            //Tarjeta
            $("#divCash").hide();
            $("#divCheck").hide();
            $("#divCreditCard").show();
            $("#divTransfer").hide();
            $("#divDepositVoucher").hide();
            $("#divRetentionReceipt").hide();
            $("#divPaymentArea").hide();
            $("#divPaymentCard").hide();
            break;

        case $("#ViewBagParamPaymentMethodCash").val():

            //Efectivo
            $("#divCash").show();
            $("#divCheck").hide();
            $("#divCreditCard").hide();
            $("#divTransfer").hide();
            $("#divDepositVoucher").hide();
            $("#divRetentionReceipt").hide();
            $("#divPaymentArea").hide();
            $("#divPaymentCard").hide();
            break;

        case $("#ViewBagParamPaymentMethodDepositVoucher").val():
            //boleta de deposito

            $("#divCash").hide();
            $("#divCheck").hide();
            $("#divCreditCard").hide();
            $("#divTransfer").hide();
            $("#divDepositVoucher").show();
            $("#divRetentionReceipt").hide();
            $("#divPaymentArea").hide();
            $("#divPaymentCard").hide();
            break;

        case $("#ViewBagParamPaymentMethodRetentionReceipt").val():
            //recibo de retencion

            $("#divCash").hide();
            $("#divCheck").hide();
            $("#divCreditCard").hide();
            $("#divTransfer").hide();
            $("#divDepositVoucher").hide();
            $("#divRetentionReceipt").show();
            $("#divPaymentArea").hide();
            $("#divPaymentCard").hide();
            break;

        case $("#ViewBagParamPaymentMethodPaymentArea").val():
            //Zona de pagos

            $("#divCash").hide();
            $("#divCheck").hide();
            $("#divCreditCard").hide();
            $("#divTransfer").hide();
            $("#divDepositVoucher").hide();
            $("#divRetentionReceipt").hide();
            $("#divPaymentArea").show();
            $("#divPaymentCard").hide();
            break;

        case $("#ViewBagParamPaymentMethodPaymentCard").val():
            //pago con tarjeta

            $("#divCash").hide();
            $("#divCheck").hide();
            $("#divCreditCard").hide();
            $("#divTransfer").hide();
            $("#divDepositVoucher").hide();
            $("#divRetentionReceipt").hide();
            $("#divPaymentArea").hide();
            $("#divPaymentCard").show();
            break;

        default:
            $("#divCash").hide();
            $("#divCheck").hide();
            $("#divCreditCard").hide();
            $("#divTransfer").hide();
            $("#divDepositVoucher").hide();
            $("#divRetentionReceipt").hide();
            $("#divPaymentArea").hide();
            $("#divPaymentCard").hide();
            $("#CheckPostDate").hide();
            $("#CheckDateRegularizated").hide();
    }

});

$("#PaymentMethodType").val($("#ViewBagParamPaymentMethodCash").val());
$("#PaymentMethodType").trigger('change');
$("#divCash").show();

/// Combo TransferReceivingBankSelect
$("#TransferReceivingBankSelect").on('itemSelected', function (event, selectedItem) {
    if (selectedItem != null) {
        var controller = ACC_ROOT + "Billing/GetAccountByBankId?bankId=" + selectedItem.Id;
        $("#AccountNumberReceivingBankSelect").UifSelect({ source: controller });
    }
});

/// Combo DepositVoucherBankReceivingSelect
$("#DepositVoucherBankReceivingSelect").on('itemSelected', function (event, selectedItem) {
    if (selectedItem != null) {
        var controller = ACC_ROOT + "Billing/GetAccountByBankId?bankId=" + selectedItem.Id;
        $("#DepositVoucherReceivingAccountNumberSelect").UifSelect({ source: controller });
    }
});

//zona de pagos
$("#TransferAreaDate").val(getCurrentDate());

/// Combo TransferReceivingBankSelect
$("#TransferAreaReceivingBank").on('itemSelected', function (event, selectedItem) {
    if (selectedItem != null) {
        var controller = ACC_ROOT + "Billing/GetAccountByBankId?bankId=" + selectedItem.Id;
        $("#TransferAreaReceivingAccountNumberBankSelect").UifSelect({ source: controller });
        numTransf++;
        $("#TransferDocumentNumberArea").val(numTransf);
    }
});

//Valida si un número de boleta ya ha sido registrado
$("#DepositVoucherDocumentNumberSelect").blur(function () {
    validDepositVoucherDocumentNumberCardRegularization();
});

//Valida si un número de cheque para un determinado Banco ya ha sido registrado
$("#CheckDocumentNumber").blur(function () {
    validCheckDocumentNumberCardRegularization();
});

//Valida si un número de cheque para un determinado Banco ya ha sido registrado
$("#TransferIssuingAccountNumber").blur(function () {
    validTransferDocumentNumberCardRegularization();
});

//Valida si un número de voucher para un determinado Número de Tarjeta ya ha sido registrado
$("#CreditCardVoucherNumber").blur(function () {
    validCreditCardVoucherNumberCardRegularization();
});

//Valida si un número de voucher para un determinado Número de Tarjeta ya ha sido registrado
$("#PaymentCreditCardVoucherNumber").blur(function () {
    validCreditCardVoucherNumberPayment();
});

////Valida que no se ingresen cheques postfechados
//$("#CheckDateRegularizated").blur(function () {
//    if ($("#CheckDateRegularizated").val() != '') {

//        if (IsDate($("#CheckDateRegularizated").val()) == true) {

//            if (CompareDates($("#CheckDateRegularizated").val(), getCurrentDate()) == 2) {
//                $("#CheckDateRegularizated").val($("#BillingDate").val());
//            }
//        } else {

//            $("#alert").UifAlert('show', Resources.InvalidDates, 'warning');
//            $("#CheckDateRegularizated").val($("#BillingDate").val());
//        }
//    }
//});


// Autocomplete TransferIssuingBank
$('#TransferIssuingBank').on('itemSelected', function (event, selectedItem) {
    TransferIssuingBankId = selectedItem.Id;
});

// Autocomplete CheckBank
$('#CheckBank').on('itemSelected', function (event, selectedItem) {
    IssuingBankIdCardRegularization = selectedItem.Id;
});

// Autocomplete CreditCardIssuingBankInput
$('#CreditCardIssuingBankInput').on('itemSelected', function (event, selectedItem) {
    CreditCardIssuingBankId = selectedItem.Id;
});

// Autocomplete CreditCardIssuingBankInput
$('#DepositVoucherReceivingBank').on('itemSelected', function (event, selectedItem) {
    DepositVoucherReceivingBankId = selectedItem.Id;
});

//Tipo de moneda en pago con cheque
$('#CheckCurrencySelect').change(function () {
    if ($('#CheckCurrencySelect').val() != "") {
        setCurrencyCardRegularization("CheckCurrencySelect", "CheckExchangeRate");
        $("#CheckAmount").trigger('blur');
    } else {
        $('#CheckExchangeRate').val("");
    }
});

//Tipo de moneda en pago con transferencia
$('#TransferCurrencySelect').change(function () {
    if ($('#CheckCurrencySelect').val() != "") {
        setCurrencyCardRegularization("TransferCurrencySelect", "TransferExchangeRate");
        $("#TransferAmount").trigger('blur');
    } else {
        $('#TransferExchangeRate').val("");
    }
});

//Tipo de moneda en pago con tarjeta
$('#CreditCardCurrencySelect').change(function () {
    if ($('#CreditCardCurrencySelect').val() != "") {
        setCurrencyCardRegularization("CreditCardCurrencySelect", "CreditCardExchangeRate");
        $("#CreditCardAmount").trigger('blur');
    } else {
        $('#CreditCardExchangeRate').val("");
    }
});

//Tipo de moneda en Pago efectivo
$('#CashCurrencySelect').change(function () {
    $("#alert").UifAlert('hide');
    if ($('#CashCurrencySelect').val() != "") {
        setCurrencyCardRegularization("CashCurrencySelect", "CashExchangeRate");
        $("#CashAmountRegularization").trigger('blur');
    } else {
        $('#CashExchangeRate').val("");
    }
});

//Tipo de moneda en pago con voleta de deposito
$('#DepositVoucherCurrencySelect').change(function () {
    if ($('#DepositVoucherCurrencySelect').val() != "") {
        setCurrencyCardRegularization("DepositVoucherCurrencySelect", "DepositVoucherExchangeRate");
        $("#DepositVoucherAmount").trigger('blur');
    } else {
        $('#DepositVoucherExchangeRate').val("");
    }
});

//Tipo de moneda en pago con voleta de retención
$('#RetentionReceiptCurrencySelet').change(function () {
    if ($('#RetentionReceiptCurrencySelet').val() != "") {
        setCurrencyCardRegularization("RetentionReceiptCurrencySelet", "RetentionReceiptExchangeRate");
        $("#RetentionReceiptAmount").trigger('blur');
    } else {
        $('#RetentionReceiptExchangeRate').val("");
    }
});

//Tipo de moneda en pago  de Area de pagos
$('#TransferCurrencyAreaSlect').change(function () {
    if ($('#TransferCurrencyAreaSlect').val() != "") {
        setCurrencyCardRegularization("TransferCurrencyAreaSlect", "TransferExchangeRateArea");
        $("#TransferAmountArea").trigger('blur');
    } else {
        $('#TransferExchangeRateArea').val("");
    }
});


//Tipo de moneda en pago con tarjea acreditable
$('#CreditCardCurrencyPaymentSelet').change(function () {
    if ($('#CreditCardCurrencyPaymentSelet').val() != "") {
        setCurrencyCardRegularization("CreditCardCurrencyPaymentSelet", "PaymentCreditCardExchangeRate");
        $("#PaymentCreditCardAmount").trigger('blur');
    } else {
        $('#PaymentCreditCardExchangeRate').val("");
    }
});

$("#CreditCardValidThruYear").blur(function () {
    checkExpirationDateCardRegularization();
});


$("#CreditCardValidThruMonthSelect").change(function () {
    if ($("#CreditCardValidThruYear").val() != "")
        checkExpirationDateCardRegularization();
});


//Inicializacion de campos al cargar la página
$("#CashExchangeRate").val("");
$('#CheckExchangeRate').val("");
$('#TransferExchangeRate').val("");
$('#CreditCardExchangeRate').val("");
$('#DepositVoucherExchangeRate').val("");
$('#RetentionReceiptExchangeRate').val("");
$('#TransferExchangeRateArea').val("");
$('#PaymentCreditCardExchangeRate').val("");
//});


$("#TotalAmountCardRegularization").attr('disabled', '');

//FORMATOS/NÚMEROS-DECIMALES
$("#TotalAmountCardRegularization").blur(function () {
    $("#DifferenceCardRegularization").val($("#TotalAmountCardRegularization").val());
    setDifferenceCardRegularization();
});

$("#DifferenceCardRegularization").blur(function () {
    var difference = $("#DifferenceCardRegularization").val();
    $("#DifferenceCardRegularization").val(FormatCurrency(FormatDecimal(difference)));
});


$("#paymentsAmount").blur(function () {
    var paymentsAmount = $("#paymentsAmount").val();
    $("#paymentsAmount").val(FormatCurrency(FormatDecimal(paymentsAmount)));

});

////Valida que no se ingresen cheques postfechados
//$("#CheckDateRegularizated").blur(function () {

//    if ($("#CheckDateRegularizated").val() != '') {

//        if (IsDate($("#CheckDateRegularizated").val()) == true) {

//            if (CompareDates($("#CheckDateRegularizated").val(), getCurrentDate()) == 2) {
//                $("#CheckDateRegularizated").val($("#BillingDate").val());
//            }
//        } else {

//            $("#alert").UifAlert('show', Resources.InvalidDates, 'warning');
//            $("#CheckDateRegularizated").val($("#BillingDate").val());
//        }
//    }
//});


//LLAMADA AL DIALOGO APERTURA DE CAJA Y CIERRE DE CAJA
$("#SelectBranchCardRegularization").on('itemSelected', function (event, selectedItem) {

    if ($("#SelectBranchCardRegularization").val() != "") {
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Billing/NeedCloseBill",
            data: { "branchId": $("#SelectBranchCardRegularization").val(), "accountingDatePresent": $("#ViewBagDateAccounting").val() },
            success: function (userData) {
                if (userData[0].resp == true) {
                    $('#modalBillingClosure').UifModal('showLocal', Resources.Closing);
                } else {
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: ACC_ROOT + "Billing/AllowOpenBill",
                        data: { "branchId": $("#SelectBranchCardRegularization").val(), "accountingDate": $("#ViewBagDateAccounting").val() },
                        success: function (userata) {
                            if (userata[0].resp == true) {
                                //OpenBillingDialogCardRegularization();
                                $('#modalOpeningBilling').find("#Branch").val($("#SelectBranchCardRegularization option:selected").text());
                                $('#modalOpeningBilling').find("#AccountingDate").val($("#ViewBagAccountingDate").val());
                                $('#modalOpeningBilling').find("#UserId").val($("#ViewBagUserNick").val());
                                $('#modalOpeningBilling').UifModal('showLocal', Resources.OpeningBilling);
                            } else {
                                isOpen = true;
                            }
                        }
                    });
                }

                idBillControl = userData[0].Id;
            }
        });
    }
});

//BOTON CANCELAR
$("#CancelBilling").click(function () {
    clearFieldsCardRegularization();
    clearPaymentMethodFieldsCard();
    SetDataBillEmptyCardRegularization();
    location.href = $("#ViewBagInternalCardVoucher").val();  
});

//BOTON AÑADIR REGISTROS A LA TABLA DE RESUMEN DE TIPO DE PAGO
$("#AcceptCardRegularization").click(function () {
    $("#alert").UifAlert('hide');
    Numerrors = 0;
    if (validFieldsCardRegularization($("#PaymentMethodType").val()) == true) {

        paySummaryCardRegularization = {
            PaymentTypeId: 0,
            PaymentTypeDescription: "",
            Amount: 0,
            Exchange: 0,
            CurrencyId: 0,
            Currency: null,
            LocalAmount: 0,
            DocumentNumber: 0,
            VoucherNumber: 0,
            CardNumber: 0,
            CardType: 0,
            CardTypeName: null,
            AuthorizationNumber: 0,
            CardsName: null,
            ValidThruMonth: 0,
            ValidThruYear: 0,
            IssuingBankId: -1,
            IssuinBankName: "",
            IssuingBankAccountNumber: "",
            IssuerName: "",
            RecievingBankId: -1,
            RecievingBankName: "",
            RecievingBankAccountNumber: "",
            SerialVoucher: null,
            SerialNumber: null,
            Date: "",
            Tax: 0,
            TaxBase: 0
        };

        //Validacion de registros duplicados

        var keyValid = "";

        switch ($("#PaymentMethodType").val()) {

            case $("#ViewBagParamPaymentMethodCash").val():
                //1 Efectivo

                keyValid = $("#PaymentMethodType").val() + $("#CashCurrencySelect").val();
                validatePostDatedCheckCardRegularization();
                break;

            case $("#ViewBagParamPaymentMethodCurrentCheck").val():
            case $("#ViewBagParamPaymentMethodDebit").val():
            case $("#ViewBagParamPaymentMethodPostdatedCheck").val():
                //Cheque

                keyValid = $("#CheckDocumentNumber").val() + IssuingBankIdCardRegularization + $("#CheckAccountNumber").val();
                validCheckDocumentNumberCardRegularization();
                if (Numerrors == 0) {
                    validatePostDatedCheckCardRegularization();
                }
                break;

            case $("#ViewBagParamPaymentMethodCreditableCreditCard").val():
            case $("#ViewBagParamPaymentMethodUncreditableCreditCard").val():
            case $("#ViewBagParamPaymentMethodDataphone").val():
                //Tarjeta acreditable

                keyValid = $("#CreditCardNumber").val() + $("#CreditCardVoucherNumber").val();
                validCreditCardVoucherNumberCardRegularization();
                if (Numerrors == 0) {
                    checkExpirationDateCardRegularization();
                }
                if (Numerrors == 0) {
                    validatePostDatedCheckCardRegularization();
                }
                if (Numerrors == 0) {
                    validateTaxBase();
                }
                break;

            case $("#ViewBagParamPaymentMethodDirectConection").val():
            case $("#ViewBagParamPaymentMethodTransfer").val():
                //conexion directa y transferencia

                keyValid = $("#TransferIssuingAccountNumber").val() + TransferIssuingBankId + $("#TransferDocumentNumber").val();
                validTransferDocumentNumberCardRegularization();
                if (Numerrors == 0) {
                    validatePostDatedCheckCardRegularization();
                }
                break;

            case $("#ViewBagParamPaymentMethodDepositVoucher").val():
                //boleta de depósito

                keyValid = $("#DepositVoucherBankReceivingSelect option:selected").text() + $("#DepositVoucherDocumentNumberSelect").val() + $("#DepositVoucherReceivingAccountNumberSelect option:selected").text(); /*$("#PaymentMethodType").val() + $("#RetentionReceiptCurrencySelet").val() +*/
                validDepositVoucherDocumentNumberCardRegularization();
                if (Numerrors == 0) {
                    validatePostDatedCheckCardRegularization();
                }
                break;

            case $("#ViewBagParamPaymentMethodRetentionReceipt").val():
                //recibo de retencion

                keyValid = $("#PaymentMethodType").val() + $("#TransferCurrencySelect").val() + $("#RetentionReceiptDocumentNumber").val();
                validatePostDatedCheckCardRegularization();
                break;

            default:
        }
        //valida si el medio de pago no fue agregado todavía
        result = validarGridPaySummary(keyValid);
        validBank = validateBankIdCardRegularization();

        switch ($("#PaymentMethodType").val()) {
            case $("#ViewBagParamPaymentMethodCash").val():
                //Efectivo

                $("#divCash").validate();

                if ($("#divCash").valid() && result == true && Numerrors == 0) {

                    totAmount = totAmount + parseFloat(ClearFormatCurrency($("#CashAmountRegularization").val().replace("", ","))) * exchangeRate;
                    totAmount = parseFloat(totAmount.toFixed(2));

                    if ($("#CashCurrencySelect").val() == localCurrencyId) {
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#CashAmountRegularization").val().replace("", ",")));
                        amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                        $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                        $("#TotalCurrencyCardRegularization").trigger('blur');
                    }

                    if ($("#CashCurrencySelect").val() != localCurrencyId && $("#CashCurrencySelect").val() != -1) {
                        amountForeingCurrency = amountForeingCurrency + parseFloat(ClearFormatCurrency($("#CashAmountRegularization").val().replace("", ",")));
                        amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#CashLocalAmount").val().replace("", ",")));
                        $("#TotalExchangeRateCardRegularization").val(amountForeingCurrency);
                        $("#TotalExchangeRateCardRegularization").trigger('blur');
                        $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                        $("#TotalCurrencyCardRegularization").trigger('blur');
                    }

                    if (parseFloat(ClearFormatCurrency($("#TotalCurrencyCardRegularization").val().replace("", ","))) <= parseFloat(ClearFormatCurrency($("#TotalAmountCardRegularization").val().replace("", ",")))) {
                        $("#DifferenceCardRegularization").val(parseFloat(ClearFormatCurrency($("#TotalAmountCardRegularization").val().replace("", ","))) - totAmount);
                    }

                    paySummaryCardRegularization.PaymentTypeId = $("#PaymentMethodType").val();
                    paySummaryCardRegularization.PaymentTypeDescription = $("#PaymentMethodType option:selected").text();
                    paySummaryCardRegularization.Amount = $("#CashAmountRegularization").val();
                    paySummaryCardRegularization.Exchange = FormatCurrency(exchangeRate);
                    paySummaryCardRegularization.CurrencyId = $("#CashCurrencySelect").val();
                    paySummaryCardRegularization.Currency = $("#CashCurrencySelect option:selected").text();
                    paySummaryCardRegularization.LocalAmount = FormatCurrency(parseFloat(ClearFormatCurrency($("#CashAmountRegularization").val().replace("", ",")) * exchangeRate));
                    paySummaryCardRegularization.Date = $("#BillingDate").val();
                }
                break;

            case $("#ViewBagParamPaymentMethodCurrentCheck").val():
            case $("#ViewBagParamPaymentMethodDebit").val():
            case $("#ViewBagParamPaymentMethodPostdatedCheck").val():
                //Cheque y debito
                $("#divCheck").validate();
                if ($("#divCheck").valid() && result == true && Numerrors == 0 && validBank == true) {

                    totAmount = totAmount + parseFloat(ClearFormatCurrency($("#CheckAmount").val().replace("", ",")) * exchangeRate);
                    totAmount = parseFloat(totAmount.toFixed(2));

                    if ($("#CheckCurrencySelect").val() == localCurrencyId) {
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#CheckAmount").val().replace("", ",")));
                        amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                        $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                        $("#TotalCurrencyCardRegularization").trigger('blur');
                    }

                    if ($("#CheckCurrencySelect").val() != localCurrencyId && $("#CheckCurrencySelect").val() != -1) {
                        amountForeingCurrency = amountForeingCurrency + parseFloat(ClearFormatCurrency($("#CheckAmount").val().replace("", ",")));
                        amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#CheckLocalAmount").val().replace("", ",")));
                        $("#TotalExchangeRateCardRegularization").val(amountForeingCurrency);
                        $("#TotalExchangeRateCardRegularization").trigger('blur');
                        $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                        $("#TotalCurrencyCardRegularization").trigger('blur');
                    }

                    if (parseFloat(ClearFormatCurrency($("#TotalCurrencyCardRegularization").val().replace("", ","))) <= parseFloat(ClearFormatCurrency($("#TotalAmountCardRegularization").val().replace("", ",")))) {
                        $("#DifferenceCardRegularization").val(parseFloat(ClearFormatCurrency($("#TotalAmountCardRegularization").val().replace("", ","))) - totAmount);
                    }

                    paySummaryCardRegularization.PaymentTypeId = $("#PaymentMethodType").val();
                    paySummaryCardRegularization.PaymentTypeDescription = $("#PaymentMethodType option:selected").text();
                    paySummaryCardRegularization.Amount = $("#CheckAmount").val();
                    paySummaryCardRegularization.Exchange = FormatCurrency(exchangeRate);
                    paySummaryCardRegularization.CurrencyId = $("#CheckCurrencySelect").val();
                    paySummaryCardRegularization.Currency = $("#CheckCurrencySelect option:selected").text();
                    paySummaryCardRegularization.LocalAmount = FormatCurrency(parseFloat(ClearFormatCurrency($("#CheckAmount").val().replace("", ",")) * exchangeRate));
                    paySummaryCardRegularization.DocumentNumber = $("#CheckDocumentNumber").val();
                    paySummaryCardRegularization.IssuingBankId = IssuingBankIdCardRegularization;
                    paySummaryCardRegularization.IssuinBankName = $("#CheckBank").val();
                    paySummaryCardRegularization.IssuingBankAccountNumber = $("#CheckAccountNumber").val();
                    paySummaryCardRegularization.IssuerName = $("#CheckHolderName").val();
                    //cheque postfechado
                    if ($("#PaymentMethodType").val() == "1") {
                        paySummaryCardRegularization.Date = $("#CheckPostDate").val();
                        //cheque corriente
                    } else if ($("#PaymentMethodType").val() == "2") {
                        paySummaryCardRegularization.Date = $("#CheckDateRegularizated").val();
                    }
                }
                break;

            case $("#ViewBagParamPaymentMethodCreditableCreditCard").val():
            case $("#ViewBagParamPaymentMethodUncreditableCreditCard").val():
            case $("#ViewBagParamPaymentMethodDataphone").val():
                //Tarjeta

                $("#divCreditCard").validate();

                if ($("#divCreditCard").valid() || result == true && Numerrors == 0 && validBank == true) {

                    totAmount = totAmount + parseFloat(ClearFormatCurrency($("#CreditCardAmount").val().replace("", ",")) * exchangeRate);
                    totAmount = parseFloat(totAmount.toFixed(2));

                    if ($("#CreditCardCurrencySelect").val() == localCurrencyId) {
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#CreditCardAmount").val().replace("", ",")));
                        amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                        $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                        $("#TotalCurrencyCardRegularization").trigger('blur');
                    }

                    if ($("#CreditCardCurrencySelect").val() != localCurrencyId && $("#CreditCardCurrencySelect").val() != -1) {
                        amountForeingCurrency = amountForeingCurrency + parseFloat(ClearFormatCurrency($("#CreditCardAmount").val().replace("", ",")));
                        amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#CreditCardLocalAmount").val().replace("", ",")));
                        $("#TotalExchangeRateCardRegularization").val(amountForeingCurrency);
                        $("#TotalExchangeRateCardRegularization").trigger('blur');
                        $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                        $("#TotalCurrencyCardRegularization").trigger('blur');
                    }

                    if (parseFloat(ClearFormatCurrency($("#_TotalCurrency").val().replace("", ","))) <= parseFloat(ClearFormatCurrency($("#TotalAmountCardRegularization").val().replace("", ",")))) {
                        $("#_Difference").val(parseFloat(ClearFormatCurrency($("#TotalAmountCardRegularization").val().replace("", ","))) - totAmount);
                    }

                    paySummaryCardRegularization.PaymentTypeId = $("#PaymentMethodType").val();
                    paySummaryCardRegularization.PaymentTypeDescription = $("#PaymentMethodType option:selected").text();
                    paySummaryCardRegularization.Amount = $("#CreditCardAmount").val();
                    paySummaryCardRegularization.Exchange = FormatCurrency(exchangeRate);
                    paySummaryCardRegularization.CurrencyId = $("#CreditCardCurrencySelect").val();
                    paySummaryCardRegularization.Currency = $("#CreditCardCurrencySelect option:selected").text();
                    paySummaryCardRegularization.LocalAmount = FormatCurrency(parseFloat(ClearFormatCurrency($("#CreditCardAmount").val().replace("", ",")) * exchangeRate));
                    paySummaryCardRegularization.VoucherNumber = $("#CreditCardVoucherNumber").val();
                    paySummaryCardRegularization.CardNumber = $("#CreditCardNumber").val();
                    paySummaryCardRegularization.AuthorizationNumber = $("#CreditCardAuthorizationNumber").val();
                    paySummaryCardRegularization.IssuingBankId = CreditCardIssuingBankId;
                    paySummaryCardRegularization.IssuinBankName = $("#CreditCardIssuingBankInput").val();
                    paySummaryCardRegularization.CardsName = $("#CreditCardHolderName").val();
                    paySummaryCardRegularization.CardType = $("#CreditCardTypeSelect").val();
                    paySummaryCardRegularization.CardTypeName = $("#CreditCardTypeSelect option:selected").text();
                    paySummaryCardRegularization.ValidThruYear = $("#CreditCardValidThruYear").val();
                    paySummaryCardRegularization.ValidThruMonth = $("#CreditCardValidThruMonthSelect").val();
                    paySummaryCardRegularization.Tax = $("#tax").val();
                    paySummaryCardRegularization.TaxBase = parseFloat(ClearFormatCurrency($("#CreditCardTaxBase").val().replace("", ",")));
                }
                break;

            case $("#ViewBagParamPaymentMethodDirectConection").val():
            case $("#ViewBagParamPaymentMethodTransfer").val():
                //Transferencia
                $("#divTransfer").validate();
                if ($("#divTransfer").valid() || result == true && Numerrors == 0 && validBank == true) {

                    totAmount = totAmount + parseFloat(ClearFormatCurrency($("#TransferAmount").val().replace("", ",")) * exchangeRate);
                    totAmount = parseFloat(totAmount.toFixed(2));

                    if ($("#TransferCurrencySelect").val() == localCurrencyId) {
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#TransferAmount").val().replace("", ",")));
                        amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                        $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                        $("#TotalCurrencyCardRegularization").trigger('blur');
                    }

                    if ($("#TransferCurrencySelect").val() != localCurrencyId && $("#TransferCurrencySelect").val() != -1) {
                        amountForeingCurrency = amountForeingCurrency + parseFloat(ClearFormatCurrency($("#TransferAmount").val().replace("", ",")));
                        amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#TransferLocalAmount").val().replace("", ",")));
                        $("#TotalExchangeRateCardRegularization").val(amountForeingCurrency);
                        $("#TotalExchangeRateCardRegularization").trigger('blur');
                        $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                        $("#TotalCurrencyCardRegularization").trigger('blur');
                    }

                    if (parseFloat(ClearFormatCurrency($("#TotalCurrencyCardRegularization").val().replace("", ","))) <= parseFloat(ClearFormatCurrency($("#TotalAmountCardRegularization").val().replace("", ",")))) {
                        $("#DifferenceCardRegularization").val(parseFloat(ClearFormatCurrency($("#TotalAmountCardRegularization").val().replace("", ","))) - totAmount);
                    }

                    paySummaryCardRegularization.PaymentTypeId = $("#PaymentMethodType").val();
                    paySummaryCardRegularization.PaymentTypeDescription = $("#PaymentMethodType option:selected").text();
                    paySummaryCardRegularization.Amount = $("#TransferAmount").val();
                    paySummaryCardRegularization.Exchange = FormatCurrency(exchangeRate);
                    paySummaryCardRegularization.CurrencyId = $("#TransferCurrencySelect").val();
                    paySummaryCardRegularization.Currency = $("#TransferCurrencySelect option:selected").text();
                    paySummaryCardRegularization.LocalAmount = FormatCurrency(parseFloat(ClearFormatCurrency($("#TransferAmount").val().replace("", ",")) * exchangeRate));
                    paySummaryCardRegularization.DocumentNumber = $("#TransferIssuingAccountNumber").val();
                    paySummaryCardRegularization.IssuingBankId = TransferIssuingBankId;
                    paySummaryCardRegularization.IssuinBankName = $("#TransferIssuingBank").val();
                    paySummaryCardRegularization.IssuingBankAccountNumber = $("#TransferDocumentNumber").val();
                    paySummaryCardRegularization.RecievingBankId = $("#TransferReceivingBankSelect").val();
                    paySummaryCardRegularization.RecievingBankName = $("#TransferReceivingBankSelect option:selected").text();
                    paySummaryCardRegularization.RecievingBankAccountNumber = $("#AccountNumberReceivingBankSelect option:selected").text();
                    paySummaryCardRegularization.Date = $("#TransferDate").val();
                    paySummaryCardRegularization.IssuerName = $("#TransferHolderName").val();
                }
                break;

            case $("#ViewBagParamPaymentMethodDepositVoucher").val():

                $("#divDepositVoucher").validate();

                if ($("#divDepositVoucher").valid() || result == true && Numerrors == 0 && validBank == true) {

                    totAmount = totAmount + parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")) * exchangeRate);
                    totAmount = parseFloat(totAmount.toFixed(2));

                    if ($("#DepositVoucherCurrencySelect").val() == localCurrencyId) {
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")));
                        amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                        $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                        $("#TotalCurrencyCardRegularization").trigger('blur');
                    }

                    if ($("#DepositVoucherCurrencySelect").val() != localCurrencyId && $("#DepositVoucherCurrencySelect").val() != -1) {
                        amountForeingCurrency = amountForeingCurrency + parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")));
                        amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#DepositVoucherLocalAmount").val().replace("", ",")));
                        $("#TotalExchangeRateCardRegularization").val(amountForeingCurrency);
                        $("#TotalExchangeRateCardRegularization").trigger('blur');
                        $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                        $("#TotalCurrencyCardRegularization").trigger('blur');
                    }

                    if (parseFloat(ClearFormatCurrency($("#TotalCurrencyCardRegularization").val().replace("", ","))) <= parseFloat(ClearFormatCurrency($("#TotalAmountCardRegularization").val().replace("", ",")))) {
                        $("#DifferenceCardRegularization").val(parseFloat(ClearFormatCurrency($("#TotalAmountCardRegularization").val().replace("", ","))) - totAmount);
                    }

                    paySummaryCardRegularization.PaymentTypeId = $("#PaymentMethodType").val();
                    paySummaryCardRegularization.PaymentTypeDescription = $("#PaymentMethodType option:selected").text();
                    paySummaryCardRegularization.Amount = parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")));
                    paySummaryCardRegularization.Exchange = FormatCurrency(exchangeRate);
                    paySummaryCardRegularization.CurrencyId = $("#DepositVoucherCurrencySelect").val();
                    paySummaryCardRegularization.Currency = $("#DepositVoucherCurrencySelect option:selected").text();
                    paySummaryCardRegularization.LocalAmount = parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")) * exchangeRate);
                    paySummaryCardRegularization.DocumentNumber = $("#DepositVoucherDocumentNumberSelect").val();
                    paySummaryCardRegularization.RecievingBankId = $("#DepositVoucherBankReceivingSelect").val();
                    paySummaryCardRegularization.RecievingBankName = $("#DepositVoucherBankReceivingSelect option:selected").text();
                    paySummaryCardRegularization.RecievingBankAccountNumber = $("#DepositVoucherReceivingAccountNumberSelect option:selected").text();
                    paySummaryCardRegularization.Date = $("#DepositVoucherDate").val();
                    paySummaryCardRegularization.IssuerName = $("#DepositVoucherHolderName").val();
                }

                break;
                //recibo de retencion
            case $("#ViewBagParamPaymentMethodRetentionReceipt").val():

                $("#divRetentionReceipt").validate();

                if ($("#divRetentionReceipt").valid() || result == true && Numerrors == 0) {

                    totAmount = totAmount + parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")) * exchangeRate);
                    totAmount = parseFloat(totAmount.toFixed(2));

                    if ($("#RetentionReceiptCurrencySelet").val() == localCurrencyId) {
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")));
                        amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                        $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                        $("#TotalCurrencyCardRegularization").trigger('blur');
                    }

                    if ($("#RetentionReceiptCurrencySelet").val() != localCurrencyId && $("#RetentionReceiptCurrencySelet").val() != -1) {
                        amountForeingCurrency = amountForeingCurrency + parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")));
                        amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#RetentionReceiptLocalAmount").val().replace("", ",")));
                        $("#TotalExchangeRateCardRegularization").val(amountForeingCurrency);
                        $("#TotalExchangeRateCardRegularization").trigger('blur');
                        $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                        $("#TotalCurrencyCardRegularization").trigger('blur');
                    }

                    if (parseFloat(ClearFormatCurrency($("#TotalCurrencyCardRegularization").val().replace("", ","))) <= parseFloat(ClearFormatCurrency($("#TotalAmountCardRegularization").val().replace("", ",")))) {
                        $("#DifferenceCardRegularization").val(parseFloat(ClearFormatCurrency($("#TotalAmountCardRegularization").val().replace("", ","))) - totAmount);
                    }

                    paySummaryCardRegularization.PaymentTypeId = $("#PaymentMethodType").val();
                    paySummaryCardRegularization.PaymentTypeDescription = $("#PaymentMethodType option:selected").text();
                    paySummaryCardRegularization.Amount = parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")));
                    paySummaryCardRegularization.Exchange = FormatCurrency(exchangeRate);
                    paySummaryCardRegularization.CurrencyId = $("#RetentionReceiptCurrencySelet").val();
                    paySummaryCardRegularization.Currency = $("#RetentionReceiptCurrencySelet option:selected").text();
                    paySummaryCardRegularization.LocalAmount = parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")) * exchangeRate);
                    paySummaryCardRegularization.DocumentNumber = $("#RetentionReceiptDocumentNumber").val();
                    paySummaryCardRegularization.AuthorizationNumber = $("#RetentionReceiptAuthorizationNumber").val();
                    paySummaryCardRegularization.Date = $("#RetentionReceiptDate").val();
                    paySummaryCardRegularization.SerialVoucher = $("#RetentionReceiptSerialVoucher").val();
                    paySummaryCardRegularization.VoucherNumber = $("#RetentionReceiptVoucher").val();
                    paySummaryCardRegularization.SerialNumber = $("#RetentionReceiptSerialNumber").val();
                }

                break;
            default:
        }


        if (Numerrors == 0) {
            if (totAmount <= parseFloat(ClearFormatCurrency($("#TotalAmountCardRegularization").val().replace("", ",")))) {

                if (result == true && validBank == true) {

                    var valor = parseInt($('#rowid').val()) + 1;
                    $('#rowid').val(valor);

                    //Valida que no haya números de documento repetidos por c/ tipo de pago
                    if (Numerrors == 0) {
                        $("#tblPaySummary").UifDataTable('addRow', paySummaryCardRegularization);
                        $("#DifferenceCardRegularization").val(FormatCurrency($("#DifferenceCardRegularization").val()));
                    }

                } else {
                    if (result == false) {
                        $("#alert").UifAlert('show', Resources.AmountExceedsTotal, "warning");
                        Numerrors = 1;
                    }
                    if (validBank == false) {
                        Numerrors = 1;
                    }
                }


            } else {

                switch ($("#PaymentMethodType").val()) {
                    case $("#ViewBagParamPaymentMethodCash").val():
                        //Efectivo

                        totAmount = totAmount - parseFloat(ClearFormatCurrency($("#CashAmountRegularization").val().replace("", ",")) * exchangeRate);
                        totAmount = parseFloat(totAmount.toFixed(2));

                        if ($("#CashCurrencySelect").val() == localCurrencyId) {
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#CashAmountRegularization").val().replace("", ",")));
                            amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                            $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                            $("#TotalCurrencyCardRegularization").trigger('blur');
                        }

                        if ($("#CashCurrencySelect").val() != localCurrencyId && $("#CashCurrencySelect").val() != -1) {
                            amountForeingCurrency = amountForeingCurrency - parseFloat(ClearFormatCurrency($("#CashAmountRegularization").val().replace("", ",")));
                            amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#CashLocalAmount").val().replace("", ",")));
                            $("#TotalExchangeRateCardRegularization").val(amountForeingCurrency);
                            $("#TotalExchangeRateCardRegularization").trigger('blur');
                            $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                            $("#TotalCurrencyCardRegularization").trigger('blur');
                        }
                        break;
                    case $("#ViewBagParamPaymentMethodCurrentCheck").val():
                    case $("#ViewBagParamPaymentMethodDebit").val():
                    case $("#ViewBagParamPaymentMethodPostdatedCheck").val():
                        //Cheque y debito

                        totAmount = totAmount - parseFloat(ClearFormatCurrency($("#CheckAmount").val().replace("", ",")) * exchangeRate);
                        totAmount = parseFloat(totAmount.toFixed(2));

                        if ($("#CheckCurrencySelect").val() == localCurrencyId) {
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#CheckAmount").val().replace("", ",")));
                            amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                            $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                            $("#TotalCurrencyCardRegularization").trigger('blur');
                        }

                        if ($("#CheckCurrencySelect").val() != localCurrencyId && $("#CheckCurrencySelect").val() != -1) {
                            amountForeingCurrency = amountForeingCurrency - parseFloat(ClearFormatCurrency($("#CheckAmount").val().replace("", ",")));
                            amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#CheckLocalAmount").val().replace("", ",")));
                            $("#TotalExchangeRateCardRegularization").val(amountForeingCurrency);
                            $("#TotalExchangeRateCardRegularization").trigger('blur');
                            $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                            $("#TotalCurrencyCardRegularization").trigger('blur');
                        }
                        break;
                    case $("#ViewBagParamPaymentMethodDirectConection").val():
                    //case $("#ViewBagParamPaymentMethodTransfer").val():
                        //Tarjeta

                        totAmount = totAmount - parseFloat(ClearFormatCurrency($("#CreditCardAmount").val().replace("", ",")) * exchangeRate);
                        totAmount = parseFloat(totAmount.toFixed(2));

                        if ($("#CreditCardCurrencySelect").val() == localCurrencyId) {
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#CreditCardAmount").val().replace("", ",")));
                            amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                            $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                            $("#TotalCurrencyCardRegularization").trigger('blur');
                        }

                        if ($("#CreditCardCurrencySelect").val() != localCurrencyId && $("#CreditCardCurrencySelect").val() != -1) {
                            amountForeingCurrency = amountForeingCurrency - parseFloat(ClearFormatCurrency($("#CreditCardAmount").val().replace("", ",")));
                            amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#CreditCardLocalAmount").val().replace("", ",")));
                            $("#TotalExchangeRateCardRegularization").val(amountForeingCurrency);
                            $("#TotalExchangeRateCardRegularization").trigger('blur');
                            $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                            $("#TotalCurrencyCardRegularization").trigger('blur');
                        }
                        break;
                    //case $("#ViewBagParamPaymentMethodDirectConection").val():
                    case $("#ViewBagParamPaymentMethodTransfer").val():

                        totAmount = totAmount - parseFloat(ClearFormatCurrency($("#TransferAmount").val().replace("", ",")) * exchangeRate);
                        totAmount = parseFloat(totAmount.toFixed(2));

                        if ($("#TransferCurrencySelect").val() == localCurrencyId) {
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#TransferAmount").val().replace("", ",")));
                            amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                            $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                            $("#TotalCurrencyCardRegularization").trigger('blur');
                        }

                        if ($("#TransferCurrencySelect").val() != localCurrencyId && $("#TransferCurrencySelect").val() != -1) {
                            amountForeingCurrency = amountForeingCurrency - parseFloat(ClearFormatCurrency($("#TransferAmount").val().replace("", ",")));
                            amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#TransferLocalAmount").val().replace("", ",")));
                            $("#TotalExchangeRateCardRegularization").val(amountForeingCurrency);
                            $("#TotalExchangeRateCardRegularization").trigger('blur');
                            $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                            $("#TotalCurrencyCardRegularization").trigger('blur');
                        }

                        break;
                    case $("#ViewBagParamPaymentMethodDepositVoucher").val():
                        //boleta de deposito

                        totAmount = totAmount - parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")) * exchangeRate);
                        totAmount = parseFloat(totAmount.toFixed(2));

                        if ($("#DepositVoucherCurrencySelect").val() == localCurrencyId) {
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")));
                            amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                            $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                            $("#TotalCurrencyCardRegularization").trigger('blur');
                        }

                        if ($("#DepositVoucherCurrencySelect").val() != localCurrencyId && $("#DepositVoucherCurrencySelect").val() != -1) {
                            amountForeingCurrency = amountForeingCurrency - parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")));
                            amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#DepositVoucherLocalAmount").val().replace("", ",")));
                            $("#TotalExchangeRateCardRegularization").val(amountForeingCurrency);
                            $("#TotalExchangeRateCardRegularization").trigger('blur');
                            $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                            $("#TotalCurrencyCardRegularization").trigger('blur');
                        }

                        break;
                    case $("#ViewBagParamPaymentMethodRetentionReceipt").val():
                        //recibo de retencion

                        totAmount = totAmount - parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")) * exchangeRate);
                        totAmount = parseFloat(totAmount.toFixed(2));

                        if ($("#RetentionReceiptCurrencySelet").val() == localCurrencyId) {
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")));
                            amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                            $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                            $("#TotalCurrencyCardRegularization").trigger('blur');
                        }

                        if ($("#RetentionReceiptCurrencySelet").val() != localCurrencyId && $("#RetentionReceiptCurrencySelet").val() != -1) {
                            amountForeingCurrency = amountForeingCurrency - parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")));
                            amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#RetentionReceiptLocalAmount").val().replace("", ",")));
                            $("#TotalExchangeRateCardRegularization").val(amountForeingCurrency);
                            $("#TotalExchangeRateCardRegularization").trigger('blur');
                            $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
                            $("#TotalCurrencyCardRegularization").trigger('blur');
                        }

                        break;
                    default:
                }

                $("#alert").UifAlert('show', Resources.AmountExceedsTotal, 'warning');
                Numerrors = 1;
            }
        }
    } else {

        Numerrors = 1;
    }

    if (Numerrors == 0) {
        clearPaymentMethodFieldsCard();
    }
});


//BOTON ACEPTAR
$("#SaveBilling").click(function () {

    $("#frmCardVoucherGeneralInformation").validate();

    if ($("#frmCardVoucherGeneralInformation").valid() && (Numerrors == 0 || Numerrors == undefined)) {
        Alert = 0;
    } else {
        Alert = 1;
        Numerrors = 0;
    }

    if (Alert == 0)
        showConfirmCardRegularization();
});


//Elimina registro y resta su valor del total contabilizado
$('#tblPaySummary').on('rowDelete', function (event, selectedRow, position) {
    $('#tblPaySummary').UifDataTable('deleteRow', position);
    if (IsMainCheckRegularization) {
        deleteRowCheckRegularization(selectedRow);
    } else {

        deleteRowCardRegularization(selectedRow);
    }
        
});


//BOTONES DE MODALES
$(document).ready(function () {

    // Botón Aceptar - Cierre Caja
    $("#modalBillingClosure").find('#AcceptClosure').click(function () {

        $("#modalBillingClosure").hide();
          location.href =   $("#ViewBagBillingClosureIdLink").val() + "?billControlId=" + idBillControl + "&branchId=" + $("#SelectBranchCardRegularization").val();
    });

    // Botón Aceptar - Apertura Caja
    $("#modalOpeningBilling").find('#AcceptOpening').click(function () {
        $("#modalOpeningBilling").modal('hide');

        if ($("#SelectBranchCardRegularization").val() != undefined && $("#SelectBranchCardRegularization").val() != "") {
            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "Billing/SaveBillControl",
                data: {
                    "branchId": $("#SelectBranchCardRegularization").val(),
                    "accountingDate": $("#modalOpeningBilling").find("#AccountingDate").val()
                },
                success: function (data) {
                    idBillControl = data.result[0].Id;
                    isOpen = true;
                    $("#modalOpeningBilling").modal('hide');
                }
            });
        }
    });

    // Botón Imprimir Detalle Modal
    $("#modalSaveBillCardRegularization").find('#PrintDetail').click(function () {
        ShowReceiptReportCardRegularization(branchId, billIdResult,"");
        $('#modalSaveBillCardRegularization').UifModal('hide');
       
        location.href = $("#ViewBagInternalCardVoucher").val();
    });


    // Botón Cerrar Modal
    $("#modalSaveBillCardRegularization").find('#CancelSaveSuccess').click(function () {
        $('#modalSaveBillCardRegularization').UifModal('hide');
       
        location.href = $("#ViewBagInternalCardVoucher").val();
    });

});


/*-----------------------------------------------------------------------------------------------------------------------------------------*/
// FUNCIONES
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

function loadCardVoucherRegularization() {

    SourcePaymentId = $("#ViewBagPaymentId").val();
    VoucherNumber = $("#ViewBagVoucherNumber").val();

    if (SourcePaymentId > 0 && VoucherNumber > 0) {
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Regularization/GetRejectedCardVoucherInfoByPaymentId",
            data: { "paymentId": SourcePaymentId },
            success: function (data) {

                $("#CardVoucherType").val(data[0].CardDescription);
                $("#CardVoucherDocumentNumber").val(data[0].DocumentNumber);
                $("#CardVoucherNumber").val(data[0].VoucherRejectCard);
                $("#CardVoucherBillCode").val(data[0].BillCode);
                $("#CarVoucherDate").val(data[0].CardDate);
                $("#CarVoucherCurrency").val(data[0].CurrencyDescription);
                $("#CarVoucherAmount").val(FormatCurrency(FormatDecimal(data[0].AmountRejectCard)));
                $("#CarVoucherTax").val(FormatCurrency(FormatDecimal(data[0].Tax)));
                $("#CarVoucherHolder").val(data[0].Holder);
                $("#CarVoucherStatus").val(data[0].StatusDescription);
                $("#CarVoucherRejectedDate").val(data[0].RejectionDate);
                $("#CarVoucherRejectedNumber").val(data[0].RejectedPaymentCode);
                $("#CarVoucherRejectedMotive").val(data[0].RejectionDescription);

                $("#TotalAmountCardRegularization").val(FormatCurrency(FormatDecimal(data[0].AmountRejectCard)));
                $("#TotalAmountCardRegularization").trigger('blur');

                setTimeout(function () {
                    $("#SelectBranchCardRegularization").val($("#ViewBagBranchIdOrigin").val());
                    $("#SelectBranchCardRegularization").trigger('change');
                    $("#SelectBranchCardRegularization").attr('disabled', 'true');
                }, 3000);
            }
        });
    }
 
}


// Despliega la ventana para abrir caja
function OpenBillingDialogCardRegularization() {
    $('#modalOpeningBilling').find("#AccountingDate").val($("#ViewBagAccountingDate").val());
    $('#modalOpeningBilling').find("#UserId").val($("ViewBagUserNick").val());
    $('#modalOpeningBilling').UifModal('showLocal', Resources.OpeningBilling);
}

function fillPersonAutocompletes(selectedItem) {
    payerId = selectedItem.IndividualId;
    $("#DocumentNumber").val(selectedItem.DocumentNumber);
    $("#FirstLastName").val(selectedItem.Name);
}

function ClosureBillingDialog() {
    $('#modalBillingClosure').UifModal('showLocal', Resources.Closing);
}

function getDateCardRegularization() {
    if ($("#ViewBagImputationType").val() == undefined &&
        $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
        $("#ViewBagBillControlId") == undefined) {

        $.ajax({
            type: "GET",
            url: ACC_ROOT + "Billing/GetDate",
            success: function (data) {
                $("#billingDate").val(data);
                $("#BillingDate").val(data);
            }
        });
    }
}

function validPostDateDateCardRegularization() {

    var isDate = IsDate($("#CheckPostDate").val());
    var result = CompareDates($("#CheckPostDate").val(), getCurrentDate());

    if (isDate == true) {

        if (IsDate($("#CheckPostDate").val()) == true) {

            if (result == 1) {
                $("#CheckPostDate").val(getCurrentDate());
                showMsg(Resources.InvalidDates);
            } else if (result == 0) {
                $("#CheckPostDate").val(getCurrentDate());
                showMsg(Resources.InvalidDates);
            }
        }
    }
    else {

        $("#CheckPostDate").val(getCurrentDate());
        showMsg(Resources.InvalidDates);
    }
}

function showMsg(msg) {
    $("#alert").UifAlert('show', msg, 'warning');
}


function setCurrencyCardRegularization(ddlIn, txtOut) {
    var ddlCurrency = $("#" + ddlIn).val();
    var txtExchangeRate = $("#" + txtOut);

    //if (ddlCurrency != "") {
    if (ddlCurrency > 0) {
        var resp;
        resp = getCurrencyRateBillingCardRegularization($("#ViewBagDateAccounting").val(), ddlCurrency);
        if (resp[0] != "") {
            txtExchangeRate.val(resp[0]);
            txtExchangeRate.val(FormatCurrency(resp[0]));
            txtExchangeRate.blur();
        } else {
            txtExchangeRate.val("");
        }

        if (resp[1] == false) {
            $("#alert").UifAlert('show', Resources.ExchageRateNotUpToDate, 'warning');
        }

    } else {
        txtExchangeRate.val("");
    }
}

//Valida tasas de cambio
function setCurrencyNoMsgCardRegularization(ddlIn, txtOut) {
    var ddlCurrency = $("#" + ddlIn).val();
    var txtExchangeRate = $("#" + txtOut);

    if (ddlCurrency != "") {
        var resp;
        resp = getCurrencyRateBillingCardRegularization($("#ViewBagDateAccounting").val(), ddlCurrency);
        if (resp[0] != "") {
            txtExchangeRate.val(FormatCurrency(resp[0]));
        } else {
            txtExchangeRate.val("");
        }
    } else {
        txtExchangeRate.val("");
    }
}

//FUNCION OBTENER TASA DE CAMBIO
function getCurrencyRateBillingCardRegularization(accountingDate, currencyId) {

    var alert = true;
    var rate;
    var resp = new Array();

    if (currencyId != undefined) {


        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Billing/GetCurrencyExchangeRate",
            data: {
                "rateDate": accountingDate, "currencyId": currencyId
            },
            success: function (data) {
                if (data == 1 || data == 0) {
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: ACC_ROOT + "Billing/GetLatestCurrencyExchangeRate",
                        data: {
                            "rateDate": accountingDate, "currencyId": currencyId
                        },
                        success: function (dataRate) {
                            if (dataRate == 0 || dataRate == 1) {
                                rate = 1;
                                exchangeRate = rate;
                                alert = true;
                            } else {
                                rate = dataRate;
                                exchangeRate = rate;
                                alert = false;
                            }
                        }
                    });

                } else {
                    rate = data;
                    alert = true;
                }
            }
        });

        resp[0] = rate;
        resp[1] = alert;
        exchangeRate = rate;

    }

    return resp;
}


function validCreditCardVoucherNumberPayment() {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/ValidateVoucher",
        data: {
            "creditCardNumber": $("#PaymentCreditCardNumber").val(),
            "voucherNumber": $("#PaymentCreditCardVoucherNumber").val(),
            "conduitType": $("#CreditCardTypeSelect").val()
        },
        success: function (data) {
            if (data > 0) {
                Numerrors = 1;

                $("#alert").UifAlert('show', Resources.VoucherNumberAlreadyRegistered, 'warning');
            } else {
                Numerrors = 0;
            }
        }
    });
}

function setCurrencyComboCardRegularization(localCurrencyId) {
    $("#CashCurrencySelect").val(localCurrencyId);
}

//Elimina una fila del grid Resumen de Pagos
function deleteRowCardRegularization(selectedRow) {
    $("#alert").UifAlert('hide');
    if (selectedRow != null) {
        totAmount = totAmount - parseFloat($.trim(ClearFormatCurrency(selectedRow.LocalAmount)));

        $("#DifferenceCardRegularization").val(parseFloat($.trim(ClearFormatCurrency($("#TotalAmountCardRegularization").val()))) - totAmount);
        $("#DifferenceCardRegularization").val($.trim(FormatCurrency($("#DifferenceCardRegularization").val())));

        var currencyId = selectedRow.CurrencyId;
        if (currencyId == localCurrencyId) {
            amountLocalCurrency = amountLocalCurrency - parseFloat($.trim(ClearFormatCurrency(selectedRow.Amount)));
            $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
            $("#TotalCurrencyCardRegularization").trigger('blur');
        }
        if (currencyId != localCurrencyId) {
            amountForeingCurrency = amountForeingCurrency - parseFloat($.trim(ClearFormatCurrency(selectedRow.Amount)));
            amountLocalCurrency = amountLocalCurrency - parseFloat($.trim(ClearFormatCurrency(selectedRow.LocalAmount)));
            $("#TotalExchangeRateCardRegularization").val(amountForeingCurrency);
            $("#TotalExchangeRateCardRegularization").trigger('blur');
            $("#TotalCurrencyCardRegularization").val(amountLocalCurrency);
            $("#TotalCurrencyCardRegularization").trigger('blur');
        }

    } else {

        $("#alert").UifAlert('show', Resources.SelectedRecords, 'warning');
    }
}


// Visualiza el reporte de caja en pantalla
function ShowReceiptReportCardRegularization(branchId, billCode, otherPayerName) {

    var controller = ACC_ROOT + "Report/ShowReceiptReport?branchId=" + branchId + "&billCode=" +
                     billCode + "&reportId=3" + "&otherPayerName=" + otherPayerName;;
    window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}





function showConfirmCardRegularization() {

    $.UifDialog('confirm', { 'message': Resources.DialogExchangeCardConfirmationMessage + ' ?', 'title': Resources.RejectCard }, function (result) {
        //hace las validaciones
        if (ClearFormatCurrency($("#DifferenceCardRegularization").val().replace("", ",")) != 0.00) {
            $("#alert").UifAlert('show', Resources.RemainingDifference, 'warning');

        } else if ($("#SelectBranchCardRegularization").val() != "" && Alert == 0 && ClearFormatCurrency($("#DifferenceCardRegularization").val().replace("", ",")) == 0.00) {
            if (isOpen == true) {
                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "Regularization/SaveBillRequest",
                    data: JSON.stringify({
                        "frmBill": SetDataBillCardRegularization(),
                        "branchId": $("#SelectBranchCardRegularization").val()
                    }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {

                        $("#modalSaveBillCardRegularization").find("#ReceiptDescription").text(data.Description);
                        $("#modalSaveBillCardRegularization").find("#ReceiptTotalAmount").text($("#TotalAmountCardRegularization").val());
                        $("#modalSaveBillCardRegularization").find("#BillingId").text("00000" + data.Id);
                        $("#modalSaveBillCardRegularization").find("#TransactionNumber").text("00000" + data.TechnicalTransaction);
                        $("#modalSaveBillCardRegularization").find("#BillingDate").text($("#ViewBagDateAccounting").val());
                        $("#modalSaveBillCardRegularization").find("#ReceiptUser").text($("#ViewBagUserNick").val());

                        if (data.ShowMessage == "False") {
                            $("#modalSaveBillCardRegularization").find("#accountingLabelDiv").hide();
                            $("#modalSaveBillCardRegularization").find("#accountingMessageDiv").hide();
                        }
                        else {
                            $("#modalSaveBillCardRegularization").find("#accountingLabelDiv").show();
                            $("#modalSaveBillCardRegularization").find("#accountingMessageDiv").show();
                        }

                        $("#modalSaveBillCardRegularization").find("#accountingMessage").val(data.Message);

                        branchId = $("#SelectBranchCardRegularization").val();
                        //BillId = data.BillId; original ma
                        billIdResult = data.Id; //no se usa
                        //if (data.StatusId == 1) {
                        if (data.Status.Id == 1) {
                            $("#modalSaveBillCardRegularization").find("#ApplyBill").show();
                            //PayerDocumentNumber = $("#DocumentNumber").val();  //no se usa
                            //PayerName = $("#FirstLastName").val();  //no se usa
                            //BranchDescription = $("#SelectBranchCardRegularization option:selected").text();  //no se usa
                            //ApplyDescription = $("#Observation").val(); //no se usa
                        }
                        else {
                            $("#modalSaveBillCardRegularization").find("#ApplyBill").hide();
                        }
                        //se aplicó el recibo
                        //if (data.StatusId == 3)
                        if (data.Status.Id == 3) {
                            if (data.ShowImputationMessage == "False") {
                                $("#modalSaveBillCardRegularization").find("#applicationIntegration").hide();
                            }
                            else {
                                $("#modalSaveBillCardRegularization").find("#applicationIntegration").show();
                                $("#modalSaveBillCardRegularization").find("#accountingIntegrationMessage").val(data.ImputationMessage);
                            }
                        }
                        else {
                            $("#modalSaveBillCardRegularization").find("#applicationIntegration").hide();
                        }

                        $('#modalSaveBillCardRegularization').UifModal('showLocal', Resources.ReceiptSaveSuccess);

                        SetDataBillEmptyCardRegularization();
                        
                        branchId = $("#SelectBranchCardRegularization").val();
                        clearFieldsCardRegularization();
                        clearPaymentMethodFieldsCard();
                        SetDataBillEmptyCardRegularization();
                        
                    }
                });
                isOpen = false;
            }
            else {
                ClosureBillingDialog();  //Llama al cierre de caja
            }
        }
    });
}



function setDifferenceCardRegularization() {
    var sum = 0;
    var dataPayments = $("#tblPaySummary").UifDataTable("getData");

    if ($("#paymentsAmount").val() == '') {
        $("#_Difference").val($("#TotalAmountCardRegularization").val());
    } else if ($("#paymentsAmount").val() != '') {
        if (dataPayments != null) {
            for (i in dataPayments) {
                var rowData = dataPayments[i];
                sum = sum + parseFloat(ClearFormatCurrency(rowData.LocalAmount));
                sum = parseFloat(sum.toFixed(2));
            }
            $("#_Difference").val(parseFloat(ClearFormatCurrency($("#TotalAmountCardRegularization").val().replace("", ","))) - sum);
            $("#_Difference").trigger('blur');
        }
    }
}


function clearFieldsCardRegularization() {

    //parcial de Información General
    payerId = -1;
    $("#DocumentNumber").val('');
    $("#FirstLastName").val('');
    $("#SelectBranchCardRegularization").val('');
    $("#Description").val('');
    $("#ddlIncomeConcept").val(9);
    idBillConcept = 8;

    //parcial de cheque rechazado
    $("#searchIssuingBank").val('');
    $("#searchCheckNumber").val('');
    $("#checkDate").val('');
    $("#checkCurrency").val('');
    $("#checkAmount").val('');
    $("#checkIssuer").val('');
    $("#checkRejectedDate").val('');

    //parcial de resumen
    $("#rejectedCheckAmount").val('');
    $("#paymentsAmount").val('');
    $("#amountDifference").val('');

    //parcial de tipo de pago
    $("#PaymentMethodType").val($("#ViewBagParamPaymentMethodCash").val());
    $("#PaymentMethodType").trigger('change');
    $("#tblItemToPaySummary").dataTable().fnClearTable();

    //campos de pago con cheque
    $("#CheckAmount").val('');
    $("#CheckCurrencySelect").val(localCurrencyId);
    $("#DocumentNumber").val('');
    $("#CheckBank").val('');
    $("#CheckDateRegularizated").val('');

    //campos de pago con transferencia
    $("#TransferAmount").val('');
    $("#TransferCurrencySelect").val(localCurrencyId);
    $("#TransferIssuingAccountNumber").val('');
    $("#TransferIssuingBank").val('');
    $("#TransferDate").val('');
    $("#TransferDocumentNumber").val('');
    $("#TransferReceivingBankSelect").val('');

    IssuingBankIdCardRegularization = -1;
    TransferIssuingBankId = -1;
    TransferReceivingBankId = -1;
    totalAmount = 0;
    totAmount = 0;
    amountLocalCurrency = 0;

    //campos de pago con tarjeta de crédito
    $("#CreditCardAmount").val('');
    $("#CreditCardCurrencySelect").val(localCurrencyId);
    $("#VoucherNumber").val('');
    $("#CreditCardNumber").val('');
    $("#AuthorizationNumber").val('');

    //campos de pago en efectivo
    $("#CashAmountRegularization").val('');
    $("#CashCurrencySelect").val(localCurrencyId);

    //tabla de resumen
    $("#tblPaySummary").dataTable().fnClearTable();

    //limpia los mensajes de validación
    $("#frmCardVoucherGeneralInformation").validate().resetForm();
}

function GetUserNick() {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/GetUserNick",
        success: function (data) {
            $("#user").val(data);
            $("#UserId").val(data);
            $("#UserIdClosure").val(data);
            $("#ReceiptUser").val(data);
        }
    });
}

//Valida campos vacios en resumen de Pagos

function validFieldsCardRegularization(idPay) {
    var result = true;
    switch (idPay) {
        case "0":
            result = false;
            break;

        case $("#ViewBagParamPaymentMethodCash").val():
            //Efectivo
            $("#divCash").validate();

            if (!$("#divCash").valid() || $("#CashAmountRegularization").val() == "" ||
                 $("#CashAmountRegularization").val() == "$" || $("#CashCurrencySelect").val() == "-1") {
                result = false;
            }

            break;

        case $("#ViewBagParamPaymentMethodCurrentCheck").val():
        case $("#ViewBagParamPaymentMethodDebit").val():
        case $("#ViewBagParamPaymentMethodPostdatedCheck").val():
            //Cheque y debito
            $("#divCheck").validate();
            if ($("#divCheck").valid() && $("#PaymentMethodType").val() == "1") {

                if ($("#CheckAmount").val() == "" || $("#CheckBank").val() == "" ||
                    $("#CheckAccountNumber").val() == "" || $("#CheckDocumentNumber").val() == "" ||
                    $("#CheckHolderName").val() == "" || $("#CheckPostDate").val() == "") {
                    result = false;
                }
            }
            else if ($("#PaymentMethodType").val() == "2") {

                if ($("#CheckAmount").val() == "" || $("#CheckBank").val() == "" || $("#CheckAccountNumber").val() == "" ||
                    $("#CheckDocumentNumber").val() == "" || $("#CheckHolderName").val() == "" || $("#CheckDateRegularizated").val() == "") {
                    result = false;
                }
            }
            break;

        case $("#ViewBagParamPaymentMethodCreditableCreditCard").val():
        case $("#ViewBagParamPaymentMethodUncreditableCreditCard").val():
        case $("#ViewBagParamPaymentMethodDataphone").val():
            //Tarjeta

            if ($("#CreditCardAmount").val() == "" || $("#CreditCardTypeSelect").val() == "-1" ||
                $("#CreditCardIssuingBankInput").val() == "" || $("#CreditCardNumber").val() == "" ||
                $("#CreditCardAuthorizationNumber").val() == "" || $("#CreditCardVoucherNumber").val() == "" ||
                $("#CreditCardHolderName").val() == "" || $("#CreditCardValidThruYear").val() == "") {
                result = false;
            }

            break;

        case $("#ViewBagParamPaymentMethodDirectConection").val():
        case $("#ViewBagParamPaymentMethodTransfer").val():
            //Transferencia
            $("#divTransfer").validate();

            if (!$("#divTransfer").valid() || $("#TransferAmount").val() == "" ||
                $("#TransferIssuingAccountNumber").val() == "" || $("#ddlTransferIssuingBank").val() == "-1"
                || $("#TransferDate").val() == "" || $("#TransferReceivingBankSelect").val() == "-1" ||
                $("#TransferCurrencySelect").val() == "-1" || $("#ddlTransferReceivingAccountNumber").val() == "-1") {
                result = false;
            }

            break;
        case $("#ViewBagParamPaymentMethodDepositVoucher").val():                        //Boleta de Depósito
            //boleta de deposito

            if ($("#DepositVoucherAmount").val() == "" || $("#DepositVoucherBankReceivingSelect").val() == "-1" || $("#DepositVoucherReceivingAccountNumberSelect").val() == "-1"
                || $("#DepositVoucherDocumentNumberSelect").val() == "" || $("#DepositVoucherHolderName").val() == "" || $("#DepositVoucherDate").val() == "") {
                result = false;
            }

            break;
        case $("#ViewBagParamPaymentMethodRetentionReceipt").val():                        //recibo de retencion
            //recibo de retencion

            if ($("#RetentionReceiptAmount").val() == "" || $("#RetentionReceiptSerialNumber").val() == "" || $("#RetentionReceiptDocumentNumber").val() == ""
                || $("#RetentionReceiptAuthorizationNumber").val() == "" || $("#RetentionReceiptVoucher").val() == "" || $("#RetentionReceiptSerialVoucher").val() == "" || $("#RetentionReceiptDate").val() == "") {
                result = false;
            }

            break;
        default:
    }
    return result;
}

function validCheckDocumentNumberCardRegularization() {

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/ValidateCheckBankOrTransfer",
        data: { "bankId": IssuingBankIdCardRegularization, "numberDoc": $("#CheckDocumentNumber").val(), "accountNumber": $("#CheckAccountNumber").val() },
        success: function (data) {
            if (data > 0) {
                Numerrors = 1;
                $("#alert").UifAlert('show', Resources.RegisteredCheckNum, 'warning');

            } else {
                Numerrors = 0;
            }
        }
    });
}

function validCreditCardVoucherNumberCardRegularization() {


    if ($("#CreditCardTypeSelect").val() != undefined) {
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Billing/ValidateVoucher",
            data: {
                "creditCardNumber": $("#CreditCardNumber").val(), "voucherNumber": $("#CreditCardVoucherNumber").val()

            },
            success: function (data) {
                if (data > 0) {

                    Numerrors = 1;
                    $("#alert").UifAlert('show', Resources.RegisteredCheckNum, 'warning');

                } else {
                    Numerrors = 0;
                }
            }
        });
    }
}

function validatePostDatedCheckCardRegularization() {

    var postDatedChecks = 0;
    var otherPayments = 0;

    var paymentSummary = $("#tblPaySummary").UifDataTable("getData");
    var paymentsSummaryRow;

    if (paymentSummary.length > 0) {
        for (var j in paymentSummary) {
            paymentsSummaryRow = paymentSummary[j];
            if (paymentsSummaryRow.PaymentTypeId == $("#ViewBagParamPaymentMethodPostdatedCheck").val()) {
                postDatedChecks = postDatedChecks + 1;
            } else {
                otherPayments = otherPayments + 1;
            }
        }
        if (postDatedChecks > 0) {
            Numerrors = 1;
            $("#alert").UifAlert('show', Resources.ValidationPostdatedCheck, 'warning');
        }
        if (otherPayments > 0 && $("#PaymentMethodType").val() == $("#ViewBagParamPaymentMethodPostdatedCheck").val()) {
            Numerrors = 1;
            $("#alert").UifAlert('show', Resources.ValidationPostdatedCheck, 'warning');
        }
    }
    if (paymentSummary.length == 0) {
        Numerrors = 0;
    }
}


function checkExpirationDateCardRegularization() {
    var date;

    $.ajax({
        type: "GET",
        url: ACC_ROOT + "Billing/GetDate",
        async: false,
        success: function (data) {
            date = data;
        }
    });

    if ($("#CreditCardValidThruYear").val() < year) {

        $("#alert").UifAlert('show', Resources.ExpiredCreditCard, 'warning');

        Numerrors = 1;
    } else if ($("#CreditCardValidThruYear").val() == year && $("#CreditCardValidThruMonth").val() < month) {

        $("#alert").UifAlert('show', Resources.ExpiredCreditCard, 'warning');

        Numerrors = 1;
    } else if ($("#CreditCardValidThruYear").val() == year && $("#CreditCardValidThruMonth").val() == month && day >= 1) {

        $("#alert").UifAlert('show', Resources.ExpiredCreditCard, 'warning');

        Numerrors = 1;
    } else {
        Numerrors = 0;
    }

}

function validTransferDocumentNumberCardRegularization() {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/ValidateCheckBankOrTransfer",
        data: {
            "bankId": TransferIssuingBankId, "numberDoc": $("#TransferDocumentNumber").val(),
            "accountNumber": $("#TransferIssuingAccountNumber").val()
        },
        success: function (data) {
            if (data > 0) {
                Numerrors = 1;

                $("#alert").UifAlert('show', Resources.BillTransferValid, 'warning');

            } else {
                Numerrors = 0;
            }
        }
    });
}

function validDepositVoucherDocumentNumberCardRegularization() {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/ValidateDepositVoucher",
        data: {
            "bankId": $("#DepositVoucherBankReceiving").val(),
            "numberDoc": $("#DepositVoucherDocumentNumber").val(),
            "accountNumber": $("#DepositVoucherReceivingAccountNumber option:selected").text()
        },
        success: function (data) {
            if (data > 0) {
                Numerrors = 1;

                $("#alert").UifAlert('show', Resources.BillDepositVoucherValid, 'warning');

            } else {
                Numerrors = 0;
            }
        }
    });
}

//Control para que no se agregue un medio de pago ya seleccionado
function validarGridPaySummary(keyValid) {

    var exists = 0;
    var pays = $("#tblPaySummary").UifDataTable("getData");

    if (pays.length == 0) {
        return true;
    } else {
        for (var x in pays) {

            var rowData = pays[x];
            var keyPay;

            switch (rowData.TipoPagoId) {
                //Efectivo
                case $("#ViewBagParamPaymentMethodCash").val():
                    keyPay = rowData.PaymentTypeId + rowData.CurrencyId;
                    break;

                    //Cheque
                case $("#ViewBagParamPaymentMethodCurrentCheck").val():
                case $("#ViewBagParamPaymentMethodDebit").val():
                case $("#ViewBagParamPaymentMethodPostdatedCheck").val():
                    keyPay = rowData.DocumentNumber + rowData.IssuingBankId + rowData.IssuingBankAccountNumber;
                    break;

                    //Tarjeta
                case $("#ViewBagParamPaymentMethodCreditableCreditCard").val():
                case $("#ViewBagParamPaymentMethodUncreditableCreditCard").val():
                case $("#ViewBagParamPaymentMethodDataphone").val():

                    keyPay = rowData.CardNumber + rowData.VoucherNumber;
                    break;

                    //Transferencia
                case $("#ViewBagParamPaymentMethodDirectConection").val():
                case $("#ViewBagParamPaymentMethodTransfer").val():
                    keyPay = rowData.IssuingBankAccountNumber + rowData.IssuingBankId + rowData.DocumentNumber;
                    break;

                    //Boleta de deposito
                case $("#ViewBagParamPaymentMethodDepositVoucher").val():                        //Boleta de Depósito
                    keyPay = rowData.RecievingBankName + rowData.DocumentNumber + rowData.RecievingBankAccountNumber;
                    break;

                    //recibo de retencion
                case $("#ViewBagParamPaymentMethodRetentionReceipt").val():                        //recibo de retencion
                    keyPay = rowData.PaymentTypeId + rowData.CurrencyId + rowData.DocumentNumber;
                    break;

                default:
            }

            if (keyValid == keyPay) {
                exists++;
            }
        }
        if (exists > 0) {
            return false;
        } else {
            return true;
        }
    }
}

function clearPaymentMethodFieldsCard() {

    //campos de pago con cheque
    $("#CheckAmount").val('');
    $("#CheckCurrency").val(localCurrencyId);
    $("#CheckDocumentNumber").val('');
    $("#CheckBank").val('');
    $("#CheckDateRegularizated").val('');
    $("#CheckPostDate").val('');
    $("#CheckAccountNumber").val('');
    $("#CheckHolderName").val('');
    $("#CheckExchangeRate").val('');
    $("#CheckLocalAmount").val('');

    //campos de pago con transferencia
    $("#TransferAmount").val('');
    $("#TransferCurrency").val(localCurrencyId);
    $("#TransferDocumentNumber").val('');
    $("#TransferIssuingBank").val('');
    $("#TransferIssuingAccountNumber").val('');
    $("#TransferDate").val('');
    $("#TransferReceivingBank").val('');
    $("#TransferReceivingAccountNumber").val("");
    $("#TransferHolderName").val('');
    $("#TransferExchangeRate").val('');
    $("#TransferLocalAmount").val('');
    $("#TransferReceivingBankSelect").val('');
    $("#AccountNumberReceivingBankSelect").val('');

    IssuingBankIdCardRegularization = -1;
    //ReceivingBankId = -1;

    //campos de pago con tarjeta de crédito
    $("#CreditCardAmount").val('');
    $("#CreditCardCurrency").val(localCurrencyId);
    $("#CreditCardVoucherNumber").val('');
    $("#CreditCardType").val(-1);
    $("#CreditCardNumber").val('');
    $("#CreditCardAuthorizationNumber").val('');
    $("#CreditCardType").val('');
    $("#CreditCardIssuingBank").val('');
    $("#CreditCardHolderName").val('');
    $("#CreditCardExchangeRate").val('');
    $("#CreditCardLocalAmount").val('');
    $("#CreditCardValidThruMonth").val("");
    $("#CreditCardValidThruYear").val('');
    $("#tax").val('');
    $("#taxBase").val('');
    CreditCardIssuingBankId = -1;

    //campos de pago en efectivo
    $("#CashAmountRegularization").val('');
    $("#CashCurrencySelect").val(localCurrencyId);
    $("#CashExchangeRate").val('');
    $("#CashLocalAmount").val('');

    //campos de boleta
    $("#DepositVoucherCurrency").val(localCurrencyId);
    $("#DepositVoucherAmount").val('');
    $("#DepositVoucherReceivingBank").val('');
    $("#DepositVoucherReceivingAccountNumber").val('');
    $("#DepositVoucherDocumentNumber").val('');
    $("#DepositVoucherDate").val('');
    $("#DepositVoucherHolderName").val('');
    $("#DepositVoucherExchangeRate").val('');
    $("#DepositVoucherLocalAmount").val('');
    DepositVoucherReceivingBankId = -1;

    //campos de retencion
    $("#RetentionReceiptCurrency").val(localCurrencyId);
    $("#RetentionReceiptSerialNumber").val('');
    $("#RetentionReceiptDocumentNumber").val('');
    $("#RetentionReceiptAuthorizationNumber").val('');
    $("#RetentionReceiptVoucher").val('');
    $("#RetentionReceiptSerialVoucher").val('');
    $("#RetentionReceiptDate").val('');
    $("#RetentionReceiptAmount").val('');
    $("#RetentionReceiptExchangeRate").val('');
    $("#RetentionReceiptLocalAmount").val('');

    //variable de cambio de moneda
    exchangeRate = 1;
}

function SetDataBillEmptyCardRegularization() {

    paySummaryCardRegularization = {
        PaymentTypeId: 0,
        PaymentTypeDescription: "",
        Amount: 0,
        Exchange: 0,
        CurrencyId: 0,
        Currency: null,
        LocalAmount: 0,
        DocumentNumber: 0,
        VoucherNumber: 0,
        CardNumber: 0,
        CardType: 0,
        CardTypeName: null,
        AuthorizationNumber: 0,
        CardsName: null,
        ValidThruMonth: 0,
        ValidThruYear: 0,
        IssuingBankId: -1,
        IssuinBankName: "",
        IssuingBankAccountNumber: "",
        IssuerName: "",
        RecievingBankId: -1,
        RecievingBankName: "",
        RecievingBankAccountNumber: "",
        SerialVoucher: null,
        SerialNumber: null,
        Date: "",
        Tax: 0,
        TaxBase: 0
    };

    oBillModel = {
        BillId: 0,
        BillingConceptId: 0,
        BillControlId: 0,
        RegisterDate: null,
        Description: null,
        PaymentsTotal: 0,
        PayerId: -1,
        PaymentSummary: [],
        PayerName:""
    };

    oPaymentSummaryModel = {
        PaymentId: 0,
        BillId: 0,
        PaymentMethodId: 0,
        Amount: 0,
        CurrencyId: 0,
        ExchangeRate: 0,
        CheckPayments: [],
        CreditPayments: [],
        TransferPayments: []
    };

    oCheckModel = {
        DocumentNumber: null,
        IssuingBankId: -1,
        Date: null,
        Amount: 0
    };

    oCreditModel = {
        CardNumber: null,
        Voucher: null,
        AuthorizationNumber: null,
        CreditCardTypeId: 0,
        IssuingBankId: -1,
        Holder: null,
        ValidThruYear: 0,
        ValidThruMonth: 0,
        TaxBase: 0
    };

    oTransferModel = {
        DocumentNumber: null,
        IssuingBankId: -1,
        ReceivingBankId: -1,
        Date: null,
        Amount: 0
    };

    oDepositVoucherModel = {
        VoucherNumber: null,
        ReceivingAccountBankId: -1,
        ReceivingAccountNumber: null,
        Date: null,
        DepositorName: null
    };

    oRetentionReceiptModel = {
        BillNumber: null,
        AuthorizationNumber: null,
        SerialNumber: null,
        VoucherNumber: null,
        SerialVoucherNumber: null,
        Date: null
    };
}

function ClosureBilling() {
    $.ajax({
        url: ACC_ROOT + "Billing/NeedCloseBill",
        data: { "branchId": $("#SelectBranchCardRegularization").val(), "accountingDatePresent": $("#ViewBagDateAccounting").val() },
        success: function (userData) {
            if (userData[0].resp == true) {
                ClosureBillingDialog();
            } else {
                $.ajax({
                    url: ACC_ROOT + "Billing/AllowOpenBill",
                    data: { "branchId": $("#SelectBranchCardRegularization").val(), "accountingDate": $("#ViewBagDateAccounting").val() },
                    success: function (userata) {
                        if (userata[0].resp == true) {
                            OpenBillingDialogCardRegularization();
                        } else {
                            isOpen = true;
                        }
                    }
                });
            }
            idBillControl = userData[0].Id;
        }
    });
}

function SetDataBillCardRegularization() {

    var paymentsTotal = 0;

    oBillModel.BillId = 0;
    oBillModel.BillingConceptId = idBillConcept;
    oBillModel.BillControlId = idBillControl;
    oBillModel.RegisterDate = $("#BillingDate").val();
    oBillModel.Description = Resources.CardRegularizationDescription + " " + VoucherNumber;
    paymentsTotal = ClearFormatCurrency($("#TotalAmountCardRegularization").val().replace("", ","));
    oBillModel.PaymentsTotal = paymentsTotal.replace(",", ".");

    oBillModel.PayerId = payerId;
    oBillModel.PayerName = "";
    oBillModel.SourcePaymentId = SourcePaymentId;

    //llenar pagos
    var paymentsSummary = $("#tblPaySummary").UifDataTable("getData");
    var paymentsSummaryRow;

    for (var j in paymentsSummary) {
        paymentsSummaryRow = paymentsSummary[j];
        oPaymentSummaryModel = {
            PaymentId: 0,
            BillId: 0,
            PaymentMethodId: 0,
            Amount: 0,
            CurrencyId: 0,
            LocalAmount: 0,
            ExchangeRate: 0,
            CheckPayments: [],
            CreditPayments: [],
            TransferPayments: [],
            DepositVouchers: [],
            RetentionReceipts: []
        };

        oPaymentSummaryModel.PaymentId = 0;
        //se lo asigna a nivel de controlador
        oPaymentSummaryModel.BillId = 0;
        oPaymentSummaryModel.PaymentMethodId = paymentsSummaryRow.PaymentTypeId;
        oPaymentSummaryModel.Amount = ClearFormatCurrency(paymentsSummaryRow.Amount).replace(",", ".");
        oPaymentSummaryModel.CurrencyId = paymentsSummaryRow.CurrencyId;
        oPaymentSummaryModel.LocalAmount = ClearFormatCurrency(paymentsSummaryRow.LocalAmount).replace(",", ".");
        oPaymentSummaryModel.ExchangeRate = ClearFormatCurrency(paymentsSummaryRow.Exchange).replace(",", ".");

        //llena los datos de los metodos de pago

        //cheque
        if (paymentsSummaryRow.PaymentTypeId == $("#ViewBagParamPaymentMethodCurrentCheck").val() ||
            paymentsSummaryRow.PaymentTypeId == $("#ViewBagParamPaymentMethodPostdatedCheck").val() ||
            paymentsSummaryRow.PaymentTypeId == $("#ViewBagParamPaymentMethodDebit").val()) {
            oCheckModel = {
                DocumentNumber: null,
                IssuingBankId: -1,
                IssuingAccountNumber: null,
                IssuerName: null,
                Date: null
            };

            oCheckModel.DocumentNumber = paymentsSummaryRow.DocumentNumber;
            oCheckModel.IssuingBankId = paymentsSummaryRow.IssuingBankId;
            oCheckModel.IssuingAccountNumber = paymentsSummaryRow.IssuingBankAccountNumber;
            oCheckModel.IssuerName = paymentsSummaryRow.IssuerName;
            oCheckModel.Date = paymentsSummaryRow.Date;

            oPaymentSummaryModel.CheckPayments.push(oCheckModel);
        }

        //tarjeta de crédito
        if (paymentsSummaryRow.PaymentTypeId == $("#ViewBagParamPaymentMethodCreditableCreditCard").val() ||
            paymentsSummaryRow.PaymentTypeId == $("#ViewBagParamPaymentMethodUncreditableCreditCard").val() ||
            paymentsSummaryRow.PaymentTypeId == $("#ViewBagParamPaymentMethodDataphone").val()) {
            oCreditModel = {
                CardNumber: null,
                Voucher: null,
                AuthorizationNumber: null,
                CreditCardTypeId: 0,
                IssuingBankId: -1,
                Holder: null,
                ValidThruYear: 0,
                ValidThruMonth: 0,
                TaxBase: 0
            };

            oCreditModel.CardNumber = paymentsSummaryRow.CardNumber;
            oCreditModel.Voucher = paymentsSummaryRow.VoucherNumber;
            oCreditModel.AuthorizationNumber = paymentsSummaryRow.AuthorizationNumber;
            oCreditModel.CreditCardTypeId = paymentsSummaryRow.CardType;
            oCreditModel.IssuingBankId = paymentsSummaryRow.IssuingBankId;
            oCreditModel.Holder = paymentsSummaryRow.CardsName;
            oCreditModel.ValidThruYear = paymentsSummaryRow.ValidThruYear;
            oCreditModel.ValidThruMonth = paymentsSummaryRow.ValidThruMonth;
            oCreditModel.TaxBase = paymentsSummaryRow.TaxBase.replace(",", ".");

            oPaymentSummaryModel.CreditPayments.push(oCreditModel);
        }

        //transferencia y coneccion directa
        if (paymentsSummaryRow.PaymentTypeId == $("#ViewBagParamPaymentMethodDirectConection").val() ||
            paymentsSummaryRow.PaymentTypeId == $("#ViewBagParamPaymentMethodTransfer").val()) {
            oTransferModel = {
                DocumentNumber: null,
                IssuingBankId: -1,
                IssuingAccountNumber: null,
                IssuerName: null,
                ReceivingBankId: -1,
                ReceivingAccountNumber: null,
                Date: null
            };

            oTransferModel.DocumentNumber = paymentsSummaryRow.DocumentNumber;
            oTransferModel.IssuingBankId = paymentsSummaryRow.IssuingBankId;
            oTransferModel.IssuingAccountNumber = paymentsSummaryRow.IssuingBankAccountNumber;
            oTransferModel.IssuerName = paymentsSummaryRow.IssuerName;
            oTransferModel.ReceivingBankId = paymentsSummaryRow.RecievingBankId;
            oTransferModel.ReceivingAccountNumber = paymentsSummaryRow.RecievingBankAccountNumber;
            oTransferModel.Date = paymentsSummaryRow.Date;

            oPaymentSummaryModel.TransferPayments.push(oTransferModel);

        }

        //boleta de depósito
        if (paymentsSummaryRow.PaymentTypeId == $("#ViewBagParamPaymentMethodDepositVoucher").val()) {
            oDepositVoucherModel = {
                VoucherNumber: null,
                ReceivingAccountBankId: -1,
                ReceivingAccountNumber: null,
                Date: null,
                DepositorName: null
            };

            oDepositVoucherModel.VoucherNumber = paymentsSummaryRow.DocumentNumber;
            oDepositVoucherModel.ReceivingAccountBankId = paymentsSummaryRow.RecievingBankId;
            oDepositVoucherModel.ReceivingAccountNumber = paymentsSummaryRow.RecievingBankAccountNumber;
            oDepositVoucherModel.Date = paymentsSummaryRow.Date;
            oDepositVoucherModel.DepositorName = paymentsSummaryRow.IssuerName;

            oPaymentSummaryModel.DepositVouchers.push(oDepositVoucherModel);
        }

        //Recibo de Retención
        if (paymentsSummaryRow.PaymentTypeId == $("ViewBagParamPaymentMethodRetentionReceipt").val()) {
            oRetentionReceiptModel = {
                BillNumber: null,
                AuthorizationNumber: null,
                SerialNumber: null,
                VoucherNumber: null,
                SerialVoucherNumber: null,
                Date: null
            };

            oRetentionReceiptModel.BillNumber = paymentsSummaryRow.DocumentNumber;
            oRetentionReceiptModel.AuthorizationNumber = paymentsSummaryRow.AuthorizationNumber;
            oRetentionReceiptModel.SerialNumber = paymentsSummaryRow.SerialNumber;
            oRetentionReceiptModel.VoucherNumber = paymentsSummaryRow.VoucherNumber;
            oRetentionReceiptModel.SerialVoucherNumber = paymentsSummaryRow.SerialVoucher;
            oRetentionReceiptModel.Date = paymentsSummaryRow.Date;

            oPaymentSummaryModel.RetentionReceipts.push(oRetentionReceiptModel);
        }

        oBillModel.PaymentSummary.push(oPaymentSummaryModel);
    }
    return oBillModel;
}

function GetAccountingDate() {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/GetAccountingDate",
        success: function (data) {
            $("#billingDate").val(data);
            $("#BillingDate").val(data);
            $("#modalSaveBillCardRegularization").find("#BillingDate").val(data);
        }
    });
}

//valida que el BaseIva no sea mayor al importe
function validateTaxBase() {
    if (parseFloat(ClearFormatCurrency($("#_taxBase").val().replace("", ","))) > parseFloat(ClearFormatCurrency($("#CreditCardAmount").val().replace("", ",")))) {

        $("#alert").UifAlert('show', Resources.WarningGreaterTaxBase, 'warning');
        Numerrors = 1;
    } else {
        Numerrors = 0;
    }
}

//valida que el id de banco se haya ingresado
function validateBankIdCardRegularization() {
    var result = true;
    switch ($("#PaymentMethodType").val()) {
        case $("#ViewBagParamPaymentMethodCurrentCheck").val():
        case $("#ViewBagParamPaymentMethodDebit").val():
        case $("#ViewBagParamPaymentMethodPostdatedCheck").val():
            if (IssuingBankIdCardRegularization == -1) {
                result = false;
            } else
                result = true;

            break;
        case $("#ViewBagParamPaymentMethodCreditableCreditCard").val():
        case $("#ViewBagParamPaymentMethodUncreditableCreditCard").val():
        case $("#ViewBagParamPaymentMethodDataphone").val():
            //Tarjeta

            if (CreditCardIssuingBankId == -1) {
                result = false;
            } else
                result = true;

            break;
        case $("#ViewBagParamPaymentMethodDirectConection").val():
        case $("#ViewBag.ParamPaymentMethodTransfer").val():
            //Transferencia
            if (TransferIssuingBankId == -1 || $("#TransferReceivingBankSelect").val() == -1) {
                result = false;
            } else
                result = true;

            break;
        case $("#ViewBagParamPaymentMethodDepositVoucher").val():
            //Boleta de deposito

            if ($("#DepositVoucherBankReceivingSelect").val() == -1) {
                result = false;
            } else
                result = true;

            break;
        default:
    }
    return result;
}


