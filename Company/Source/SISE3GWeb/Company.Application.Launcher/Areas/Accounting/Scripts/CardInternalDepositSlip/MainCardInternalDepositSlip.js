/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//  DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
   
var accountCurrencyVariable = -1;
var issuingBankVariable;
var bankId = null;

var oInternalCardDepositChecksTable = {
    PaymentTicket: []
};

var oPaymentTicketCardInternal = {
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
    CurrencyDesc: null,
    Amount: null,
    CommissionAmount: null,
    PaymentId: null,
    Select: null,
    CardNumber:null
};

var firstEntry = 0;
var ccardEntry;
var branchEntry;
var dateEntry;
var bankEntry;
var accountEntry;
var creditableCreditCard = 0;
var ncreditableCreditCard = 0;

setTimeout(function () {
    //PARA QUE CARGUE EL VALOR RECUPERADO DEL VIEWBAG
    creditableCreditCard = $("#ViewBagCreditable").val();    // EE--> 4  BB --> -1
    ncreditableCreditCard = $("#ViewBagNoCreditable").val(); // EE--> 5  BB --> 0
}, 2000);

var validation;
var errorNumber = 0;
var resourcesGlobal = "";
//Funciones Globales
var ticketCodeVariable = -1;
var methodTypeCodeVariable = -1;


/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#Branch").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#Branch").removeAttr("disabled");
}


$('#IssuingBank').on('itemSelected', function (event, selectedItem) {
    issuingBankVariable = selectedItem.bankId;
});

$('#CardsSelect').on('rowDelete', function (event, data, position) {
    index = position;
    $.UifDialog('confirm', {'message': Resources.DeleteBallot,
                            'title':   Resources.InternalBallotDepositCardConfirmationTitle
    }, function (result) {
        if (result) {
            $('#CardsSelect').UifDataTable('deleteRow', index);
        }
    });
});



$('#ReceivingBankCardInternal').on('itemSelected', function (event, selectedItem) {
    
    $("#alert").UifAlert('hide');
    $("#alertInternalDeposit").UifAlert('hide');
    
    if ($('#ReceivingBankCardInternal').val() !="") {
        if (firstEntry > 0) {
            showConfirmCardInternalBallot(4, /*bankEntry*/ $('#ReceivingBankCardInternal').val());
        }
        else {
            bankEntry = $("#ReceivingBankCardInternal").val();           

            if ($("#Branch").val() != "") {
                var controller = ACC_ROOT + "CheckControl/GetAccountByBankIdByBranchId?bankId=" + selectedItem.Id + '&branchId=' + $("#Branch").val();
                $("#ReceivingAccountNumber").UifSelect({ source: controller });
            } else {
                $("#InternalBallot").valid();
            } 
        }
    }
    else {
        $("#ReceivingAccountNumber").UifSelect();
    }
});

//Restringe dias mayores al actual

//Valida que no se ingrese fecha mayor al del sistema
$('#DateInternalDeposit').on("dateChanged.datepicker", function (event, date) {
    $("#alert").UifAlert('hide');
    $("#alertInternalDeposit").UifAlert('hide');

    if ($("#DateInternalDeposit").val() != '') {
        var dateEntry = $("#DateInternalDeposit").val();
        if (IsDate($("#DateInternalDeposit").val()) == true) {
            if (CompareDates($("#DateInternalDeposit").val(), getCurrentDate()) == 2) {
                $("#alertInternalDeposit").UifAlert('show', Resources.SystemdateValidation, "warning");
                $("#DateInternalDeposit").val("");
            }
            else {
                if (firstEntry > 0) {
                    showConfirmCardInternalBallot(3, dateEntry);
                }
            }
        }
        else {
            $("#alertInternalDeposit").UifAlert('show', Resources.InvalidDates, "warning");
        }
    }
});


