/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
var BillId = 0;
var paymentMethod = "";
var Numerrors = 0;
var IssuingBankId = -1;
var RejectedPaymentId = 0;
var SourcePaymentId = 0;
var Alert = 0;
var CheckDocumentNumber = 0;
var IssuingAccountNumber = 0;
var idBillControl = 0;
var isOpen = false;
var localCurrencyId = $("#ViewBagLocalCurrencyId").val();
var TransferIssuingBankId = -1;
var TransferReceivingBankId = -1;
var totalAmount = 0;
var totAmount = 0;
var amountLocalCurrency = 0;
var amountForeingCurrency = 0;
var payerId = -1;
var exchangeRate = 1;
var CreditCardIssuingBankId = -1;
var DepositVoucherReceivingBankId = -1;
var params = {};
var result = true;
var validBank = true;
//flag para el método deleteRow de la grilla de resumen de tipo de pago.
var IsMainCheckRegularization = true;
var IsMainBilling = false;
var CardTax = 0;
var tempImputationId = 0;
var amount = 0;
var branchId = 0;
var msgResult = "";
var idBillConcept = 0;

var paySummary = {
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
    PaymentSummary: []
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




//setTimeout(function () {

//    //DropDown
//    var controller = ACC_ROOT + "Common/GetAccountByBankIdSelect?bankId=" + 0;
//    $("#AccountNumberReceivingBankSelect").UifSelect({ source: controller });

//    $("#PaymentMethodType").val($("#ViewBagParamPaymentMethodCash").val());
//    $("#PaymentMethodType").trigger('change');

//    if ($("#ViewBagCheckRegularization").val() == "true") {
//        $("#divCash").show();

//        //Muestra para cheques
//        $("#_accept").show();
//        $("#SelectBranchCheckRegularization").show();

//        //Muestra para tarjetas
//        $("#AcceptCardRegularization").hide();
//        $("#SelectBranchCardRegularization").hide();
//    }
//    //CARGA DATA 
//    loadCheckRegularization();
//    getDateCheckRegularization();


//}, 2000);



//Inicializacion de campos al cargar la página
$("#CashExchangeRate").val("");
$('#CheckExchangeRate').val("");
$('#TransferExchangeRate').val("");
$('#CreditCardExchangeRate').val("");
$('#DepositVoucherExchangeRate').val("");
$('#RetentionReceiptExchangeRate').val("");
$('#TransferExchangeRateArea').val("");
$('#PaymentCreditCardExchangeRate').val("");


//Autocomplete
$('#DocumentNumber').on('itemSelected', function (event, selectedItem) {
    fillPersonAutocompletes(selectedItem);
});

//Autocomplete Nombres
$('#FirstLastName').on('itemSelected', function (event, selectedItem) {
    fillPersonAutocompletes(selectedItem);
});


// llena autocompletes de personas
function fillPersonAutocompletes(selectedItem) {
    payerId = selectedItem.IndividualId;
    $('#DocumentNumber').UifAutoComplete('setValue', selectedItem.DocumentNumber);
    $("#FirstLastName").UifAutoComplete('setValue', selectedItem.Name);
}


// Despliega la ventana para abrir caja
function OpenBillingDialog() {
    $('#modalOpeningBilling').find("#AccountingDate").val($("#ViewBagAccountingDate").val());
    $('#modalOpeningBilling').find("#UserId").val($("#ViewBagUserNick").val());
    $('#modalOpeningBilling').UifModal('showLocal', Resources.OpeningBilling);
}

//Obtiene la fecha del servidor
function getDateCheckRegularization() {
    $.ajax({
        type: "GET",
        url: ACC_ROOT + "Billing/GetDate",
        success: function (data) {
            $("#_billingDate").val(data);
        }
           });
}



/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES GLOBALES                                                                       */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/

function clearPaymentMethodFields() {

    //campos de pago con cheque
    $("#CheckAmount").val('');
    $("#CheckCurrencySelect").val(localCurrencyId);
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
    $("#TransferCurrencySelect").val(localCurrencyId);
    $("#TransferIssuingAccountNumber").val('');
    $("#TransferIssuingBank").val('');
    $("#TransferDocumentNumber").val('');
    $("#TransferDate").val('');
    $("#TransferReceivingBank").val('');
    $("#ddlTransferReceivingAccountNumber").val('');
    $("#TransferHolderName").val('');
    $("#TransferExchangeRate").val('');
    $("#TransferLocalAmount").val('');
    IssuingBankId = -1;
    var ReceivingBankId = -1;

    //campos de pago con tarjeta de crédito
    $("#CreditCardAmount").val('');
    $("#CreditCardCurrencySelect").val(localCurrencyId);
    $("#CreditCardVoucherNumber").val('');
    $("#CreditCardTypeSelect").val('');
    $("#CreditCardNumber").val('');
    $("#CreditCardAuthorizationNumber").val('');
    $("#_CreditCardType").val('');
    $("#CreditCardIssuingBankInput").val('');
    $("#CreditCardHolderName").val('');
    $("#CreditCardExchangeRate").val('');
    $("#CreditCardLocalAmount").val('');
    $("#CreditCardValidThruMonthSelect").val(1);
    $("#CreditCardValidThruYear").val('');
    $("#tax").val('');
    $("#CreditCardTaxBase").val('');
    CreditCardIssuingBankId = -1;

    //campos de pago en efectivo
    $("#CashAmountRegularization").val('');
    $("#CashCurrencySelect").val(localCurrencyId);
    $("#CashExchangeRate").val('');
    $("#CashLocalAmount").val('');

    //campos de boleta
    $("#DepositVoucherCurrencySelect").val(localCurrencyId);
    $("#DepositVoucherAmount").val('');
    $("#DepositVoucherReceivingBank").val('');
    $("#_DepositVoucherReceivingAccountNumber").val('');
    $("#DepositVoucherDocumentNumberSelect").val('');
    $("#DepositVoucherDate").val('');
    $("#DepositVoucherHolderName").val('');
    $("#DepositVoucherExchangeRate").val('');
    $("#DepositVoucherLocalAmount").val('');
    DepositVoucherReceivingBankId = -1;

    //campos de retencion
    $("#RetentionReceiptCurrencySelet").val(localCurrencyId);
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



function validCheckDocumentNumberCheckRegularization() {

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/ValidateCheckBankOrTransfer",
        data: { "bankId": IssuingBankId, "numberDoc": $("#CheckDocumentNumber").val(), "accountNumber": $("#CheckAccountNumber").val() },
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

function validCreditCardVoucherNumber() {

    if ($("#CreditCardTypeSelect").val() != undefined) {
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Billing/ValidateVoucher",
            data: {
                "creditCardNumber": $("#CreditCardNumber").val(),
                "voucherNumber": $("#CreditCardVoucherNumber").val(),
                "conduitType": $("#CreditCardTypeSelect").val()
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

function validatePostDatedCheck() {
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

function checkExpirationDate() {
    var date;

    $.ajax({
        type: "GET",
        url: ACC_ROOT + "Billing/GetDate",
        async: false,
        success: function (data) {
            date = data;
        }
    });

    var partDate = date.split("/");

    var day = parseInt(partDate[0], 10);
    var month = parseInt(partDate[1], 10);
    var year = partDate[2];

    if ($("#CreditCardValidThruYear").val() < year) {
        $("#alert").UifAlert('show', Resources.ExpiredCreditCard, 'warning');
        Numerrors = 1;
    } else if ($("#CreditCardValidThruYear").val() == year && $("#CreditCardValidThruMonthSelect").val() < month) {
        $("#alert").UifAlert('show', Resources.ExpiredCreditCard, 'warning');
        Numerrors = 1;
    } else if ($("#CreditCardValidThruYear").val() == year && $("#CreditCardValidThruMonthSelect").val() == month && day >= 1) {
        $("#alert").UifAlert('show', Resources.ExpiredCreditCard, 'warning');
        Numerrors = 1;
    } else {
        Numerrors = 0;
    }
}

function validTransferDocumentNumber() {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/ValidateCheckBankOrTransfer",
        data: { "bankId": TransferIssuingBankId, "numberDoc": $("#TransferIssuingAccountNumber").val(), "accountNumber": $("#TransferDocumentNumber").val() },
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


function validDepositVoucherDocumentNumber() {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/ValidateDepositVoucher",
        data: {
            "bankId": $("#DepositVoucherBankReceivingSelect").val(),
            "numberDoc": $("#DepositVoucherDocumentNumberSelect").val(),
            "accountNumber": $("#DepositVoucherReceivingAccountNumberSelect option:selected").text()
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


/***/
//$(document).ready(function () {
$("#TotalAmount").attr('disabled', '');

//FORMATOS/NÚMEROS-DECIMALES

$("#TotalAmount").blur(function () {
    $("#TotalAmount").val(FormatCurrency($("#TotalAmount").val()));
    $("#Difference").val($("#TotalAmount").val());
    $("#Difference").val(FormatCurrency($("#Difference").val()));
    setDifference();
});

$("#TotalCurrency").blur(function () {
    $("#TotalCurrency").val(ClearFormatCurrency($("#TotalCurrency").val()));
    $("#TotalCurrency").val(FormatCurrency($("#TotalCurrency").val()));
});

$("#TotalExchangeRate").blur(function () {
    $("#TotalExchangeRate").val(ClearFormatCurrency($("#TotalExchangeRate").val()));
    $("#TotalExchangeRate").val(FormatCurrency($("#TotalExchangeRate").val()));
});

$("#Difference").blur(function () {
    $("#Difference").val(parseFloat(ClearFormatCurrency($("#Difference").val())).toFixed(2));
    $("#Difference").val(FormatCurrency($("#Difference").val()));
});


$("#paymentsAmount").blur(function () {
    $("#paymentsAmount").val(FormatCurrency($("#paymentsAmount").val()));
});

$("#checkAmount").blur(function () {
    $("#checkAmount").val(FormatCurrency($("#checkAmount").val()));
});

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

////Valida que no se ingresen cheques postfechados cuando esta seleccionado "Cheque Corriente"
$("#CheckDateRegularizated").blur(function () {

    if ($("#CheckDateRegularizated").val() != '') {

        if (IsDate($("#CheckDateRegularizated").val()) == true) {

            if (CompareDates($("#CheckDateRegularizated").val(), getCurrentDate()) == 2) {
                $("#CheckDateRegularizated").val(getCurrentDate());
            }
            else if (CompareDates($("#CheckDateRegularizated").val(), getCurrentDate()) == 1) {
                $("#CheckDateRegularizated").val(getCurrentDate());
            }
        }
        else {
            $("#CheckDateRegularizated").val(getCurrentDate());
        }
    }
});

function loadCheckRegularization() {

    SourcePaymentId = parseInt($("#ViewBagPaymentId").val());
    CheckDocumentNumber = $("#ViewBagDocumentNumber").val();

    if (SourcePaymentId > 0) {
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Regularization/GetRejectedCheckInfoByPaymentId",
            data: { "paymentId": SourcePaymentId },
            success: function (data) {

                $("#checkIssuingBank").val(data.BankDescription);
                $("#checkIssuingAccountBank").val(data.IssuingAccountNumber);
                $("#checkDocumentNumber").val(data.DocumentNumber);
                $("#checkBillCode").val(data.CollectCode);
                $("#checkDate").val(data.DatePayment);
                $("#checkCurrency").val(data.CurrencyDescription);
                $("#checkAmount").val(data.Amount);
                $("#checkHolder").val(data.Holder);
                $("#checkStatus").val(data.StatusDescription);
                $("#checkRejectedDate").val(data.RejectionDate);
                $("#checkRejectedNumber").val(data.RejectedPaymentCode);
                $("#checkRejectedMotive").val(data.RejectionDescription);
                idBillConcept = data.CollectConceptCode;

                $("#TotalAmount").val(data.Amount * data.ExchangeRate);
                $("#TotalAmount").trigger('blur');
                $("#checkAmount").trigger('blur');
                
                setTimeout(function () {
                    $("#SelectBranchCheckRegularization").val($("#ViewBagBranchIdOrigin").val());
                    $("#SelectBranchCheckRegularization").trigger('change');
                    $("#SelectBranchCheckRegularization").attr('disabled', 'true');
                }, 1200);
            }
        });
    }
    else {
        if ($("#ViewBagPaymentId").val() != undefined) {

            location.href = $("#ViewBagMainCheckSearchLink").val();
        }
    }
}



//LLAMADA AL DIALOGO APERTURA DE CAJA Y CIERRE DE CAJA
$("#SelectBranchCheckRegularization").on('itemSelected', function (event, selectedItem) {
    if ($("#SelectBranchCheckRegularization").val() != "") {
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Billing/NeedCloseBill",
            data: { "branchId": $("#SelectBranchCheckRegularization").val(), "accountingDatePresent": $("#ViewBagDateAccounting").val() },
            success: function (userData) {
                if (userData[0].resp == true) {
                    $('#modalBillingClosureCheck').UifModal('showLocal', Resources.Closing);
                } else {
                    $.ajax({
                        async: false,
                        type: "POST",
                        url: ACC_ROOT + "Billing/AllowOpenBill",
                        data: { "branchId": $("#SelectBranchCheckRegularization").val(), "accountingDate": $("#ViewBagDateAccounting").val() },
                        success: function (userata) {
                            if (userata[0].resp == true) {
                                //OpenBillingDialog();
                                $('#modalOpeningBilling').find("#Branch").val($("#SelectBranchCheckRegularization").UifSelect("getSelectedText"));
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


//BOTÓN CANCELAR DE LA BARRA INFERIOR
$("#cancelBilling").click(function () {

    clearFieldsCheckRegularization();
    clearPaymentMethodFields();
    SetDataBillEmpty();
    location.href = $("#ViewBagMainCheckSearchLink").val();
    
});

//AÑADIR REGISTROS A LA TABLA DE RESUMEN DE TIPO DE PAGO
$("#_accept").click(function () {
    $("#alert").UifAlert('hide');
    Numerrors = 0;
    if (validarCampos($("#PaymentMethodType").val()) == true) {

        paySummary = {
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
                //1 efectivo

                keyValid = $("#PaymentMethodType").val() + $("#CashCurrencySelect").val();
                validatePostDatedCheck();

                break;
            case $("#ViewBagParamPaymentMethodCurrentCheck").val():
            case $("#ViewBagParamPaymentMethodDebit").val():
            case $("#ViewBagParamPaymentMethodPostdatedCheck").val():
                //Cheque

                keyValid = $("#CheckDocumentNumber").val() + IssuingBankId + $("#CheckAccountNumber").val();
                validCheckDocumentNumberCheckRegularization();
                if (Numerrors == 0) {
                    validatePostDatedCheck();
                }

                break;
            case $("#ViewBagParamPaymentMethodCreditableCreditCard").val():
            case $("#ViewBagParamPaymentMethodUncreditableCreditCard").val():
            case $("#ViewBagParamPaymentMethodDataphone").val():
                //Tarjeta acreditable

                keyValid = $("#CreditCardNumber").val() + $("#CreditCardVoucherNumber").val();
                validCreditCardVoucherNumber();
                if (Numerrors == 0) {
                    checkExpirationDate();
                }
                if (Numerrors == 0) {
                    validatePostDatedCheck();
                }
                if (Numerrors == 0) {
                    validateTaxBase();
                }

                break;
            case $("#ViewBagParamPaymentMethodDirectConection").val():
            case $("#ViewBagParamPaymentMethodTransfer").val():
                //conexion directa y transferencia

                keyValid = $("#TransferIssuingAccountNumber").val() + TransferIssuingBankId + $("#TransferDocumentNumber").val();
                validTransferDocumentNumber();
                if (Numerrors == 0) {
                    validatePostDatedCheck();
                }

                break;
            case $("#ViewBagParamPaymentMethodDepositVoucher").val():
                //boleta de depósito

                keyValid = $("#DepositVoucherBankReceivingSelect option:selected").text() + $("#DepositVoucherDocumentNumberSelect").val() + $("#DepositVoucherReceivingAccountNumberSelect option:selected").text(); /*$("#PaymentMethodType").val() + $("#RetentionReceiptCurrencySelet").val() +*/
                validDepositVoucherDocumentNumber();
                if (Numerrors == 0) {
                    validatePostDatedCheck();
                }

                break;
            case $("#ViewBagParamPaymentMethodRetentionReceipt").val():
                //recibo de retencion

                keyValid = $("#PaymentMethodType").val() + $("#TransferCurrencySelect").val() + $("#RetentionReceiptDocumentNumber").val();
                validatePostDatedCheck();

                break;
            default:
        }
        result = validarGridPaySummary(keyValid);
        validBank = validateBankId();

        switch ($("#PaymentMethodType").val()) {
            case $("#ViewBagParamPaymentMethodCash").val():
                //Efectivo

                $("#divCash").validate();

                if ($("#divCash").valid() && result == true && Numerrors == 0) {

                    totAmount = totAmount + parseFloat(ClearFormatCurrency($("#CashAmountRegularization").val().replace("", ",")) * exchangeRate);
                    totAmount = parseFloat(totAmount.toFixed(2));

                    if ($("#CashCurrencySelect").val() == localCurrencyId) {
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#CashAmountRegularization").val().replace("", ",")));
                        amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                        $("#TotalCurrency").val(amountLocalCurrency);
                        $("#TotalCurrency").trigger('blur');
                    }

                    if ($("#CashCurrencySelect").val() != localCurrencyId && $("#CashCurrencySelect").val() != -1) {
                        amountForeingCurrency = amountForeingCurrency + parseFloat(ClearFormatCurrency($("#CashAmountRegularization").val().replace("", ",")));
                        amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#CashLocalAmount").val().replace("", ",")));
                        $("#TotalExchangeRate").val(amountForeingCurrency);
                        $("#TotalExchangeRate").trigger('blur');
                        $("#TotalCurrency").val(amountLocalCurrency);
                        $("#TotalCurrency").trigger('blur');
                    }

                    if (parseFloat(ClearFormatCurrency($("#TotalCurrency").val().replace("", ","))) <= parseFloat(ClearFormatCurrency($("#TotalAmount").val().replace("", ",")))) {
                        $("#Difference").val(parseFloat(ClearFormatCurrency($("#TotalAmount").val().replace("", ","))) - totAmount);
                    }

                    paySummary.PaymentTypeId = $("#PaymentMethodType").val();
                    paySummary.PaymentTypeDescription = $("#PaymentMethodType option:selected").text();
                    paySummary.Amount = $("#CashAmountRegularization").val();
                    paySummary.Exchange = FormatCurrency(exchangeRate);
                    paySummary.CurrencyId = $("#CashCurrencySelect").val();
                    paySummary.Currency = $("#CashCurrencySelect option:selected").text();
                    paySummary.LocalAmount = FormatCurrency(parseFloat(ClearFormatCurrency($("#CashAmountRegularization").val().replace("", ",")) * exchangeRate));
                    paySummary.Date = $("#BillingDate").val();
                    paySummary.DocumentNumber = $("#DocumentNumber").val();
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
                        $("#TotalCurrency").val(amountLocalCurrency);
                        $("#TotalCurrency").trigger('blur');
                    }

                    if ($("#CheckCurrencySelect").val() != localCurrencyId && $("#CheckCurrencySelect").val() != -1) {
                        amountForeingCurrency = amountForeingCurrency + parseFloat(ClearFormatCurrency($("#CheckAmount").val().replace("", ",")));
                        amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#CheckLocalAmount").val().replace("", ",")));
                        $("#TotalExchangeRate").val(amountForeingCurrency);
                        $("#TotalExchangeRate").trigger('blur');
                        $("#TotalCurrency").val(amountLocalCurrency);
                        $("#TotalCurrency").trigger('blur');
                    }

                    if (parseFloat(ClearFormatCurrency($("#TotalCurrency").val().replace("", ","))) <= parseFloat(ClearFormatCurrency($("#TotalAmount").val().replace("", ",")))) {
                        $("#Difference").val(parseFloat(ClearFormatCurrency($("#TotalAmount").val().replace("", ","))) - totAmount);
                    }

                    paySummary.PaymentTypeId = $("#PaymentMethodType").val();
                    paySummary.PaymentTypeDescription = $("#PaymentMethodType option:selected").text();
                    paySummary.Amount = $("#CheckAmount").val();
                    paySummary.Exchange = FormatCurrency(exchangeRate);
                    paySummary.CurrencyId = $("#CheckCurrencySelect").val();
                    paySummary.Currency = $("#CheckCurrencySelect option:selected").text();
                    paySummary.LocalAmount = FormatCurrency(parseFloat(ClearFormatCurrency($("#CheckAmount").val().replace("", ",")) * exchangeRate));
                    paySummary.DocumentNumber = $("#CheckDocumentNumber").val();
                    paySummary.IssuingBankId = IssuingBankId;
                    paySummary.IssuinBankName = $("#CheckBank").val();
                    paySummary.IssuingBankAccountNumber = $("#CheckAccountNumber").val();
                    paySummary.IssuerName = $("#CheckHolderName").val();
                    //cheque postfechado
                    if ($("#PaymentMethodType").val() == "1") {
                        paySummary.Date = $("#CheckPostDate").val();
                        //cheque corriente
                    } else if ($("#PaymentMethodType").val() == "2") {
                        paySummary.Date = $("#CheckDateRegularizated").val();
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
                        $("#TotalCurrency").val(amountLocalCurrency);
                        $("#TotalCurrency").trigger('blur');
                    }

                    if ($("#CreditCardCurrencySelect").val() != localCurrencyId && $("#CreditCardCurrencySelect").val() != -1) {
                        amountForeingCurrency = amountForeingCurrency + parseFloat(ClearFormatCurrency($("#CreditCardAmount").val().replace("", ",")));
                        amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#CreditCardLocalAmount").val().replace("", ",")));
                        $("#TotalExchangeRate").val(amountForeingCurrency);
                        $("#TotalExchangeRate").trigger('blur');
                        $("#TotalCurrency").val(amountLocalCurrency);
                        $("#TotalCurrency").trigger('blur');
                    }

                    if (parseFloat(ClearFormatCurrency($("#TotalCurrency").val().replace("", ","))) <= parseFloat(ClearFormatCurrency($("#TotalAmount").val().replace("", ",")))) {
                        $("#Difference").val(parseFloat(ClearFormatCurrency($("#TotalAmount").val().replace("", ","))) - totAmount);
                    }

                    paySummary.PaymentTypeId = $("#PaymentMethodType").val();
                    paySummary.PaymentTypeDescription = $("#PaymentMethodType option:selected").text();
                    paySummary.Amount = $("#CreditCardAmount").val();
                    paySummary.Exchange = FormatCurrency(exchangeRate);
                    paySummary.CurrencyId = $("#CreditCardCurrencySelect").val();
                    paySummary.Currency = $("#CreditCardCurrencySelect option:selected").text();
                    paySummary.LocalAmount = FormatCurrency(parseFloat(ClearFormatCurrency($("#CreditCardAmount").val().replace("", ",")) * exchangeRate));
                    paySummary.VoucherNumber = $("#CreditCardVoucherNumber").val();
                    paySummary.CardNumber = $("#CreditCardNumber").val();
                    paySummary.AuthorizationNumber = $("#CreditCardAuthorizationNumber").val();
                    paySummary.IssuingBankId = CreditCardIssuingBankId;
                    paySummary.IssuinBankName = $("#CreditCardIssuingBankInput").val();
                    paySummary.CardsName = $("#CreditCardHolderName").val();
                    paySummary.CardType = $("#CreditCardTypeSelect").val();
                    paySummary.CardTypeName = $("#CreditCardTypeSelect option:selected").text();
                    paySummary.ValidThruYear = $("#CreditCardValidThruYear").val();
                    paySummary.ValidThruMonth = $("#CreditCardValidThruMonthSelect").val();
                    paySummary.Tax = $("#tax").val();
                    paySummary.TaxBase = parseFloat(ClearFormatCurrency($("#CreditCardTaxBase").val().replace("", ",")));
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
                        $("#TotalCurrency").val(amountLocalCurrency);
                        $("#TotalCurrency").trigger('blur');
                    }

                    if ($("#TransferCurrencySelect").val() != localCurrencyId && $("#TransferCurrencySelect").val() != -1) {
                        amountForeingCurrency = amountForeingCurrency + parseFloat(ClearFormatCurrency($("#TransferAmount").val().replace("", ",")));
                        amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#TransferLocalAmount").val().replace("", ",")));
                        $("#TotalExchangeRate").val(amountForeingCurrency);
                        $("#TotalExchangeRate").trigger('blur');
                        $("#TotalCurrency").val(amountLocalCurrency);
                        $("#TotalCurrency").trigger('blur');
                    }

                    if (parseFloat(ClearFormatCurrency($("#TotalCurrency").val().replace("", ","))) <= parseFloat(ClearFormatCurrency($("#TotalAmount").val().replace("", ",")))) {
                        $("#Difference").val(parseFloat(ClearFormatCurrency($("#TotalAmount").val().replace("", ","))) - totAmount);
                    }

                    paySummary.PaymentTypeId = $("#PaymentMethodType").val();
                    paySummary.PaymentTypeDescription = $("#PaymentMethodType option:selected").text();
                    paySummary.Amount = $("#TransferAmount").val();
                    paySummary.Exchange = FormatCurrency(exchangeRate);
                    paySummary.CurrencyId = $("#TransferCurrencySelect").val();
                    paySummary.Currency = $("#TransferCurrencySelect option:selected").text();
                    paySummary.LocalAmount = FormatCurrency(parseFloat(ClearFormatCurrency($("#TransferAmount").val().replace("", ",")) * exchangeRate));
                    paySummary.DocumentNumber = $("#TransferIssuingAccountNumber").val();
                    paySummary.IssuingBankId = TransferIssuingBankId;
                    paySummary.IssuinBankName = $("#TransferIssuingBank").val();
                    paySummary.IssuingBankAccountNumber = $("#TransferDocumentNumber").val();
                    paySummary.RecievingBankId = $("#TransferReceivingBankSelect").val();
                    paySummary.RecievingBankName = $("#TransferReceivingBankSelect option:selected").text();
                    paySummary.RecievingBankAccountNumber = $("#AccountNumberReceivingBankSelect option:selected").text();
                    paySummary.Date = $("#TransferDate").val();
                    paySummary.IssuerName = $("#TransferHolderName").val();
                }

                break;
            case $("#ViewBagParamPaymentMethodDepositVoucher").val():

                //Boleta de Depósito
                $("#divDepositVoucher").validate();

                if ($("#divDepositVoucher").valid() || result == true && Numerrors == 0 && validBank == true) {

                    totAmount = totAmount + parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")) * exchangeRate);
                    totAmount = parseFloat(totAmount.toFixed(2));

                    if ($("#DepositVoucherCurrencySelect").val() == localCurrencyId) {
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")));
                        amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                        $("#TotalCurrency").val(amountLocalCurrency);
                        $("#TotalCurrency").trigger('blur');
                    }

                    if ($("#DepositVoucherCurrencySelect").val() != localCurrencyId && $("#DepositVoucherCurrencySelect").val() != -1) {
                        amountForeingCurrency = amountForeingCurrency + parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")));
                        amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#DepositVoucherLocalAmount").val().replace("", ",")));
                        $("#TotalExchangeRate").val(amountForeingCurrency);
                        $("#TotalExchangeRate").trigger('blur');
                        $("#TotalCurrency").val(amountLocalCurrency);
                        $("#TotalCurrency").trigger('blur');
                    }

                    if (parseFloat(ClearFormatCurrency($("#TotalCurrency").val().replace("", ","))) <= parseFloat(ClearFormatCurrency($("#TotalAmount").val().replace("", ",")))) {
                        $("#Difference").val(parseFloat(ClearFormatCurrency($("#TotalAmount").val().replace("", ","))) - totAmount);
                    }

                    paySummary.PaymentTypeId = $("#PaymentMethodType").val();
                    paySummary.PaymentTypeDescription = $("#PaymentMethodType option:selected").text();
                    paySummary.Amount = parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")));
                    paySummary.Exchange = FormatCurrency(exchangeRate);
                    paySummary.CurrencyId = $("#DepositVoucherCurrencySelect").val();
                    paySummary.Currency = $("#DepositVoucherCurrencySelect option:selected").text();
                    paySummary.LocalAmount = parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")) * exchangeRate);
                    paySummary.DocumentNumber = $("#DepositVoucherDocumentNumberSelect").val();
                    paySummary.RecievingBankId = $("#DepositVoucherBankReceivingSelect").val();
                    paySummary.RecievingBankName = $("#DepositVoucherBankReceivingSelect option:selected").text();
                    paySummary.RecievingBankAccountNumber = $("#DepositVoucherReceivingAccountNumberSelect option:selected").text();
                    paySummary.Date = $("#DepositVoucherDate").val();
                    paySummary.IssuerName = $("#DepositVoucherHolderName").val();
                }

                break;
            case $("#ViewBagParamPaymentMethodRetentionReceipt").val():
                //recibo de retencion
                $("#divRetentionReceipt").validate();

                if ($("#divRetentionReceipt").valid() || result == true && Numerrors == 0) {

                    totAmount = totAmount + parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")) * exchangeRate);
                    totAmount = parseFloat(totAmount.toFixed(2));

                    if ($("#RetentionReceiptCurrencySelet").val() == localCurrencyId) {
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")));
                        amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                        $("#TotalCurrency").val(amountLocalCurrency);
                        $("#TotalCurrency").trigger('blur');
                    }

                    if ($("#RetentionReceiptCurrencySelet").val() != localCurrencyId && $("#RetentionReceiptCurrencySelet").val() != -1) {
                        amountForeingCurrency = amountForeingCurrency + parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")));
                        amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                        amountLocalCurrency = amountLocalCurrency + parseFloat(ClearFormatCurrency($("#RetentionReceiptLocalAmount").val().replace("", ",")));
                        $("#TotalExchangeRate").val(amountForeingCurrency);
                        $("#TotalExchangeRate").trigger('blur');
                        $("#TotalCurrency").val(amountLocalCurrency);
                        $("#TotalCurrency").trigger('blur');
                    }

                    if (parseFloat(ClearFormatCurrency($("#TotalCurrency").val().replace("", ","))) <= parseFloat(ClearFormatCurrency($("#TotalAmount").val().replace("", ",")))) {
                        $("#Difference").val(parseFloat(ClearFormatCurrency($("#TotalAmount").val().replace("", ","))) - totAmount);
                    }

                    paySummary.PaymentTypeId = $("#PaymentMethodType").val();
                    paySummary.PaymentTypeDescription = $("#PaymentMethodType option:selected").text();
                    paySummary.Amount = parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")));
                    paySummary.Exchange = FormatCurrency(exchangeRate);
                    paySummary.CurrencyId = $("#RetentionReceiptCurrencySelet").val();
                    paySummary.Currency = $("#RetentionReceiptCurrencySelet option:selected").text();
                    paySummary.LocalAmount = parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")) * exchangeRate);
                    paySummary.DocumentNumber = $("#RetentionReceiptDocumentNumber").val();
                    paySummary.AuthorizationNumber = $("#RetentionReceiptAuthorizationNumber").val();
                    paySummary.Date = $("#RetentionReceiptDate").val();
                    paySummary.SerialVoucher = $("#RetentionReceiptSerialVoucher").val();
                    paySummary.VoucherNumber = $("#RetentionReceiptVoucher").val();
                    paySummary.SerialNumber = $("#RetentionReceiptSerialNumber").val();
                }

                break;
            default:
        }


        if (Numerrors == 0) {
            if (totAmount <= parseFloat(ClearFormatCurrency($("#TotalAmount").val().replace("", ",")))) {

                if (result == true && validBank == true) {

                    var valor = parseInt($('#rowid').val()) + 1;
                    $('#rowid').val(valor);

                    //Valida que no haya números de documento repetidos por c/ tipo de pago
                    if (Numerrors == 0) {
                        jQuery("#tblPaySummary").UifDataTable('addRow', paySummary);
                        $("#Difference").val(FormatCurrency($("#Difference").val()));
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
                            $("#TotalCurrency").val(amountLocalCurrency);
                            $("#TotalCurrency").trigger('blur');
                        }

                        if ($("#CashCurrencySelect").val() != localCurrencyId && $("#CashCurrencySelect").val() != -1) {
                            amountForeingCurrency = amountForeingCurrency - parseFloat(ClearFormatCurrency($("#CashAmountRegularization").val().replace("", ",")));
                            amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#CashLocalAmount").val().replace("", ",")));
                            $("#TotalExchangeRate").val(amountForeingCurrency);
                            $("#TotalExchangeRate").trigger('blur');
                            $("#TotalCurrency").val(amountLocalCurrency);
                            $("#TotalCurrency").trigger('blur');
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
                            $("#TotalCurrency").val(amountLocalCurrency);
                            $("#TotalCurrency").trigger('blur');
                        }

                        if ($("#CheckCurrencySelect").val() != localCurrencyId && $("#CheckCurrencySelect").val() != -1) {
                            amountForeingCurrency = amountForeingCurrency - parseFloat(ClearFormatCurrency($("#CheckAmount").val().replace("", ",")));
                            amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#CheckLocalAmount").val().replace("", ",")));
                            $("#TotalExchangeRate").val(amountForeingCurrency);
                            $("#TotalExchangeRate").trigger('blur');
                            $("#TotalCurrency").val(amountLocalCurrency);
                            $("#TotalCurrency").trigger('blur');
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
                            $("#TotalCurrency").val(amountLocalCurrency);
                            $("#TotalCurrency").trigger('blur');
                        }

                        if ($("#CreditCardCurrencySelect").val() != localCurrencyId && $("#CreditCardCurrencySelect").val() != -1) {
                            amountForeingCurrency = amountForeingCurrency - parseFloat(ClearFormatCurrency($("#CreditCardAmount").val().replace("", ",")));
                            amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#CreditCardLocalAmount").val().replace("", ",")));
                            $("#TotalExchangeRate").val(amountForeingCurrency);
                            $("#TotalExchangeRate").trigger('blur');
                            $("#TotalCurrency").val(amountLocalCurrency);
                            $("#TotalCurrency").trigger('blur');
                        }
                        break;
                    //case $("#ViewBagParamPaymentMethodDirectConection").val():
                    case $("#ViewBagParamPaymentMethodTransfer").val():

                        totAmount = totAmount - parseFloat(ClearFormatCurrency($("#TransferAmount").val().replace("", ",")) * exchangeRate);
                        totAmount = parseFloat(totAmount.toFixed(2));

                        if ($("#TransferCurrencySelect").val() == localCurrencyId) {
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#TransferAmount").val().replace("", ",")));
                            amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                            $("#TotalCurrency").val(amountLocalCurrency);
                            $("#TotalCurrency").trigger('blur');
                        }

                        if ($("#TransferCurrencySelect").val() != localCurrencyId && $("#TransferCurrencySelect").val() != -1) {
                            amountForeingCurrency = amountForeingCurrency - parseFloat(ClearFormatCurrency($("#TransferAmount").val().replace("", ",")));
                            amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#TransferLocalAmount").val().replace("", ",")));
                            $("#TotalExchangeRate").val(amountForeingCurrency);
                            $("#TotalExchangeRate").trigger('blur');
                            $("#TotalCurrency").val(amountLocalCurrency);
                            $("#TotalCurrency").trigger('blur');
                        }

                        break;
                    case $("#ViewBagParamPaymentMethodDepositVoucher").val():
                        //boleta de deposito

                        totAmount = totAmount - parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")) * exchangeRate);
                        totAmount = parseFloat(totAmount.toFixed(2));

                        if ($("#DepositVoucherCurrencySelect").val() == localCurrencyId) {
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")));
                            amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                            $("#TotalCurrency").val(amountLocalCurrency);
                            $("#TotalCurrency").trigger('blur');
                        }

                        if ($("#DepositVoucherCurrencySelect").val() != localCurrencyId && $("#DepositVoucherCurrencySelect").val() != -1) {
                            amountForeingCurrency = amountForeingCurrency - parseFloat(ClearFormatCurrency($("#DepositVoucherAmount").val().replace("", ",")));
                            amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#DepositVoucherLocalAmount").val().replace("", ",")));
                            $("#TotalExchangeRate").val(amountForeingCurrency);
                            $("#TotalExchangeRate").trigger('blur');
                            $("#TotalCurrency").val(amountLocalCurrency);
                            $("#TotalCurrency").trigger('blur');
                        }

                        break;
                    case $("#ViewBagParamPaymentMethodRetentionReceipt").val():
                        //recibo de retencion

                        totAmount = totAmount - parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")) * exchangeRate);
                        totAmount = parseFloat(totAmount.toFixed(2));

                        if ($("#RetentionReceiptCurrencySelet").val() == localCurrencyId) {
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")));
                            amountLocalCurrency = parseFloat(amountLocalCurrency.toFixed(2));
                            $("#TotalCurrency").val(amountLocalCurrency);
                            $("#TotalCurrency").trigger('blur');
                        }

                        if ($("#RetentionReceiptCurrencySelet").val() != localCurrencyId && $("#RetentionReceiptCurrencySelet").val() != -1) {
                            amountForeingCurrency = amountForeingCurrency - parseFloat(ClearFormatCurrency($("#RetentionReceiptAmount").val().replace("", ",")));
                            amountForeingCurrency = parseFloat(amountForeingCurrency.toFixed(2));
                            amountLocalCurrency = amountLocalCurrency - parseFloat(ClearFormatCurrency($("#RetentionReceiptLocalAmount").val().replace("", ",")));
                            $("#TotalExchangeRate").val(amountForeingCurrency);
                            $("#TotalExchangeRate").trigger('blur');
                            $("#TotalCurrency").val(amountLocalCurrency);
                            $("#TotalCurrency").trigger('blur');
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
        clearPaymentMethodFields();
    }
});

// Elimina registro y resta su valor del total contabilizado

//$('#tblPaySummary').on('rowDelete', function (event, selectedRow, position) {
//    $('#tblPaySummary').UifDataTable('deleteRow', position);
//    deleteRowCheckRegularization(selectedRow);
//});


$("#saveBilling").click(function () {

    $("#frmCardVoucherGeneralInformation").validate();

    if ($("#frmCardVoucherGeneralInformation").valid() && (Numerrors == 0 || Numerrors == undefined)) {
        Alert = 0;
    } else {
        Alert = 1;
        Numerrors = 0;
    }

    if (Alert == 0) {
        showConfirmCheckRegularization();
    }
});


/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

// Valida el ingreso de campos obligatorios
function ValidateAddForm(paymentMethod) {
    // Cash
    if (paymentMethod == "E") {
        if ($("#modalCashAdd").find('#selectCashCurrency').val() == "") {
            $("#modalCashAdd").find("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
            return false;
        }
        if ($("#modalCashAdd").find('#CashAmountRegularization').val() == "") {
            $("#modalCashAdd").find("#alertForm").UifAlert('show', Resources.AmountRequired, "warning");
            return false;
        }
    }
    // Check
    if (paymentMethod == "C") {
        if ($("#modalCheckAdd").find('#selectCheckCurrency').val() == "") {
            $("#modalCheckAdd").find("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
            return false;
        }
        if ($("#modalCheckAdd").find('#CheckAmount').val() == "") {
            $("#modalCheckAdd").find("#alertForm").UifAlert('show', Resources.AmountRequired, "warning");
            return false;
        }
        if ($("#modalCheckAdd").find('#inputCheckIssuingBank').val() == "") {
            $("#modalCheckAdd").find("#alertForm").UifAlert('show', Resources.IssuingBankRequired, "warning");
            return false;
        }
        if ($("#modalCheckAdd").find('#CheckAccountNumber').val() == "") {
            $("#modalCheckAdd").find("#alertForm").UifAlert('show', Resources.AccountNumberRequired, "warning");
            return false;
        }
        if ($("#modalCheckAdd").find('#CheckDocumentNumber').val() == "") {
            $("#modalCheckAdd").find("#alertForm").UifAlert('show', Resources.DocumentNumberRequired, "warning");
            return false;
        }
        if ($("#modalCheckAdd").find('#CheckHolderName').val() == "") {
            $("#modalCheckAdd").find("#alertForm").UifAlert('show', Resources.HolderNameRequired, "warning");
            return false;
        }
        if ($("#modalCheckAdd").find('#CheckDateRegularizated').val() == "") {
            $("#modalCheckAdd").find("#alertForm").UifAlert('show', Resources.DateRequired, "warning");
            return false;
        }
    }
    // CreditCard
    if (paymentMethod == "T") {
        if ($("#modalCardAdd").find('#selectCreditCardCurrency').val() == "") {
            $("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
            return false;
        }
        if ($("#modalCardAdd").find('#CreditCardAmount').val() == "") {
            $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.AmountRequired, "warning");
            return false;
        }
        if ($("#modalCardAdd").find('#selectCreditCardType').val() == "") {
            $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.ConduitRequired, "warning");
            return false;
        }
        if ($("#modalCardAdd").find('#inputCreditCardIssuingBank').val() == "") {
            $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.IssuingBankRequired, "warning");
            return false;
        }
        if ($("#modalCardAdd").find('#CreditCardNumber').val() == "") {
            $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.CreditCardNumberRequired, "warning");
            return false;
        }
        if ($("#modalCardAdd").find('#CreditCardAuthorizationNumber').val() == "") {
            $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.AuthorizationNumberRequired, "warning");
            return false;
        }
        if ($("#modalCardAdd").find('#CreditCardVoucherNumber').val() == "") {
            $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.VoucherNumberRequired, "warning");
            return false;
        }
        if ($("#modalCardAdd").find('#CreditCardHolderName').val() == "") {
            $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.NameRequired, "warning");
            return false;
        }
        if ($("#modalCardAdd").find('#selectCreditCardValidThruMonth').val() == "") {
            $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.MonthRequired, "warning");
            return false;
        }
        if ($("#modalCardAdd").find('#CreditCardValidThruYear').val() == "") {
            $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.YearRequired, "warning");
            return false;
        }
        if ($("#modalCardAdd").find('#CreditCardTaxBase').val() == "") {
            $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.TaxBaseRequired, "warning");
            return false;
        }
    }
    // Consignment
    if (paymentMethod == "B") {
        if ($("#modalConsignmentAdd").find('#selectConsignmentCurrency').val() == "") {
            $("#modalConsignmentAdd").find("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
            return false;
        }
        if ($("#modalConsignmentAdd").find('#ConsignmentAmount').val() == "") {
            $("#modalConsignmentAdd").find("#alertForm").UifAlert('show', Resources.AmountRequired, "warning");
            return false;
        }
        if ($("#modalConsignmentAdd").find('#selectConsignmentReceivingBank').val() == "") {
            $("#modalConsignmentAdd").find("#alertForm").UifAlert('show', Resources.ReceivingBankRequired, "warning");
            return false;
        }
        if ($("#modalConsignmentAdd").find('#selectConsignmentAccountNumber').val() == "") {
            $("#modalConsignmentAdd").find("#alertForm").UifAlert('show', Resources.AccountNumberRequired, "warning");
            return false;
        }
        if ($("#modalConsignmentAdd").find('#ConsignmentBallotNumber').val() == "") {
            $("#modalConsignmentAdd").find("#alertForm").UifAlert('show', Resources.BallotNumberRequired, "warning");
            return false;
        }
        if ($("#modalConsignmentAdd").find('#ConsignmentDate').val() == "") {
            $("#modalConsignmentAdd").find("#alertForm").UifAlert('show', Resources.DateRequired, "warning");
            return false;
        }
        if ($("#modalConsignmentAdd").find('#ConsignmentDepositorName').val() == "") {
            $("#modalConsignmentAdd").find("#alertForm").UifAlert('show', Resources.DepositorNameRequired, "warning");
            return false;
        }
    }

    // Datafono
    if (paymentMethod == "D") {
        if ($("#modalDatafonoAdd").find('#selectDatafonoCurrency').val() == "") {
            $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
            return false;
        }
        if ($("#modalDatafonoAdd").find('#CashAmountRegularization').val() == "") {
            $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.AmountRequired, "warning");
            return false;
        }
    }

    return true;
}

function clearCheckSearchFields() {
    $("#searchIssuingBank").val('');
    $("#searchCheckNumber").val('');
    $("#checkDate").val('');
    $("#checkCurrency").val('');
    $("#checkAmount").val('');
    $("#checkIssuer").val('');
    $("#checkRejectedDate").val('');
    $("#rejectedCheckAmount").val('');
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
        }
    });
}

//Comprobación de cambios en Importe Total
function changeTotalAmount() {

    var sum = 0;
    var pays = $("#tblPaySummary").UifDataTable("getData");
    if (pays.length > 0) {
        for (var x in pays) {
            var rowData = pays[x];
            sum = sum + parseFloat(rowData.ImporteLocal);
        }

        if (sum != ClearFormatCurrency($("#TotalAmount").val().replace("", ","))) {
            $("#alert").UifAlert('show', Resources.ChangesOnTotalAmount, 'warning');
        }
    }
}

function setDifference() {
    var sum = 0;
    var dataPayments = $("#tblPaySummary").UifDataTable("getData");

    if ($("#paymentsAmount").val() == '') {
        $("#Difference").val($("#TotalAmount").val());
    } else if ($("#paymentsAmount").val() != '') {
        if (dataPayments != null) {
            for (i in dataPayments) {
                var rowData = dataPayments[i];
                sum = sum + parseFloat(ClearFormatCurrency(rowData.LocalAmount));
                sum = parseFloat(sum.toFixed(2));
            }
            $("#Difference").val(parseFloat(ClearFormatCurrency($("#TotalAmount").val().replace("", ","))) - sum);
            $("#Difference").trigger('blur');
        }
    }

}

function clearFieldsCheckRegularization() {

    //parcial de Información General
    payerId = -1;
    $("#DocumentNumber").val('');
    $("#FirstLastName").val('');
    $("#SelectBranchCheckRegularization").val('');
    $("#Description").val('');
    $("#ddlIncomeConcept").val(0);
    idBillConcept = 0;

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

    IssuingBankId = -1;
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

//Valida campos vacios en resumen de Pagos

function validarCampos(idPay) {
    var result = true;
    switch (idPay) {
        case "0":
            result = false;
            break;
        case $("#ViewBagParamPaymentMethodCash").val():
            //Efectivo
            $("#divCash").validate();

            if (!$("#divCash").valid() || $("#CashAmountRegularization").val() == "" || $("#CashAmountRegularization").val() == "$" || $("#CashCurrencySelect").val() == "-1") {
                result = false;
            }

            break;
        case $("#ViewBagParamPaymentMethodCurrentCheck").val():
        case $("#ViewBagParamPaymentMethodDebit").val():
        case $("#ViewBagParamPaymentMethodPostdatedCheck").val():
            //Cheque y debito
            $("#divCheck").validate();
            if ($("#divCheck").valid() && $("#PaymentMethodType").val() == "1") {

                if ($("#CheckAmount").val() == "" || $("#CheckBank").val() == "" || $("#CheckAccountNumber").val() == "" ||
                    $("#CheckDocumentNumber").val() == "" || $("#CheckHolderName").val() == "" || $("#CheckPostDate").val() == "") {
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

            if ($("#CreditCardAmount").val() == "" || $("#CreditCardTypeSelect").val() == "-1" || $("#CreditCardIssuingBankInput").val() == "" || $("#CreditCardNumber").val() == "" || $("#CreditCardAuthorizationNumber").val() == ""
                || $("#CreditCardVoucherNumber").val() == "" || $("#CreditCardHolderName").val() == "" || $("#CreditCardValidThruYear").val() == "") {
                result = false;
            }

            break;
        case $("#ViewBagParamPaymentMethodDirectConection").val():
        case $("#ViewBagParamPaymentMethodTransfer").val():
            //Transferencia
            $("#divTransfer").validate();

            if (!$("#divTransfer").valid() || $("#TransferAmount").val() == "" || $("#TransferIssuingAccountNumber").val() == "" || $("#ddlTransferIssuingBank").val() == "-1"
                || $("#TransferDate").val() == "" || $("#TransferReceivingBankSelect").val() == "-1" || $("#TransferCurrencySelect").val() == "-1" || $("#ddlTransferReceivingAccountNumber").val() == "-1") {
                result = false;
            }

            break;
        case $("#ViewBagParamPaymentMethodDepositVoucher").val():
            //boleta de deposito

            if ($("#DepositVoucherAmount").val() == "" || $("#DepositVoucherBankReceivingSelect").val() == "-1" || $("#DepositVoucherReceivingAccountNumberSelect").val() == "-1"
                || $("#DepositVoucherDocumentNumberSelect").val() == "" || $("#DepositVoucherHolderName").val() == "" || $("#DepositVoucherDate").val() == "") {
                result = false;
            }

            break;
        case $("#ViewBagParamPaymentMethodRetentionReceipt").val():
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
                case $("#ViewBagParamPaymentMethodDepositVoucher").val():
                    keyPay = rowData.RecievingBankName + rowData.DocumentNumber + rowData.RecievingBankAccountNumber;
                    break;

                    //recibo de retencion
                case $("#ViewBagParamPaymentMethodRetentionReceipt").val():
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


function ClosureBillingCheckRegularization() {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/NeedCloseBill",
        data: { "branchId": $("#SelectBranchCheckRegularization").val(), "accountingDatePresent": $("#ViewBagDateAccounting").val() },
        success: function (userData) {
            if (userData[0].resp == true) {
                $('#modalBillingClosureCheck').UifModal('showLocal', Resources.Closing);
            } else {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "Billing/AllowOpenBill",
                    data: { "branchId": $("#SelectBranchCheckRegularization").val(), "accountingDate": $("#ViewBagDateAccounting").val() },
                    success: function (userata) {

                        if (userata[0].resp == true) {
                            $('#modalOpeningBilling').find("#Branch").val($("#SelectBranchCheckRegularization").UifSelect("getSelectedText"));
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

function SetDataBill() {
    var paymentsTotal = 0;

    oBillModel.BillId = 0;
    oBillModel.BillingConceptId = idBillConcept;
    oBillModel.BillControlId = idBillControl;
    oBillModel.RegisterDate = $("#BillingDate").val();
    oBillModel.Description = Resources.ForCheckRegularization + " " + CheckDocumentNumber;
    paymentsTotal = RemoveFormatMoney($("#TotalAmount").val());
    oBillModel.PaymentsTotal = paymentsTotal;

    oBillModel.PayerId = payerId;
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
        oPaymentSummaryModel.Amount = RemoveFormatMoney(paymentsSummaryRow.Amount);
        oPaymentSummaryModel.CurrencyId = paymentsSummaryRow.CurrencyId;
        oPaymentSummaryModel.LocalAmount = RemoveFormatMoney(paymentsSummaryRow.LocalAmount);
        oPaymentSummaryModel.ExchangeRate = RemoveFormatMoney(paymentsSummaryRow.Exchange);

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
            oCreditModel.TaxBase = paymentsSummaryRow.TaxBase;

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
        if (paymentsSummaryRow.PaymentTypeId == $("#ViewBagParamPaymentMethodRetentionReceipt").val()) {
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

function GetAccountingDateCheckRegularization() {
    $.ajax({
        async: false,
        type: "GET",
        url: ACC_ROOT + "Common/GetAccountingDate",
        success: function (data) {
            $("#billingDateDialog").val(data);
            $("#modalSaveBill").find("#BillingDate").text(data);
        }
    });
}

function SetDataBillEmpty() {

    paySummary = {
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
        PaymentSummary: []
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
        ReceivingAccountBankId: 0,
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

function showConfirmCheckRegularization() {
    $.UifDialog('confirm', { 'message': Resources.RegularizationConfirmationMessage, 'title': Resources.CheckRegularization }, function (result) {
        if (result) {

            //hace las validaciones
            if (ClearFormatCurrency($("#Difference").val().replace("", ",")) != 0.00) {
                $("#alert").UifAlert('show', Resources.RemainingDifference, 'warning');
            }
            else if ($("#SelectBranchCheckRegularization").val() != -1 && Alert == 0 && ClearFormatCurrency($("#Difference").val().replace("", ",")) == 0.00) {
                if (true) {
                    if (isOpen == true) {
                        $("#modalSaveBill").find("#ApplyBill").hide();

                        $.ajax({
                            async: false,
                            type: "POST",
                            url: ACC_ROOT + "Regularization/SaveBillRequest",
                            data: { "frmBill": prepareRequest(SetDataBill()), "branchId": $("#SelectBranchCheckRegularization").val() },
                            success: function (data) {

                                var resultTransaction = "00000" + $.trim(data.TechnicalTransaction.toString())
                                $("#modalSaveBill").find("#TransactionNumber").val(resultTransaction);
                                $("#modalSaveBill").find("#TransactionNumber").text(resultTransaction);
                                $("#modalSaveBill").find("#ReceiptDescription").text(data.Description);
                                $("#modalSaveBill").find("#ReceiptTotalAmount").text($("#TotalAmount").val());
                                $("#modalSaveBill").find("#BillingId").text("00000" + data.Id);
                                $("#modalSaveBill").find("#BillingId").val(data.Id);
                                GetAccountingDateCheckRegularization();
                                $("#modalSaveBill").find("#ReceiptUser").text($("#ViewBagUserNick").val());
                                if (data.ShowMessage == "False") {
                                    $("#modalSaveBill").find("#accountingLabelDiv").hide();
                                    $("#modalSaveBill").find("#accountingMessageDiv").hide();
                                }
                                else {
                                    $("#modalSaveBill").find("#accountingLabelDiv").show();
                                    $("#modalSaveBill").find("#accountingMessageDiv").show();
                                }
                                $("#modalSaveBill").find("#accountingMessage").val(data.Message);

                                branchId = $("#SelectBranchCheckRegularization").val();
                                BillId = data.Id;
                                $("#modalSaveBill").find("#ApplyBill").hide();

                                if (data.Status.Id == 1) {
                                    PayerDocumentNumber = $("#inputDocumentNumber").val();
                                    PayerName = $("#inputName").val();
                                    BranchDescription = $("#selectBranch option:selected").text();
                                    ApplyDescription = $("#Observation").val();
                                }

                                if (data.Status.Id == 3) {
                                    if (data.ShowImputationMessage == "False") {
                                        $("#modalSaveBill").find("#applicationIntegration").hide();
                                    }
                                    else {
                                        $("#modalSaveBill").find("#applicationIntegration").show();
                                        $("#modalSaveBill").find("#accountingIntegrationMessage").val(data.ImputationMessage);
                                    }
                                }
                                else {
                                    $("#modalSaveBill").find("#applicationIntegration").hide();
                                }

                                $('#modalSaveBill').UifModal('showLocal', Resources.ReceiptSaveSuccess);

                                SetDataBillEmpty();
                                branchId = $("#SelectBranchCheckRegularization").val();
                                clearFieldsCheckRegularization();
                                clearPaymentMethodFields();

                            }
                        });
                        isOpen = false;
                    }
                    else {
                        ClosureBillingCheckRegularization();
                    }
                }
            }
        }
    });
};

// Visualiza el reporte de caja en pantalla
function ShowReceiptReport(branchId, billCode, otherPayerName) {
    var controller = ACC_ROOT + "Report/ShowReceiptReport?branchId=" + parseInt(branchId) + "&billCode=" +
                     parseInt(billCode) + "&reportId=3" + "&otherPayerName=" +otherPayerName;
    window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

//valida que el BaseIva no sea mayor al importe
function validateTaxBase() {
    if (parseFloat(ClearFormatCurrency($("#CreditCardTaxBase").val().replace("", ","))) > parseFloat(ClearFormatCurrency($("#CreditCardAmount").val().replace("", ",")))) {
        $("#alert").UifAlert('show', Resources.WarningGreaterTaxBase, 'warning');
        Numerrors = 1;
    } else {
        Numerrors = 0;
    }
}

//valida que el id de banco se haya ingresado
function validateBankId() {
    var result = true;
    switch ($("#PaymentMethodType").val()) {
        case $("#ViewBagParamPaymentMethodCurrentCheck").val():
        case $("#ViewBagParamPaymentMethodDebit").val():
            //Cheque
        case $("#ViewBagParamPaymentMethodPostdatedCheck").val():
            if (IssuingBankId == -1) {
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
        case $("#ViewBagParamPaymentMethodTransfer").val():
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
//});


/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

var exchangeRate = 0;
var TransferIssuingBankId = -1;
var CreditCardIssuingBankId = -1;
var DepositVoucherReceivingBankId = -1;

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

function validPostDateDate() {

    var isDate = IsDate($("#CheckPostDate").val());
    var result = CompareDates($("#CheckPostDate").val(), getCurrentDate());

    if (isDate == true) {

        if (IsDate($("#CheckPostDate").val()) == true) {

            if (result == 1) {
                $("#CheckPostDate").val(getCurrentDate());
                showMsgCheckRegularization(Resources.InvalidDates);
            } else if (result == 0) {
                $("#CheckPostDate").val(getCurrentDate());
                showMsgCheckRegularization(Resources.InvalidDates);
            }
        }
    }
    else {

        $("#CheckPostDate").val(getCurrentDate());
        showMsgCheckRegularization(Resources.InvalidDates);
    }
}

function showMsgCheckRegularization(msg) {
    $("#alert").UifAlert('show', msg, 'warning');
}

//validar no se utiliza
function informationTable(cellvalue, options, rowObject) {

    var table;

    //pago en efectivo
    if (rowObject.TipoPagoId == $("#ViewBagParamPaymentMethodCash").val()) {
        table = "";
    }
    //transferencia
    if (rowObject.TipoPagoId == $("#ViewBagParamPaymentMethodDirectConection").val() || rowObject.TipoPagoId == $("#ViewBagParamPaymentMethodTransfer").val())
        table = "<table border = 1><tr><td>Resources.DocumentNumber:</td><td>" + paySummary.NoDocumento + "</td></tr><tr><td>Resources.IssuingBank:</td><td>" + paySummary.BancoEmisor + "</td></tr><tr><td>Resources.ReceivingBank:</td><td>" + paySummary.BancoReceptor + "</td></tr><tr><td>Resources.Date:</td><td>" + paySummary.Fecha + "</td></tr></table><br>";
    //Tarjeta de Crédito
    if (rowObject.TipoPagoId == $("#ViewBagParamPaymentMethodCreditableCreditCard").val() || rowObject.TipoPagoId == $("#ViewBagParamPaymentMethodUncreditableCreditCard").val() || rowObject.TipoPagoId == $("#ViewBagParamPaymentMethodDataphone").val())
        table = "<table border = 1><tr><td>Resources.VoucherNumber:</td><td>" + paySummary.NoVoucher + "</td></tr><tr><td>Resources.Card:</td><td>" + paySummary.NoTarjeta + "</td></tr><tr><td>Resources.AuthorizationNumber:</td><td>" + paySummary.NoAutorizacion + "</td></tr></table><br>";
    //Cheque
    if (rowObject.TipoPagoId == $("#ViewBagParamPaymentMethodCurrentCheck").val() || rowObject.TipoPagoId == $("ViewBagParamPaymentMethodPostdatedCheck").val())
        table = "<table border = 1><tr><td>Resources.DocumentNumber:</td><td>" + paySummary.NoDocumento + "</td></tr><tr><td>Resources.IssuingBank:</td><td>" + paySummary.BancoEmisor + "</td></tr><tr><td>Resources.ReceivingBank:</td><td>" + paySummary.Fecha + "</td></tr></table><br>";
    //Debito
    if (rowObject.TipoPagoId == $("#ViewBagParamPaymentMethodDebit").val())
        table = "<table border = 1><tr><td>Resources.DocumentNumber:</td><td>" + paySummary.NoDocumento + "</td></tr><tr><td>Resources.IssuingBank:</td><td>" + paySummary.BancoEmisor + "</td></tr><tr><td>Resources.ReceivingBank:</td><td>" + paySummary.Fecha + "</td></tr></table><br>";

    return table;
}

function setCurrency(source, destination) {


    var selectCurrency = $("#" + source).val();
    var textExchangeRate = $("#" + destination);

    if (selectCurrency != null) {
        var response;
        response = getCurrencyRateBilling($("#ViewBagDateAccounting").val(), selectCurrency);
        if (response[0] != "") {
            textExchangeRate.val(response[0]);
            textExchangeRate.val(FormatCurrency(response[0]));
            textExchangeRate.blur();
        } else {
            textExchangeRate.val("");
        }

        if (response[1] == false) {
            $("#alert").UifAlert('show', Resources.ExchageRateNotUpToDate, 'warning');
        }

    } else {
        textExchangeRate.val("");
    }
}

//Valida tasas de cambio
function setCurrencyNoMsg(ddlIn, txtOut) {
    var ddlCurrency = $("#" + ddlIn).val();
    var txtExchangeRate = $("#" + txtOut);

    if (ddlCurrency != "") {
        var resp;
        resp = getCurrencyRateBilling($("#ViewBagDateAccounting").val(), ddlCurrency);
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
function getCurrencyRateBilling(accountingDate, currencyId) {
    var alert = true;
    var rate;
    var resp = new Array();

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

function setCurrencyCombo(localCurrencyId) {

    $("#CashCurrencySelect").val(localCurrencyId);

}

//Elimina una fila del grid Resumen de Pagos
function deleteRowCheckRegularization(selectedRow) {
    $("#alert").UifAlert('hide');
    if (selectedRow != null) {
        totAmount = totAmount - parseFloat($.trim(ClearFormatCurrency(selectedRow.LocalAmount)));

        $("#Difference").val(parseFloat($.trim(ClearFormatCurrency($("#TotalAmount").val().replace("", ",")))) - totAmount);
        $("#Difference").val($.trim(FormatCurrency($("#Difference").val())));

        var currencyId = selectedRow.CurrencyId;
        if (currencyId == localCurrencyId) {
            amountLocalCurrency = amountLocalCurrency - parseFloat($.trim(ClearFormatCurrency(selectedRow.Amount)));
            $("#TotalCurrency").val(amountLocalCurrency);
            $("#TotalCurrency").trigger('blur');
        }
        if (currencyId != localCurrencyId) {
            amountForeingCurrency = amountForeingCurrency - parseFloat($.trim(ClearFormatCurrency(selectedRow.Amount)));
            amountLocalCurrency = amountLocalCurrency - parseFloat($.trim(ClearFormatCurrency(selectedRow.LocalAmount)));
            $("#TotalExchangeRate").val(amountForeingCurrency);
            $("#TotalExchangeRate").trigger('blur');
            $("#TotalCurrency").val(amountLocalCurrency);
            $("#TotalCurrency").trigger('blur');
        }

    } else {

        $("#alert").UifAlert('show', Resources.SelectedRecords, 'warning');
    }
}

//$(document).ready(function () {


//FORMATOS/#CARACTERES
$("#TotalAmount").attr("maxlength", 18);
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


setCurrencyCombo($("#ViewBagLocalCurrencyId").val());
//Efectivo por default
setCurrency("CashCurrencySelect", "CashExchangeRate");
clearPaymentMethodFields();

$("#divCash").hide();
$("#divCheck").hide();
$("#divCreditCard").hide();
$("#divTransfer").hide();
$("#divDepositVoucher").hide();
$("#divRetentionReceipt").hide();


$("#CashAmountRegularization").blur(function () {

    $("#alert").UifAlert('hide');
    if ($.trim($("#CashAmountRegularization").val()) == "$") {
        $("#CashAmountRegularization").val('');
    } else
        $("#CashAmountRegularization").val($.trim(ClearFormatCurrency($("#CashAmountRegularization").val())));

    setCurrencyNoMsg("CashCurrencySelect", "CashExchangeRate");
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

    setCurrencyNoMsg("CheckCurrencySelect", "CheckExchangeRate");
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
    setCurrencyNoMsg("CreditCardCurrencySelect", "CreditCardExchangeRate");
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

    setCurrencyNoMsg("TransferCurrencySelect", "TransferExchangeRate");
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

    setCurrencyNoMsg("DepositVoucherCurrencySelect", "DepositVoucherExchangeRate");
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

    setCurrencyNoMsg("RetentionReceiptCurrencySelet", "RetentionReceiptExchangeRate");
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

    setCurrencyNoMsg("TransferCurrencyAreaSlect", "TransferExchangeRateArea");
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

    setCurrencyNoMsg("CreditCardCurrencyPaymentSelet", "PaymentCreditCardExchangeRate");
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



//Valida que no ingresen una fecha invalida en recivo de retención.
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
        validPostDateDate();
        $("#_accept").focus();
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


$("#CashLocalAmount").trigger('blur');
$("#CheckLocalAmount").trigger('blur');
$("#CreditCardLocalAmount").trigger('blur');
$("#TransferLocalAmount").trigger('blur');
$("#DepositVoucherLocalAmount").trigger('blur');
$("#RetentionReceiptLocalAmount").trigger('blur');


//Accion para ejecutar ocultado de paneles de tipos de pago.
$("#PaymentMethodType").on('itemSelected', function (event, selectedItem) {

    clearPaymentMethodFields();

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
            //case cambio para BE
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

//});//fin del dom

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
    validDepositVoucherDocumentNumber();
});

//Valida si un número de cheque para un determinado Banco ya ha sido registrado
$("#CheckDocumentNumber").blur(function () {
    validCheckDocumentNumberCheckRegularization();
});

//Valida si un número de cheque para un determinado Banco ya ha sido registrado
$("#TransferIssuingAccountNumber").blur(function () {
    validTransferDocumentNumber();
});

//Valida si un número de voucher para un determinado Número de Tarjeta ya ha sido registrado
$("#CreditCardVoucherNumber").blur(function () {
    validCreditCardVoucherNumber();
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
    IssuingBankId = selectedItem.Id;
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
        setCurrency("CheckCurrencySelect", "CheckExchangeRate");
        $("#CheckAmount").trigger('blur');
    } else {
        $('#CheckExchangeRate').val("");
    }
});


//Tipo de moneda en pago con transferencia
$('#TransferCurrencySelect').change(function () {
    if ($('#CheckCurrencySelect').val() != "") {
        setCurrency("TransferCurrencySelect", "TransferExchangeRate");
        $("#TransferAmount").trigger('blur');
    } else {
        $('#TransferExchangeRate').val("");
    }
});



//Tipo de moneda en pago con tarjeta
$('#CreditCardCurrencySelect').change(function () {
    if ($('#CreditCardCurrencySelect').val() != "") {
        setCurrency("CreditCardCurrencySelect", "CreditCardExchangeRate");
        $("#CreditCardAmount").trigger('blur');
    } else {
        $('#CreditCardExchangeRate').val("");
    }
});

//Tipo de moneda en Pago efectivo
$('#CashCurrencySelect').change(function () {
    $("#alert").UifAlert('hide');
    if ($('#CashCurrencySelect').val() != "") {
        setCurrency("CashCurrencySelect", "CashExchangeRate");
        $("#CashAmountRegularization").trigger('blur');
    } else {
        $('#CashExchangeRate').val("");
    }
});


//Tipo de moneda en pago con voleta de deposito
$('#DepositVoucherCurrencySelect').change(function () {
    if ($('#DepositVoucherCurrencySelect').val() != "") {
        setCurrency("DepositVoucherCurrencySelect", "DepositVoucherExchangeRate");
        $("#DepositVoucherAmount").trigger('blur');
    } else {
        $('#DepositVoucherExchangeRate').val("");
    }
});


//Tipo de moneda en pago con voleta de retención
$('#RetentionReceiptCurrencySelet').change(function () {
    if ($('#RetentionReceiptCurrencySelet').val() != "") {
        setCurrency("RetentionReceiptCurrencySelet", "RetentionReceiptExchangeRate");
        $("#RetentionReceiptAmount").trigger('blur');
    } else {
        $('#RetentionReceiptExchangeRate').val("");
    }
});


//Tipo de moneda en pago  de Area de pagos
$('#TransferCurrencyAreaSlect').change(function () {
    if ($('#TransferCurrencyAreaSlect').val() != "") {
        setCurrency("TransferCurrencyAreaSlect", "TransferExchangeRateArea");
        $("#TransferAmountArea").trigger('blur');
    } else {
        $('#TransferExchangeRateArea').val("");
    }
});


//Tipo de moneda en pago con tarjeta acreditable
$('#CreditCardCurrencyPaymentSelet').change(function () {
    if ($('#CreditCardCurrencyPaymentSelet').val() != "") {
        setCurrency("CreditCardCurrencyPaymentSelet", "PaymentCreditCardExchangeRate");
        $("#PaymentCreditCardAmount").trigger('blur');
    } else {
        $('#PaymentCreditCardExchangeRate').val("");
    }
});

$("#CreditCardValidThruYear").blur(function () {
    checkExpirationDate();
});

$("#PaymentCreditCardValidThruYear").blur(function () {
});

$("#PaymentCreditCardNumber").blur(function () {
    $("#PaymentCreditCardAuthorizationNumber").val("1458756");
    $("#PaymentCreditCardVoucherNumber").val("754156");
});

$("#CreditCardValidThruMonthSelect").change(function () {
    if ($("#CreditCardValidThruYear").val() != "")
        checkExpirationDate();
});

$("#PaymentCreditCardValidThruMonth").change(function () {
    if ($("#PaymentCreditCardValidThruYear").val() != "") {
        
    }
});


/*************************************************************************************************************/
//FUNCIONALIDAD BOTONES DE MODALES
$(document).ready(function () {

    // Botón Aceptar - Cierre Caja
    $("#modalBillingClosureCheck").find('#AcceptClosure').click(function () {
        $("#modalBillingClosureCheck").hide();

        location.href = $("#ViewBagBillingClosureIdLink").val() + "?billControlId=" + idBillControl +
                        "&branchId=" + $("#SelectBranchCheckRegularization").val();
    });

    // Botón Aceptar - Apertura Caja
    $("#modalOpeningBilling").find('#AcceptOpening').click(function () {
        $("#modalOpeningBilling").modal('hide');

        if ($("#SelectBranchCheckRegularization").val() != undefined && $("#SelectBranchCheckRegularization").val() != "") {
            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "Billing/SaveBillControl",
                data: {
                    "branchId": $("#SelectBranchCheckRegularization").val(),
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

    // Botón Imprimir Detalle
    $("#modalSaveBill").find('#PrintDetail').click(function () {

        ShowReceiptReport(branchId, $("#modalSaveBill").find("#BillingId").val(), "");

        $('#modalSaveBill').UifModal('hide');
        location.href = $("#ViewBagMainCheckSearchLink").val(); 
    });

    // Botón Imprimir Detalle
    $("#modalSaveBill").find('#CloseDetail').click(function () {
        $('#modalSaveBill').UifModal('hide');
        location.href = $("#ViewBagMainCheckSearchLink").val();
    });

    
    //$("#AccountNumberReceivingBankSelect").UifSelect({ source: data });

    $("#PaymentMethodType").val($("#ViewBagParamPaymentMethodCash").val());
    $("#PaymentMethodType").trigger('change');

    if ($("#ViewBagCheckRegularization").val() == "true") {
        $("#divCash").show();

        //Muestra para cheques
        $("#_accept").show();
        $("#SelectBranchCheckRegularization").show();

        //Muestra para tarjetas
        $("#AcceptCardRegularization").hide();
        $("#SelectBranchCardRegularization").hide();
    }
    //CARGA DATA 
    loadCheckRegularization();
    getDateCheckRegularization();

});