$("#cardTypeA").click(function () {
    if (firstEntry > 0) {
        showConfirmCardInternalBallot(0, creditableCreditCard); //Esta seguro de eliminar to do lo procesado?
    }
    else {
        $("#ReceivingBankCardInternal").val("");
        $("#ReceivingAccountNumber").val("");
        $("#ReceivingBankCardInternal").removeAttr("disabled");
        $("#ReceivingAccountNumber").removeAttr("disabled");
        $("#AccountCurrency").val('');

        oInternalCardDepositChecksTable = {
            PaymentTicket: []
        };
    }
});


$("#cardTypeB").click(function () {
    if (firstEntry > 0) {
        showConfirmCardInternalBallot(0, ncreditableCreditCard);
    }
    else {
        $("#ReceivingBankCardInternal").val("");
        $("#ReceivingAccountNumber").val("");

        $("#ReceivingBankCardInternal").attr("disabled", "disabled");
        $("#ReceivingAccountNumber").attr("disabled", "disabled");
        $("#AccountCurrency").val("");
        $("#CardAmount").val("");
        
        oInternalCardDepositChecksTable = {
            PaymentTicket: []
        };
    }
});


$('#ReceivingAccountNumber').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    $("#alertInternalDeposit").UifAlert('hide');
        
    if ($('#ReceivingAccountNumber').val() != "") {
        accountEntry = $("#ReceivingAccountNumber option:selected").text();

        $.ajax({
            url: ACC_ROOT + "CheckInternalDepositSlip/GetAccountCurrencyByBankId",
            data: { "bankId": $("#ReceivingBankCardInternal").val(), "accountNumber": $("#ReceivingAccountNumber option:selected").html() },
            success: function (data) {
                $("#AccountCurrency").val(data.Currency.Description);
                accountCurrencyVariable = data.Currency.Id;
            }
        });
        setDataFieldsEmpty();
        $('#CardAmount').val('');
        $('#CommissionAmount').val('');
        cleanObjectCardInternal();
    }
});

$('#CreditCardType').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    $("#alertInternalDeposit").UifAlert('hide');

    if (firstEntry > 0) {
        showConfirmCardInternalBallot(1, ccardEntry);
    }
    else
        ccardEntry = $("#CreditCardType").val();
});

$('#Branch').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    $("#alertInternalDeposit").UifAlert('hide');
    var controller = ACC_ROOT + "CheckControl/GetBankBranchsByBranchId?branchId=" + $("#Branch").val();
    $("#ReceivingBankCardInternal").UifSelect({ source: controller });
    if (firstEntry > 0) {
        showConfirmCardInternalBallot(2, /*branchEntry*/$("#Branch").val());
    }
    else {
        branchEntry = $("#Branch").val();
    }
});




$("#SaveTicketCardInternal").click(function () {
    $("#alert").UifAlert('hide');
    $("#alertInternalDeposit").UifAlert('hide');

    if (ValidateFieldsCardInternal() == true) {
        if (errorNumber == 0 || errorNumber == undefined) {
            validation = 0;
        }
        else {
            validation = 1;
            errorNumber = 0;
        }

        if (validation != 0) {
            $("#alertInternalDeposit").UifAlert('show', Resources.ValidateCash, 'warning');
        }
        else {
            lockScreen();
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "CheckInternalDepositSlip/SaveInternalBallotDeposit",
                    data: JSON.stringify({ "tblChecksModel": InternalCardDepositSetTicket() }),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.success == false) {
                            $("#alertInternalDeposit").UifAlert('show', data.result, "danger");
                        }
                        else {

                            var branchName = $("#Branch option:selected").html();
                            $("#branchNameSuccessModal").val(branchName);
                            ticketCodeVariable = data[0].Id;
                            methodTypeCodeVariable = data[0].MethodType;

                            var ballotNumber = ("000000000" + data[0].Id).slice(-10);
                            $("#paymentTicketItemIdSuccessModal").val(ballotNumber);

                            var reportType = $('input:radio[name=options]:checked').val();
                            if (reportType == "C") {
                                var issuingBankName = $("#ReceivingBankCardInternal option:selected").html();
                                $("#receivingBankNameSuccessModal").val(issuingBankName);
                                //var accountNumber = $("#ReceivingAccountNumber option:selected").html();
                                $("#accountBankSuccessModal").val(oPaymentTicketCardInternal.CardNumber);
                                //var accountCurrency = $("#AccountCurrency").val();
                                $("#accountCurrencySuccessModal").val(oPaymentTicketCardInternal.CurrencyDesc);
                            }
                            else {
                                $("#receivingBankNameSuccessModal").val("");
                                $("#accountBankSuccessModal").val("");
                                $("#accountCurrencySuccessModal").val("");
                            }

                            $("#dateBallotSuccessModal").val(data[0].Date);
                            $("#userBallotSuccessModal").val(data[0].User);

                            var cardAmount = null;
                            if ($("#CardAmount").val() == "")
                                cardAmount = "0";
                            else
                                cardAmount = $("#CardAmount").val();

                            $("#totalCardsSuccessModal").val(data[0].Total);
                            var totalCards = $("#totalCardsSuccessModal").val();
                            $("#totalCardsSuccessModal").val(FormatCurrency(FormatDecimal(totalCards)));

                            var totalBallot = parseFloat(data[0].Total);
                            /*$("#totalBallotSuccessModal").val(totalBallot);
                            totalBallot = $("#totalBallotSuccessModal").val();*/
                            $("#totalBallotSuccessModal").val(FormatCurrency(FormatDecimal(totalBallot)));
                            $("#ModalCardInternalSave").UifModal('showLocal', Resources.InternalSlipSuccessfulSaved);

                        }

                        unlockScreen();
                        setDataFieldsAEmpty();
                        oInternalCardDepositChecksTable = {
                            PaymentTicket: []
                        };
                        firstEntry = 0;
                    }
                });
            }, 1000);
        }
    }
    else {
        $("#alertInternalDeposit").UifAlert('show', Resources.InternalBallotCardValidateSaveMessage, 'warning');
    }
});


$("#CancelTicket").click(function () {
    $("#alert").UifAlert('hide');
    $("#alertInternalDeposit").UifAlert('hide');

    setDataFieldsAEmpty();
    cleanObjectCardInternal();
});

$("#CleanInternalDeposit").click(function () {
    $("#alert").UifAlert('hide');
    $("#alertInternalDeposit").UifAlert('hide');

    setDataFieldsVaEmpty();
    setDataFieldsEmpty();
    cleanObjectCardInternal();
    firstEntry = 0;
});

$("#SearchIntenalDeposit").click(function () {
    $("#alert").UifAlert('hide');
    $("#alertInternalDeposit").UifAlert('hide');

    $("#InternalBallot").validate();

    if ($("#InternalBallot").valid())
    {
        if ($("#DateInternalDeposit").val() == "") {
            errorNumber = 1;
            resourcesGlobal = Resources.DateRequired;
        }
        else
        {
            if (IsDate($("#DateInternalDeposit").val()) == false) {
                errorNumber = 1;
                resourcesGlobal = Resources.InvalidDates;
            }
        }

        if (errorNumber == 0 || errorNumber == undefined)
        {
            validation = 0;
        }
        else
        {
            validation = 1;
            errorNumber = 0;
        }

        if (validation != 0) {
            $("#alertInternalDeposit").UifAlert('show', 'ResourcesGlobal', "warning"); //validar msj
        }
        else {
            firstEntry = 1;
            accountEntry = $("#ReceivingAccountNumber option:selected").html();
            $("#CardsDeposit").UifDataTable();
            refreshGridCardInternal();
        }
    }
});

$('#CardsDeposit').on('rowAdd', function (event, data) {
    $("#alert").UifAlert('hide');
    $("#alertInternalDeposit").UifAlert('hide');

    var rowidc = $("#CardsDeposit").UifDataTable("getSelected");
    if (rowidc != null) {
        addCardInternal();
    }
    else {
        $("#alertInternalDeposit").UifAlert('show', Resources.InternalBallotDepositCardSaveMessage, 'warning');
    }
});

$('#AddIntenalDepositSlip').hide();

$("#AddIntenalDepositSlip").click(function () {
    $("#alert").UifAlert('hide');
    $("#alertInternalDeposit").UifAlert('hide');

    var rowidc = $("#CardsDeposit").UifDataTable("getSelected");
    if (rowidc != null) {
        addCardInternal();
    }
    else {
        $("#alertInternalDeposit").UifAlert('show', Resources.InternalBallotDepositCardSaveMessage, 'warning');
    }
});

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
function showConfirmCardInternalBallot(id, value) {
    $.UifDialog('confirm', { 'message': Resources.ConfirmFilterMessage + '?', 'title': Resources.FilterChangeBallot },
     function (result) {
        if (result) {
            setDataFieldsVaEmpty();
            if (id == 0 && value == creditableCreditCard) {
                $("#cardTypeA").prop('checked', true);
                $("#ReceivingBankCardInternal").val("");
                $("#ReceivingAccountNumber").val("");
                $("#ReceivingBankCardInternal").removeAttr("disabled");
                $("#ReceivingAccountNumber").removeAttr("disabled");

                $("#AccountCurrency").val("");

                oInternalCardDepositChecksTable = {
                    PaymentTicket: []
                };
            }
            // Tipo de tarjeta no acreditable
            if (id == 0 && value == ncreditableCreditCard) {
                $("#cardTypeB").prop('checked', true);
                $("#ReceivingBankCardInternal").val("");
                $("#ReceivingAccountNumber").val("");
                $("#ReceivingBankCardInternal").attr("disabled", "disabled");
                $("#ReceivingAccountNumber").attr("disabled", "disabled");

                $("#AccountCurrency").val("");
                $("#CardAmount").val("");

                oInternalCardDepositChecksTable = {
                    PaymentTicket: []
                };
            }

            firstEntry = 0;
        }
        //else {
        //    // Tipo de tarjeta acreditable
        //    if (id == 0 && value == ncreditableCreditCard) {
        //        $("#cardTypeA").prop('checked', true);
        //    }
        //    // Tipo de tarjeta no acreditable
        //    else if (id == 0 && value == creditableCreditCard) {
        //        $("#cardTypeB").prop('checked', true);
        //    }

        //    // Conducto
        //    else if (id == 1 && value > -1) {
        //        $("#CreditCardType").val(value);
        //    }
        //    // Sucursal
        //    else if (id == 2 && value > -1) {
        //        $("#Branch").val(value);
        //    }
        //    // Fecha hasta
        //    else if (id == 3) {
        //        $("#DateInternalDeposit").val(value);
        //    }
        //    // Banco receptor
        //    else if (id == 4 && value > -1) {
        //        $("#ReceivingBankCardInternal").val(value);
        //        $("#ReceivingBankCardInternal").trigger('change');
        //    }
        //    // Cuenta banco receptor
        //    else if (id == 5 && value > -1) {
        //        $("#ReceivingAccountNumber option:selected").html(value);
        //    }
        ////}

    });
}


function ShowReceiptReportCardInternalBallot(ticketCodeVariable, methodTypeCodeVariable) {
    var controller = ACC_ROOT + "Report/LoadInternalBallotCardReport?paymentTicketCode=" + ticketCodeVariable +
                                 "&paymentMethodTypeCode=" + methodTypeCodeVariable;
    window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function InternalCardDepositSetTicket() {
    var issuingBankCode = $("#ReceivingBankCardInternal").val();
    var issuingBankName = $("#ReceivingBankCardInternal option:selected").html();
    var accountNumber = $("#ReceivingAccountNumber option:selected").html();
    var cardAmount = $("#CardAmount").val();
    var currencyId = accountCurrencyVariable;
    var paymentMethodTypeCode = 0;

    var reportType = $('input:radio[name=options]:checked').val();


    if (reportType == "C") {
        paymentMethodTypeCode = creditableCreditCard;
    }
    if (reportType == "U") {
        paymentMethodTypeCode = ncreditableCreditCard;
        issuingBankCode = "";
        issuingBankName = "";
        accountNumber = "";
        currencyId = "-1";
    }
   
    var branchCode = $("#Branch").val();
    var branchName = $("#Branch option:selected").html();

    var dataCardsSelect = $("#CardsSelect").UifDataTable("getData");   // getSelected

    if (cardAmount == "" && dataCardsSelect != null) {
        cardAmount = "0";

        for (var i in dataCardsSelect)
        {
            cleanObjectCardInternal();
            
            oPaymentTicketCardInternal.Branch = branchCode;
            oPaymentTicketCardInternal.BranchName = branchName;
            oPaymentTicketCardInternal.Bank = issuingBankCode;
            oPaymentTicketCardInternal.BankDescription = issuingBankName;
            oPaymentTicketCardInternal.Checks = dataCardsSelect[i].CardNumber;
            oPaymentTicketCardInternal.DatePayment = dataCardsSelect[i].CardDate;
            oPaymentTicketCardInternal.PaymentMethodId = paymentMethodTypeCode;
            oPaymentTicketCardInternal.Currency = dataCardsSelect[i].CurrencyCode;
            oPaymentTicketCardInternal.Amount = parseFloat(ClearFormatCurrency(dataCardsSelect[i].Amount.toString().replace("", ",")));
            oPaymentTicketCardInternal.CommissionAmount = parseFloat(ClearFormatCurrency(dataCardsSelect[i].CommissionAmount.toString().replace("", ",")));
            oPaymentTicketCardInternal.PaymentId = dataCardsSelect[i].PaymentCode;
            
            oPaymentTicketCardInternal.AccountNumber = accountNumber;
            oPaymentTicketCardInternal.CashAmount = parseFloat(cardAmount);
            oInternalCardDepositChecksTable.PaymentTicket.push(oPaymentTicketCardInternal);
        }
    }
    else if (cardAmount != "" && dataCardsSelect != null) {
        cardAmount = ClearFormatCurrency(cardAmount.replace("", ","));

        for (var i in dataCardsSelect) {

            cleanObjectCardInternal();
            
            oPaymentTicketCardInternal.Branch = branchCode;
            oPaymentTicketCardInternal.BranchName = branchName;
            oPaymentTicketCardInternal.Bank = issuingBankCode;
            oPaymentTicketCardInternal.BankDescription = issuingBankName;
            oPaymentTicketCardInternal.Checks = dataCardsSelect[i].CardNumber;
            oPaymentTicketCardInternal.DatePayment = dataCardsSelect[i].CardDate;
            oPaymentTicketCardInternal.PaymentMethodId = paymentMethodTypeCode;
            oPaymentTicketCardInternal.Currency = dataCardsSelect[i].CurrencyCode;
            oPaymentTicketCardInternal.CurrencyDesc = dataCardsSelect[i].CurrencyName;
            oPaymentTicketCardInternal.CardNumber = dataCardsSelect[i].CardNumber;
            oPaymentTicketCardInternal.Amount = parseFloat(ClearFormatCurrency(dataCardsSelect[i].Amount.toString().replace("", ",")));
            oPaymentTicketCardInternal.CommissionAmount = parseFloat(ClearFormatCurrency(dataCardsSelect[i].CommissionAmount.toString().replace("", ",")));
            oPaymentTicketCardInternal.PaymentId = dataCardsSelect[i].PaymentCode;
            
            oPaymentTicketCardInternal.AccountNumber = accountNumber;
            oPaymentTicketCardInternal.CashAmount = 0; 
            oInternalCardDepositChecksTable.PaymentTicket.push(oPaymentTicketCardInternal);
        };
    }
    else if (cardAmount != "" && dataCardsSelect == null) {
        cardAmount = ClearFormatCurrency(cardAmount.replace("", ","));

        paymentMethodTypeCode = $("#ViewBagPaymentMethodCash").val();

        cleanObjectCardInternal();
        oPaymentTicketCardInternal.Branch = branchCode;
        oPaymentTicketCardInternal.BranchName = branchName;
        oPaymentTicketCardInternal.Bank = issuingBankCode;
        oPaymentTicketCardInternal.BankDescription = issuingBankName;
        oPaymentTicketCardInternal.PaymentMethodId = paymentMethodTypeCode;
        oPaymentTicketCardInternal.Currency = currencyId;
        oPaymentTicketCardInternal.AccountNumber = accountNumber;
        oPaymentTicketCardInternal.CashAmount = parseFloat(cardAmount);

        oInternalCardDepositChecksTable.PaymentTicket.push(oPaymentTicketCardInternal);
    }

    return oInternalCardDepositChecksTable;
}

function setDataFieldsEmpty() {
    
    $("#IssuingBank").val("");
    issuingBankVariable = -1;
    $("#VoucherNumber").val("");
    $("#CardsDeposit").dataTable().fnClearTable();
    $("#CardsSelect").dataTable().fnClearTable();
}

function setDataFieldsAEmpty() {
    $("#CreditCardType").val("");
    $("#Branch").val("");
    $("#DateInternalDeposit").val("");
    $("#CardAmount").val("");
    $("#CommissionAmount").val("");
    $("#VoucherNumber").val("");
    $("#IssuingBank").val("");
    issuingBankVariable = -1;
    $("#ReceivingBankCardInternal").val("");
    $("#ReceivingAccountNumber").val("");

    $("#ReceivingAccountNumber").UifSelect();

    $("#AccountCurrency").val("");
    accountCurrencyVariable = -1;
    $("#CardsDeposit").dataTable().fnClearTable();
    $("#CardsSelect").dataTable().fnClearTable();

    $("#ReceivingBankAutocomplete").val("");
    $("#ReceivingBankCardInternal").val("");
    $("#CardAmount").val("");


    $("#cardTypeA").prop('checked', true);
    $("#ReceivingBankCardInternal").removeAttr("disabled");
    $("#ReceivingAccountNumber").removeAttr("disabled");
}

function setDataFieldsVaEmpty() {
    $("#CreditCardType").val("");
    $("#Branch").val("");
    $("#DateInternalDeposit").val("");
    $("#CardAmount").val("");
    $("#CommissionAmount").val("");
    $("#VoucherNumber").val("");
    $("#IssuingBank").val("");
    issuingBankVariable = -1;
    $("#ReceivingBankCardInternal").val("");
    $("#ReceivingAccountNumber").val("");
    $("#AccountCurrency").val("");
    accountCurrencyVariable = -1;
    $("#CardsDeposit").dataTable().fnClearTable();
    $("#CardsSelect").dataTable().fnClearTable();

    $("#ReceivingBankAutocomplete").val("");
    //receivingBankVariable = -1;
    $("#CardAmount").val("");
    $("#ReceivingBankCardInternal").removeAttr("disabled");
    $("#ReceivingAccountNumber").removeAttr("disabled");
    $("#cardTypeA").prop('checked', true);
}

function cleanObjectCardInternal() {
    oPaymentTicketCardInternal = {
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
        Select: null
    };
    //ids = 0;
    bankId = null;
}


function refreshGridCardInternal() {
    var paymentMethodTypeCode = null;
    var reportType = $('input:radio[name=options]:checked').val();
    if (reportType == "C") {
        paymentMethodTypeCode = creditableCreditCard;
    }
    else {
        paymentMethodTypeCode = ncreditableCreditCard;
    }

    var issuingBankCode = issuingBankVariable;
    var voucherNumber = $("#VoucherNumber").val();
    var currencyCode = accountCurrencyVariable;
    var creditCardType = $("#CreditCardType").val();
    var branchCode = $("#Branch").val();
    var cardDate = $("#DateInternalDeposit").val();

    if (issuingBankCode == undefined) {
        issuingBankCode = "";
    }
        
    var controller = ACC_ROOT + "CardInternalDepositSlip/GetCardsToDepositBallot?paymentMethodTypeCode=" + paymentMethodTypeCode +
                              "&creditCardType=" + creditCardType + "&branchCode=" + branchCode + "&cardDate=" + cardDate +
        "&currencyCode=" + currencyCode + "&issuingBankCode=" + issuingBankCode + "&voucherNumber=" + voucherNumber;
    $("#CardsDeposit").UifDataTable({ source: controller });
}

//Agrega tarjetas a depositar a tarjetas seleccionadas
function addCardInternal() {
 
    var dataCardsDeposit = $("#CardsDeposit").UifDataTable("getSelected");
    var totalCards = 0;
    var totalCommission = 0;

    if (dataCardsDeposit != null) {
        for (var i in dataCardsDeposit) {
            if (DuplicateRecords(dataCardsDeposit[i].PaymentCode) == false) {
                $('#CardsSelect').UifDataTable('addRow', dataCardsDeposit[i]);

                var valueCards = parseFloat(ClearFormatCurrency(dataCardsDeposit[i].Amount.toString().replace("", ",")));
                if ($("#CardAmount").val() == "")
                    totalCards = 0;
                else
                    totalCards = parseFloat(ClearFormatCurrency($("#CardAmount").val().replace("", ",")));

                totalCards += valueCards;

                $("#CardAmount").val(totalCards);
                var cardAmount = $("#CardAmount").val();
                $("#CardAmount").val(FormatCurrency(FormatDecimal(cardAmount)));

                var valueCommission = parseFloat(ClearFormatCurrency(dataCardsDeposit[i].CommissionAmount.toString().replace("", ",")));
                if ($("#CommissionAmount").val() == "")
                    totalCommission = 0;
                else
                    totalCommission = parseFloat(ClearFormatCurrency($("#CommissionAmount").val().replace("", ",")));

                totalCommission += valueCommission;

                $("#CommissionAmount").val(FormatCurrency(FormatDecimal(totalCommission)));
            }
        }
    }
}

//Validar que no se agregue duplicados en tarjetas seleccionadas
function DuplicateRecords(paymentCode) {
    var result = false;
    var dataCardsSelect = $("#CardsSelect").UifDataTable("getData");

    if (dataCardsSelect != null) {
        for (var f in dataCardsSelect) {
            if (paymentCode == dataCardsSelect[f].PaymentCode) {
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

function ValidateFieldsCardInternal() {
    var result = false;
    var reportType = $('input:radio[name=options]:checked').val();

    var dataCardsSelect = $("#CardsSelect").UifDataTable("getData");

    if (reportType == "C") {
        if ($("#ReceivingAccountNumber").val() == "") {
            result = false;
        }
        else {
            if ($("#CardAmount").val() == "" && dataCardsSelect == null) {
                result = false;
            }
            else {
                    
                if ($("#CardAmount").val() != "" && dataCardsSelect != null)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
        }
    }
    else {
        if ($("#CardAmount").val() == "" && dataCardsSelect == null) {
            result = false;
        }
        else {
                
            if ($("#CardAmount").val() != "" && dataCardsSelect != null) {
                result = true;
            }
            else {
                result = false;
            }
        }
    }

    return result;
}

function validateCashAmountAdmitted() {
    var cardAmount = "";

    cardAmount = ClearFormatCurrency($("#CardAmount").val());

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "CheckInternalDepositSlip/ValidateCashAmount",
        data: { "currencyId": accountCurrencyVariable, "cashAmountAdmitted": cardAmount, "registerDate": "", "paymentTicketId": 0 },
        success: function (data) {
            if (data > 0) {
                errorNumber = 1;
                $("#alertInternalDeposit").UifAlert('show', Resources.ValidateCash, 'warning');
            }
            else {
                errorNumber = 0;
            }
        }
    });
}

/**********************************************************************************************************/
//ACCIONES DE BOTONES DE MODALES
$(document).ready(function () {

    $("#PrintReportSuccessModalCardInternal").click(function () {
        $('#ModalCardInternalSave').modal('hide');
        ShowReceiptReportCardInternalBallot(ticketCodeVariable, methodTypeCodeVariable);
    });

    $("#Branch").UifSelect('setSelected', $("#Branch").UifSelect('getSelected'));
    $("#Branch").trigger('change');
});