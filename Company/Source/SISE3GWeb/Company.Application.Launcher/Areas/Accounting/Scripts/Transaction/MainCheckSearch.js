/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                               DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                                                 */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
var IssuingBankIdCheckSearch = -1;
var bankId = -1;
var checkNumber = -1;
var paymentCode;
var branchId = -1;
// Exchange Check
var errorNumber = 0;
var Alert = 0;
var params = {};
var IssuingNewBankId = 0;
var PaymentMethodTypeCode = 0;
var checkValid = true;
var ExchangeRate = 0;
var PaymentId = -1;
var BillId = -1;
var payerId = -1;
var currencyCode = 0;

var currencyName = "";
var checkDate = 0;
var issuingBankO = 0;
var issuingBankNameO = 0;
var issuingAccountBankO = 0;
var amountOriginal = 0;
var currentStatusName = "";
var incomeReceiptNumberO = 0;
var paymentStatusO = 0;
var branchCodeO = 0;
var branchNameO = "";
var holderCheckO = 0;
var exchangeRateO = 0;
var paymentMethodTypeCodeO = 0;
var cancelClick = 0;

var PaymentSummaryModel = {
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
    IssuingBankId: 0,
    IssuingAccountNumber: null,
    IssuerName: null,
    Date: null
};
// End Exchange Check

// Legalize Check
var legalizeIssuingBankId = 0;
var rejectedPaymentId = 0;
var paymentId = 0;

var oLegalPaymentModel = {
    LegalPaymentId: 0,
    RejectedPaymentId: 0,
    LegalDate: null,
    PaymentId: 0,
    BillId: 0,
    DatePayment: null,
    DocumentNumber: null,
    IssuerName: null,
    IssuingBankId: 0,
    IssuingAccountNumber: null
};
// End Legalize Check


/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                    FUNCIONES GLOBALES                                                                                    */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#Branches").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#Branches").removeAttr("disabled");
}

// Exchange Check

//Llenado del objeto
function setDataPayment() {

    PaymentSummaryModel = {
        PaymentId: 0,
        BillId: 0,
        PaymentMethodId: 0,
        Amount: 0,
        CurrencyId: 0,
        LocalAmount: 0,
        ExchangeRate: 0,
        BranchId:0,
        CheckPayments: []
    };

    PaymentId = $("#paymentCode").val();
    ExchangeRate = $("#OriginalExchangeRate").val();
    BillId = $("#IncomeReceiptNumberOriginal").val();
    PaymentMethodTypeCode = $("#PaymentMethodTypeCode").val();

    PaymentSummaryModel.PaymentId = PaymentId;
    PaymentSummaryModel.BillId = BillId;
    PaymentSummaryModel.PaymentMethodId = PaymentMethodTypeCode;
    PaymentSummaryModel.Amount = RemoveFormatMoney($("#CheckNewAmount").val());
    PaymentSummaryModel.CurrencyId = $("#CurrencyCodeOriginal").val();
    PaymentSummaryModel.ExchangeRate = ExchangeRate;
    PaymentSummaryModel.BranchId = branchId;    

    oCheckModel = {
        DocumentNumber: null,
        IssuingBankId: 0,
        IssuingAccountNumber: null,
        IssuerName: null,
        Date: null
    };

    oCheckModel.DocumentNumber = $("#CheckNewNumber").val();
    oCheckModel.IssuingBankId = IssuingNewBankId;
    oCheckModel.IssuingAccountNumber = $("#CheckNewAccountNumber").val();
    oCheckModel.IssuerName = $("#CheckNewEmisor").val();
    oCheckModel.Date = $("#CheckNewDate").val();

    PaymentSummaryModel.CheckPayments.push(oCheckModel);
}

//Vaciado del Objeto
function setDataPaymentEmpty() {

    PaymentSummaryModel = {
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
        IssuingBankId: 0,
        Date: null,
        Amount: 0
    };
}

function validCheckDocumentNumber(idBank, checkNumber, accountNumber) {
    var result = false;
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/ValidateCheckBankOrTransfer",
        data: { "bankId": idBank, "numberDoc": checkNumber, "accountNumber": accountNumber },
        success: function (data) {
            if (data > 0) {
                result = false;
            } else {
                result = true;
            }
        }
    });

    return result;
}

// End Exchange Check

// Legalize Check
function setDataLegalPayment() {
    paymentId = $("#LegalizePaymentCode").val();
    PaymentId = $("#LegalizePaymentCode").val();
    BillId = $("#LegalizeBillCode").val();
    PaymentMethodTypeCode = $("#PaymentMethodTypeCode").val();

    oLegalPaymentModel.LegalPaymentId = 0;
    oLegalPaymentModel.RejectedPaymentId = rejectedPaymentId;
    oLegalPaymentModel.LegalDate = $("#CheckLegalTransferDate").val();
    oLegalPaymentModel.PaymentId = paymentId;
    oLegalPaymentModel.BillId = BillId;
    oLegalPaymentModel.DatePayment = $("#LegalizeCheckDate").val();
    oLegalPaymentModel.DocumentNumber = $("#LegalizeCheckNumber").val();
    oLegalPaymentModel.IssuerName = $("#checkIssuer").val();
    oLegalPaymentModel.IssuingBankId = $("#LegalizeIssuingBankCode").val();
    oLegalPaymentModel.IssuingAccountNumber = $("#LegalizeIssuingAccountBank").val();

    return oLegalPaymentModel;
}

function refreshTable() {
    var controller = ACC_ROOT + "Transaction/GetChecksUpdated?bankCode=" + IssuingBankIdCheckSearch + "&checkNumber=" + $('#CheckNumber').val()
                     + "&accountNumber=" + $('#AccountNumber').val() + "&technicalTransaction=" + $('#TechnicalTransaction').val() + "&branchCode=" + $('#Branches').val();
    $("#searchCheck").UifDataTable({ source: controller });
}

// End Legalize Check

/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                                          ACCIONES / EVENTOS                                                              */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/

$("#SearchModal").hide();
$("#Exchange").hide();
$("#Reject").hide();
$("#Legalize").hide();
$("#Regularize").hide();
$("#CheckSearchExcelExport").hide();

$("#CheckNewEmisor").blur(function () {
    $("#CheckNewEmisor").val($("#CheckNewEmisor").val().toUpperCase());
});

$("#RejectionDate").val(getCurrentDate());

// Boton que llama a la modal de rechazo de cheques
$("#Reject").click(function () {

    var bankId = 0;
    var checkNumber = "";
    var receiptNumber = 0;
    var paymentCode = 0;

    var selRowIds = $("#searchCheck").UifDataTable("getSelected");
    $("#alert").UifAlert('hide');
    for (var i = 0; i < selRowIds.length; i++) {
        bankId = selRowIds[i].IssuingBankCode;
        checkNumber = selRowIds[i].DocumentNumber;
        receiptNumber = selRowIds[i].BillCode;
        paymentCode = selRowIds[i].PaymentCode;
    }
    loadCheckingRejection(bankId, checkNumber, paymentCode);

    $('#RejectionModal').UifModal('showLocal', Resources.Rejection + " " + Resources.Check + " No." + checkNumber);
});

// Carga datos en la modal de rechazo de cheques
function loadCheckingRejection(bank, check, paymentCode) {
    PaymentId = paymentCode;
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "CheckingRejection/GetCheckInformation",
        data: { "BankId": bank, "documentNumber": check, "paymentCode": paymentCode },
        success: function (data) {
            if (data.length > 0) {

                if (data[0].CollectCode > 0) {
                    var controller = ACC_ROOT + "CheckingRejection/GetPoliciesByBillId?billId=" + parseInt(data[0].CollectCode);
                    $("#tblPolicies").UifDataTable({ source: controller });
                }
                $("#CheckDate").val(data[0].DatePayment);
                $("#CurrencyRejection").val(data[0].CurrencyDescription);                
                $("#AmountRejection").val("$ " + FormatCurrencyRejected(data[0].Amount, "2", ".", ","));
                $("#IssuerRejection").val(data[0].Holder);
                $("#ReceipNumber").val(data[0].CollectCode);
                $("#TransactionNumberRejection").val(data[0].TechnicalTransaction);
                var dateRejection = data[0].RegisterDate;
                $("#DepositDateRejection").val(dateRejection.substring(0, 10));
                $("#RejectionDepositBallotNumber").val(data[0].PaymentBallotNumber);
                $("#DepositTransaction").val(data[0].PaymentBallotCode);
                $("#ReceivingAccount").val(data[0].ReceivingAccountNumber);
                $("#ReceiverBank").val(data[0].ReceivingBankName);
                $("#Depositor").val(data[0].Name);
                $("#RejectedIssuingBankName").val(data[0].BankDescription);
                $("#RejectedIssuingAccountBank").val(data[0].IssuingAccountNumber);
                $("#RejectedCheckNumber").val(data[0].DocumentNumber);
                $("#RejectionDate").val(getCurrentDate());
                BillId = data[0].CollectCode;
                payerId = data[0].PayerId;
                PaymentId = data[0].PaymentCode;
                checkNumber = check;
            }
        }
    });
};

// Boton que llama a la modal de regularización de cheques
$("#Regularize").click(function () {

    var paymentId = 0;
    var documentNumber = 0;
    var branchCodeOrigin = 0;

    var selRowIds = $("#searchCheck").UifDataTable("getSelected");
    $("#alert").UifAlert('hide');

    for (var i in selRowIds) {
        paymentId = selRowIds[i].PaymentCode;
        documentNumber = selRowIds[i].DocumentNumber;
        branchCodeOrigin = selRowIds[i].BranchCode;
    }
   
    location.href = $("#ViewBagLoadCheckRegularizationLink").val() + "?paymentId=" + paymentId +
                      "&documentNumber=" + documentNumber + "&branchId=" + branchCodeOrigin + "";

});

// Boton que ejecuta búsqueda de cheques
$("#SearchCheckSearch").click(function () {
    $("#Exchange").hide();
    $("#Reject").hide();
    $("#Legalize").hide();
    $("#Regularize").hide();
    $("#CheckSearchExcelExport").show();

    $("#FrmSearch").validate();
    if ($("#FrmSearch").valid()) {
        $("#searchCheck").dataTable().fnClearTable();
        if ($("#CheckBank").val() != "") {

            var controller = ACC_ROOT + "Transaction/GetChecksUpdated?bankCode=" + IssuingBankIdCheckSearch + "&checkNumber=" + $('#CheckNumber').val()
                             + "&accountNumber=" + $('#AccountNumber').val() + "&technicalTransaction=" + $('#TechnicalTransaction').val() + "&branchCode=" + $('#Branches').val();
            $("#searchCheck").UifDataTable({ source: controller });

        } else {

            $("#searchCheck").dataTable().fnClearTable();
            IssuingBankIdCheckSearch = -1;
        }
    }
});

// Boton que limpia formulario y objetos
$("#CleanCheckSearch").click(function () {
    setDataFieldsEmptyCheckSearch();
});

// Botón de exportación de listado a excel
$("#CheckSearchExcelExport").click(function () {
    var url = ACC_ROOT + "Transaction/ExportListToExcel?bankCode=" + IssuingBankIdCheckSearch + "&checkNumber=" + $('#CheckNumber').val()
        + "&accountNumber=" + $('#AccountNumber').val() + "&technicalTransaction=" + $('#TechnicalTransaction').val() + "&branchCode=" + $('#Branches').val();

    $.get(url, function (data, status) {
        if (data.success) {
            DownloadFile(data.result);
        } else {
            $.UifNotify('show', { 'type': 'danger', 'message': data.result, 'autoclose': true });
        }
    });
});

//Autocomplete
$('#ExchangeDocumentNumber').on('itemSelected', function (event, selectedItem) {
    fillCkeckAutocompletes(selectedItem);
});

//Autocomplete Nombres
$('#ExchangeName').on('itemSelected', function (event, selectedItem) {
    fillCkeckAutocompletes(selectedItem);
});

//Autocomplete Banco
$('#CheckNewBank').on('itemSelected', function (event, selectedItem) {
    $("#CheckNewBank").val(selectedItem.Value);
    IssuingNewBankId = selectedItem.Id;
});

// llena autocompletes de personas
function fillCkeckAutocompletes(selectedItem) {
    $("#NewPayerId").val(selectedItem.Id);
    $("#ExchangeDocumentNumber").UifAutoComplete('setValue', selectedItem.DocumentNumber);
    $("#ExchangeName").UifAutoComplete('setValue', selectedItem.Name);
}

// Boton que llama a la modal de Canje de cheques
$("#Exchange").click(function () {
    var selRowIds = $("#searchCheck").UifDataTable("getSelected");

    $("#alert").UifAlert('hide');

    for (var i in selRowIds) {

        checkNumber = selRowIds[i].DocumentNumber;
        paymentCode = selRowIds[i].PaymentCode;
        currencyCode = selRowIds[i].CurrencyCode;
        currencyName = selRowIds[i].CurrencyDescription;
        checkDate = selRowIds[i].DatePayment;
        issuingBankO = selRowIds[i].IssuingBankCode;
        issuingBankNameO = selRowIds[i].BankDescription;
        issuingAccountBankO = selRowIds[i].ReceivingAccountNumber;
        amountOriginal = selRowIds[i].Amount;
        currentStatusName = selRowIds[i].StatusDescription;
        incomeReceiptNumberO = selRowIds[i].BillCode;
        paymentStatusO = selRowIds[i].Status;
        branchCodeO = selRowIds[i].BranchCode;
        branchNameO = selRowIds[i].BranchDescription;
        holderCheckO = selRowIds[i].Holder;
        exchangeRateO = selRowIds[i].ExchangeRate;
        paymentMethodTypeCodeO = selRowIds[i].PaymentMethodTypeCode;
    }

    if (checkNumber != "") {
        $("#ExchangeCheckModal").find('.ptitle:first').html(Resources.ExchangeCheck + ": " + checkNumber);
        $("#ExchangeCheckDate").val(($("#ViewBagAccountingDate").val()).substring(0, 10));
        $("#IssuingBankNameOriginal").val(issuingBankNameO);
        $("#IssuingBankOriginal").val(issuingBankO);
        $("#IssuingAccountBankOriginal").val(issuingAccountBankO);
        $("#CurrencyNameOriginal").val(currencyName);
        $("#CurrencyNameNew").val(currencyName);
        $("#AmountOriginal").val(FormatCurrency(amountOriginal));
        $("#CheckNewAmount").val(FormatCurrency(amountOriginal));
        $("#CurrentStatusNameOriginal").val(currentStatusName);
        $("#CurrentStatusOriginal").val(paymentStatusO);
        $("#CheckNumberOriginal").val(checkNumber);
        $("#CheckDateOriginal").val(checkDate);
        $("#paymentCode").val(paymentCode);
        $("#IncomeReceiptNumberOriginal").val(incomeReceiptNumberO);
        $("#CurrencyCodeOriginal").val(currencyCode);
        $("#ExchangeBranchCode").val(branchCodeO);
        $("#ExchangeBranchName").val(branchNameO);
        $("#HolderCheckOriginal").val(holderCheckO);
        $("#OriginalExchangeRate").val(exchangeRateO);
        $("#PaymentMethodTypeCode").val(paymentMethodTypeCodeO);

        $('#ExchangeCheckModal').UifModal('showLocal', Resources.ExchangeCheck + ":  " + checkNumber);
    }
    else {
        $("#alert").UifAlert('show', Resources.InternalBallotCardDialogMessageEdit, 'info');
    }
});

//Valida que no se ingrese fecha mayor al del sistema
$("#CheckNewDate").blur(function () {

    var checkNewDate = $("#CheckNewDate").val();
    if (IsDate(checkNewDate)) {
        if (CompareDates(checkNewDate, getCurrentDate()) == 1) {
            $("#CheckNewDate").val($("#BillingDate").val());
        }
        $("#ExchangeCheckModal").find("#AlertExchangeChecks").UifAlert('hide');
    }
    else {

        $("#ExchangeCheckModal").find("#AlertExchangeChecks").UifAlert('show', Resources.InvalidDates, 'warning');
        $("#CheckNewDate").val($("#BillingDate").val());
    }

});

// Boton que cancela  Canje de cheques
$("#CancelExchange").click(function () {
    setExchangeCheckDataFieldsAEmpty();
    $("#ExchangeCheckModal").find('.ptitle:first').html("");
});

// Botón que llama a modal de Legalización de cheques
$("#Legalize").click(function () {
    var selRowIds = $("#searchCheck").UifDataTable("getSelected");

    $("#alert").UifAlert('hide');

    var issuingBankL = null;
    var issuingBankNameL = null;
    var issuingAccountBankL = null;
    var amountOriginal = null;
    var currentStatusName = null;
    var incomeReceiptNumberL = null;
    var paymentStatusL = null;
    var branchCodeL = null;
    var branchNameL = null;
    var holderCheckL = null;
    var exchangeRateL = null;
    var paymentMethodTypeCodeL = null;

    for (var i in selRowIds) {

        checkNumber = selRowIds[i].DocumentNumber;
        paymentCode = selRowIds[i].PaymentCode;
        currencyCode = selRowIds[i].CurrencyCode;
        currencyName = selRowIds[i].CurrencyDescription;
        checkDate = selRowIds[i].DatePayment;
        issuingBankL = selRowIds[i].IssuingBankCode;
        issuingBankNameL = selRowIds[i].BankDescription;
        issuingAccountBankL = selRowIds[i].ReceivingAccountNumber;
        amountOriginal = selRowIds[i].Amount;
        currentStatusName = selRowIds[i].StatusDescription;
        incomeReceiptNumberL = selRowIds[i].BillCode;
        paymentStatusL = selRowIds[i].Status;
        branchCodeL = selRowIds[i].BranchCode;
        branchNameL = selRowIds[i].BranchDescription;
        holderCheckL = selRowIds[i].Holder;
        exchangeRateL = selRowIds[i].ExchangeRate;
        paymentMethodTypeCodeL = selRowIds[i].PaymentMethodTypeCode;
    }


    if (checkNumber != "") {
        $("#LegalizeCheckModal").find('.ptitle:first').html(Resources.CheckLegalization + " " + checkNumber);

        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "LegalPayment/GetRejectedPaymentId",
            data: { "bankId": issuingBankL, "documentNumber": checkNumber },
            success: function (data) {
                if (data.length > 0) {
                    rejectedPaymentId = data[0].RejectedPaymentCode;
                    paymentId = data[0].PaymentCode;
                    //var issuingBankId = data[0].IssuingBankCode;
                    $("#LegalizeCheckModal").find("#LegalizeIssuingBankCode").val(issuingBankL);
                    $("#LegalizeCheckModal").find("#LegalizeIssuingBankName").val(issuingBankNameL);
                    $("#LegalizeCheckModal").find("#LegalizeIssuingAccountBank").val(issuingAccountBankL);
                    $("#LegalizeCheckModal").find("#LegalizePaymentCode").val(paymentCode);
                    $("#LegalizeCheckModal").find("#LegalizeCheckNumber").val(checkNumber);
                    $("#LegalizeCheckModal").find("#LegalizePaymentMethodTypeCode").val(paymentMethodTypeCodeL);
                    $("#LegalizeCheckModal").find("#LegalizeCheckDate").val(data[0].DatePayment);
                    $("#LegalizeCheckModal").find("#LegalizeBillCode").val(data[0].CollectCode);
                    $("#LegalizeCheckModal").find("#LegalizeRejectionBranch").val(branchCodeL);                    
                    $("#LegalizeCheckModal").find("#LegalizeHolderName").val(data[0].Holder);
                    $("#LegalizeCheckModal").find("#LegalizeCurrencyCode").val(currencyCode);
                    $("#LegalizeCheckModal").find("#LegalizeCurrencyName").val(data[0].CurrencyDescription);
                    $("#LegalizeCheckModal").find("#LegalizeAmount").val(FormatCurrency(data[0].Amount));
                    $("#LegalizeCheckModal").find("#LegalizeReceiptNumber").val(incomeReceiptNumberL);
                    $("#LegalizeCheckModal").find("#LegaizeCheckDepositDate").val('');
                    $("#LegalizeCheckModal").find("#LegalizeCheckDepositBallotNumber").val('');
                    $("#LegalizeCheckModal").find("#LegalizeCheckReceivingBank").val('');
                    $("#LegalizeCheckModal").find("#LegalizeCheckReceivingAccountBank").val('');
                    $("#LegalizeCheckModal").find("#LegalizeCheckDepositTransaction").val('');
                    $("#LegalizeCheckModal").find("#LegalizeCheckPayer").val(data[0].Name);
                    $("#LegalizeCheckModal").find("#LegalizeCheckRejectedDate").val(data[0].RejectionDate);
                    $("#LegalizeCheckModal").find("#LegalizeCheckRejectMotive").val(data[0].RejectionDescription);
                    $("#LegalizeCheckModal").find("#LegalizeRejectionReceiptNumber").val(rejectedPaymentId);
                    $("#LegalizeCheckModal").find("#LegalizePayerId").val(data[0].PayerId);

                    //checkDocumentNumber = data[0].DocumentNumber;
                    //issuingAccountNumber = data[0].IssuingAccountNumber;

                    $.ajax({
                        async: false,
                        type: "POST",
                        url: ACC_ROOT + "LegalPayment/GetPaymentBallotInfoByPaymentId",
                        data: { "paymentId": paymentId },
                        success: function (ballotData) {
                            var jdate = parseLegalJsonDate(ballotData.DepositBallotRegisterDate);
                            var year = jdate.getFullYear();
                            var month = jdate.getMonth() + 1 < 10 ? '0' + (jdate.getMonth() + 1) : (jdate.getMonth() + 1);
                            var date = jdate.getDate() < 10 ? '0' + jdate.getDate() : jdate.getDate();
                            var dateString = date + '/' + month + '/' + year;
                            $("#LegalizeCheckModal").find("#LegaizeCheckDepositDate").val(dateString);
                            $("#LegalizeCheckModal").find("#LegalizeCheckDepositBallotNumber").val(ballotData.DepositBallotNumber);
                            $("#LegalizeCheckModal").find("#LegalizeCheckReceivingBank").val(ballotData.DepositBallotBankDescription);
                            $("#LegalizeCheckModal").find("#LegalizeCheckReceivingAccountBank").val(ballotData.DepositBallotAccountNumber);
                            $("#LegalizeCheckModal").find("#LegalizeCheckDepositTransaction").val(ballotData.DepositBallotId);
                        }
                    });
                }
                else {
                    $("#alert").UifAlert('show', Resources.DocumentNotRejected, 'info');
                }
            }
        });

        $('#LegalizeCheckModal').UifModal('showLocal', Resources.CheckLegalization + " " + checkNumber);
    }
    else {
        $("#alert").UifAlert('show', Resources.InternalBallotCardDialogMessageEdit, 'warning');
    }

});

// Boton que cancela de legalización de cheques
$("#CancelLegalize").click(function () {
    setLegalizeCheckDataFieldsAEmpty();
});

//Autocomplete Banco
$('#CheckBank').on('itemSelected', function (event, selectedItem) {
    fillAutocompleteBankCheck(selectedItem);
});

/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                           DEFINICION DE GRIDS                                                                            */
/*----------------------------------------------------------------------------------------------------------------------------------------------------------*/

var dataSelected = null;
$('#searchCheck').on('rowSelected', function (event, selectedRow) {
    if (selectedRow != null) {
        bankId = selectedRow.IssuingBankCode;
        checkNumber = selectedRow.DocumentNumber;
        bank = selectedRow.BankDescription;
        branchId = selectedRow.BranchCode;

        //var status = selectedRow.StatusDescription;
        var statusId = selectedRow.Status;
        dataSelected = selectedRow;

        //DEPOSITADO
        if (statusId == 3) {
            $("#Exchange").hide();
            $("#Reject").show();
            $("#Legalize").hide();
            $("#Regularize").hide();
        }// INGRESADO, REGULARIZADO
        else if (statusId == 1) {
            $("#Exchange").show();
            $("#Reject").hide();
            $("#Legalize").hide();
            $("#Regularize").hide();
        }// RECHAZADO
        else if (statusId == 4) {
            $("#Exchange").hide();
            $("#Reject").hide();
            $("#Legalize").show();
            $("#Regularize").show();
        }
        else {
            $("#Exchange").hide();
            $("#Reject").hide();
            $("#Legalize").hide();
            $("#Regularize").hide();
        }
    }

});

// Atacha evento click al seleccionar un registro de la tabla y al des seleccionarlo
$('body').delegate('#searchCheck tbody tr', "dblclick", function () {

    var selected = [];
    var selectedRow = {};
    var i = 0;
    var buttonCh;

    $(this).addClass("row-selected");

    $(this).children("td").each(function () {
        selected[i] = $(this).html();
        if (($(this).children("button").children("span")).length > 0) {
            buttonCh = $(this).children("button").children("span");
        }

        if (($(this).children("div").children("button").children("span")).length > 0) {
            buttonCh = $(this).children("button").children("span");
        }

        i++;
    });

    buttonCh.addClass("glyphicon-check").removeClass("glyphicon-unchecked");
    var dataChecksSelect = $("#searchCheck").UifDataTable("getSelected");

    showDialog(dataChecksSelect[0]);
});

/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE FUNCIONES                                                                                */
/*-------------------------------------------------------------------------------------------------------------------------------------------------------------*/

function showDialog(row) {

    $("#internalBallotDate").val("");
    $("#internalBallotNumber").val("");
    $("#receivingBank").val("");
    $("#receivingAccount").val("");
    $("#depositDate").val("");
    $("#depositBallotNumber").val("");
    $("#transactionDeposit").val("");
    $("#dateRejection").val("");
    $("#motiveRejection").val("");
    $("#rejectedComision").val("");
    $("#rejectedIva").val("");
    $("#rejectedTransaction").val("");
    $("#regularizedDate").val("");
    $("#regularizedTransaction").val("");
    $("#legalDate").val("");
    $("#legalTransaction").val("");

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Transaction/GetSearchCheck",
        data: { "paymentCode": row.PaymentCode, "bankCode": row.IssuingBankCode, "accountNumber": row.ReceivingAccountNumber, "checkNumber": row.DocumentNumber },
        success: function (data) {
            if (data.length > 0) {
                //DEPLIEGUE CHEQUES INGRESADOS
                $("#checkBankDetail").val(row.BankDescription);
                $("#accountNumberDetail").val(row.ReceivingAccountNumber);
                $("#checkNumberDetail").val(row.DocumentNumber);
                $("#status").val(data[0].StatusDescription);
                $("#depositor").val(data[0].Name);
                $("#checkDate").val(data[0].DatePayment);
                $("#checkCurrency").val(data[0].CurrencyDescription);
                $("#amount").val(data[0].Amount);
                $("#issuer").val(data[0].Holder);
                $("#receiptCheckNumber").val(data[0].CollectCode);
                $("#incomeConcept").val(data[0].CollectConceptDescription);
                $("#registerDate").val(data[0].RegisterDate);
                $("#amount").blur();
                $("#branch").val(data[0].BranchDescription);

                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "Transaction/GetInternalBallotInformation",
                    data: { "paymentCode": data[0].PaymentCode },
                    success: function (depositData) {
                        if (depositData.length > 0) {
                            //DEPLIEGUE BOLETA INTERNA
                            $("#internalBallotDate").val(depositData[0].RegisterDate);
                            $("#internalBallotNumber").val(depositData[0].PaymentTicketCode);
                            $("#receivingBank").val(depositData[0].BankDescription);
                            $("#receivingAccount").val(depositData[0].ReceivingAccountNumber);
                        }
                    }
                });

                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "Transaction/GetDepositInformation",
                    data: { "paymentCode": data[0].PaymentCode },
                    success: function (depositData) {

                        if (depositData.length > 0) {
                            //DEPLIEGUE CHEQUES DEPOSITADOS
                            $("#depositDate").val(depositData[0].DepositRegisterDate);
                            $("#depositBallotNumber").val(depositData[0].PaymentBallotNumber);
                            $("#transactionDeposit").val(depositData[0].PaymentBallotCode);

                            $.ajax({
                                async: false,
                                type: "POST",
                                url: ACC_ROOT + "Transaction/GetRejectedInformation",
                                data: { "paymentCode": depositData[0].PaymentCode },
                                success: function (rejectionData) {

                                    if (rejectionData.length > 0) {
                                        //DEPLIEGUE CHEQUES RECHAZADOS
                                        $("#dateRejection").val(rejectionData[0].RejectionDate);
                                        $("#motiveRejection").val(rejectionData[0].RejectionDescription);
                                        $("#rejectedComision").val(rejectionData[0].Comission);
                                        $("#rejectedIva").val(rejectionData[0].TaxComission);
                                        $("#rejectedTransaction").val(rejectionData[0].RejectedPaymentCode);
                                        $("#rejectedIva").blur();
                                        $("#rejectedComision").blur();

                                        $.ajax({
                                            async: false,
                                            type: "POST",
                                            url: ACC_ROOT + "Transaction/GetRegularizedInformation",
                                            data: { "paymentCode": rejectionData[0].PaymentCode },
                                            success: function (regularizedData) {
                                                if (regularizedData.length > 0) {
                                                    //DEPLIEGUE CHEQUES REGULARIZADOS
                                                    $("#regularizedDate").val(regularizedData[0].RegisterDate);
                                                    $("#regularizedTransaction").val(regularizedData[0].RegularizePaymentCode);
                                                }
                                            }
                                        });

                                        $.ajax({
                                            async: false,
                                            type: "POST",
                                            url: ACC_ROOT + "Transaction/GetLegalInformation",
                                            data: { "paymentCode": rejectionData[0].PaymentCode },
                                            success: function (legalInformation) {
                                                if (legalInformation.length > 0) {
                                                    //DEPLIEGUE CHEQUES LEGALIZADOS
                                                    $("#legalDate").val(legalInformation[0].Date);
                                                    $("#legalTransaction").val(legalInformation[0].LegalPaymentCode);
                                                }
                                            }
                                        });
                                    }
                                }
                            });
                        }
                    }
                });
            }
        }
    });

    $('#SearchModal').appendTo("body").modal('show');
}

function setDataFieldsEmptyCheckSearch() {
    clearCheckSearch();
    clearCheckInformation();
    clearDepositInformation();
    clearRejectedInformation();
    clearRegularizedInformation();
    clearLegalInformation();
    IssuingBankIdCheckSearch = -1;
    $("#searchCheck").dataTable().fnClearTable();
    $("#Exchange").hide();
    $("#Reject").hide();
    $("#Legalize").hide();
    $("#Regularize").hide();
    $("#alert").UifAlert('hide');
    $("#CheckSearchExcelExport").hide();
}

function clearCheckSearch() {
    $("#Branches").val("");
    $("#CheckBank").val("");
    $("#_BankSearch").val("");
    $("#CheckNumber").val("");
    $("#AccountNumber").val("");
    $("#BillCode").val("");
}

function clearCheckInformation() {
    $("#checkBankDetail").val("");
    $("#branch").val("");
    $("#accountNumberDetail").val("");
    $("#checkNumberDetail").val("");
    $("#receiptCheckNumber").val("");
    $("#status").val("");
    $("#checkDate").val("");
    $("#checkCurrency").val("");
    $("#amount").val("");
    $("#issuer").val("");
    $("#incomeConcept").val("");
    $("#registerDate").val("");
    $("#depositor").val("");
}

function clearDepositInformation() {
    $("#internalBallotDate").val("");
    $("#internalBallotNumber").val("");
    $("#receivingBank").val("");
    $("#receivingAccount").val("");
    $("#depositDate").val("");
    $("#depositBallotNumber").val("");
    $("#transactionDeposit").val("");
}

function clearRejectedInformation() {
    $("#dateRejection").val("");
    $("#motiveRejection").val("");
    $("#rejectedComision").val("");
    $("#rejectedIva").val("");
    $("#rejectedTransaction").val("");
}

function clearRegularizedInformation() {
    $("#regularizedDate").val("");
    $("#regularizedTransaction").val("");
}

function clearLegalInformation() {
    $("#legalDate").val("");
    $("#legalTransaction").val("");
}

// Exchange Check

function setExchangeCheckDataFieldsAEmpty() {
    $("#ExchangeCheckDate").val('');
    $("#PaymentMethodTypeCode").val('');
    $("#ExchangeBranchName").val('');
    $("#ExchangeBranchCode").val('');
    $("#ExchangeDocumentNumber").val('');
    $("#ExchangeName").val('');
    $("#IssuingBankOriginal").val('');
    $("#IssuingBankOriginal").val('');
    $("#IssuingAccountBankOriginal").val('');
    $("#IssuingAccountBankOriginal").val('');
    $("#CheckNumberOriginal").val('');
    $("#IncomeReceiptNumberOriginal").val('');
    $("#CheckDateOriginal").val("");
    $("#CurrencyNameOriginal").val("");
    $("#CurrencyNameOriginal").val('');
    $("#AmountOriginal").val('');
    $("#HolderCheckOriginal").val('');
    $("#CurrentStatusOriginal").val('');
    $("#CheckNewBank").val('');
    $("#CheckNewAccountNumber").val('');
    $("#CheckNewNumber").val('');
    $("#CurrencyNameNew").val('');
    $("#CheckNewAmount").val('');
    $("#CheckNewDate").val('');
    $("#CheckNewEmisor").val('');
}

function confirmMainCheckSearch(message, params) {
    // var message = message.split('\n').join('<br>');

    //Valida que el número del cheque no este ya registrado en la BDD
    if (validCheckDocumentNumber(IssuingNewBankId, $("#CheckNewNumber").val(), $("#CheckNewAccountNumber").val()) == false) {

        $("#ExchangeAlert").UifAlert('show', Resources.RegisteredCheckNum, 'warning');
    } else {
        setDataPayment();
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Transaction/SaveChangeCheck",
            data: { "frmBill": prepareRequest(PaymentSummaryModel), "payerId": $("#NewPayerId").val(), "descriptionChange": Resources.ForExchangeCheck },
            success: function (data) {

                $("#SaveCheckSuccessModal").find('.ptitle:first').html(Resources.SuccessfullySaved);
                $("#SaveCheckSuccessModal").find('#title').html(Resources.VoucherNumber + ": " + BillId + "." + Resources.GeneratedBillingIncomeNo + ": " + data.BillId);
                $("#SaveCheckSuccessModal").find("#CheckDescription").val(data.Message);
                $("#SaveCheckSuccessModal").find("#TotalAmountCheck").val($("#AmountOriginal").val());
                $("#SaveCheckSuccessModal").find("#ReceiptNumberCheck").val("00000" + data.BillId);

                GetAccountingDateCheckSearch();
                GetUserNickCheckSearch();

                if (data.ShowMessage == "False") {
                    $("#SaveCheckSuccessModal").find("#accountingMessageDiv").hide();
                } else {
                    $("#SaveCheckSuccessModal").find("#accountingMessageDiv").show();
                }
                $("#SaveCheckSuccessModal").find("#accountingMessage").val(data.Message);

                setExchangeCheckDataFieldsAEmpty();

                $("#ExchangeCheckModal").find("#CancelExchange").trigger('click');
                $('#SaveCheckSuccessModal').UifModal('showLocal', Resources.SuccessfullySaved);

                $("#Exchange").hide();
                $("#Reject").hide();
                $("#Legalize").hide();
                $("#Regularize").hide();

                if ($("#CheckBank").val() != "") {
                    refreshTable();
                }
            }
        });
    }
}

// Obtiene la fecha del servidor
function getcheckSearchDate() {

    if ($("#ViewBagImputationType").val() == undefined &&
        $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
        $("#ViewBagBillControlId") == undefined) {

        var checkSearchDatePromise = new Promise(function(resolve, reject) {
            $.ajax({
                type: "POST",
                url: ACC_ROOT +  "Common/GetDate",
                success: function (checkSearchDate) {
                    resolve(checkSearchDate);
                }
            });
        });

        checkSearchDatePromise.then(function (checkSearchDate) {
            $("#BillingDate").val(checkSearchDate);
            $("#CheckNewDate").val(checkSearchDate);
        });
    }
}

getcheckSearchDate();

// Legalize Check
function setLegalizeCheckDataFieldsAEmpty() {
    $("#LegalizeCheckModal").find("#LegalizeIssuingBankName").val('');
    $("#LegalizeCheckModal").find("#LegalizePaymentMethodTypeCode").val('');
    $("#LegalizeCheckModal").find("#LegalizeIssuingBankCode").val('');
    $("#LegalizeCheckModal").find("#LegalizeIssuingAccountBank").val('');
    $("#LegalizeCheckModal").find("#LegalizeCheckNumber").val('');
    $("#LegalizeCheckModal").find("#LegalizeCheckDate").val('');
    $("#LegalizeCheckModal").find("#LegalizeHolderName").val('');
    $("#LegalizeCheckModal").find("#LegalizeCurrencyName").val('');
    $("#LegalizeCheckModal").find("#LegalizeCurrencyCode").val('');
    $("#LegalizeCheckModal").find("#LegalizeAmount").val('');
    $("#LegalizeCheckModal").find("#LegalizeReceiptNumber").val('');
    $("#LegalizeCheckModal").find("#LegaizeCheckDepositDate").val('');
    $("#LegalizeCheckModal").find("#LegalizeCheckDepositBallotNumber").val("");
    $("#LegalizeCheckModal").find("#LegalizeCheckRecievingBank").val("");
    $("#LegalizeCheckModal").find("#LegalizeCheckRecievingAccountBank").val('');
    $("#LegalizeCheckModal").find("#LegalizeCheckDepositTransaction").val('');
    $("#LegalizeCheckModal").find("#LegalizeCheckPayer").val('');
    $("#LegalizeCheckModal").find("#LegalizeCheckRejectedDate").val('');
    $("#LegalizeCheckModal").find("#LegalizeRejectionReceiptNumber").val('');
    $("#LegalizeCheckModal").find("#LegalizeCheckRejectMotive").val('');
    $("#LegalizeCheckModal").find("#CheckLegalTransferDate").val('');
    $("#LegalizeCheckModal").find("#LegalizePaymentCode").val('');
}

function confirmLegalize(message, params) {
    var show = '<h4><b>' + message + '</b></h4>';
    $("#saveMessageTitle").html(show);

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "LegalPayment/SaveLegalPaymentRequest",
        data: {
            "frmLegalPayment": setDataLegalPayment(), "payerId": $("#LegalizeCheckModal").find("#LegalizePayerId").val(),
            "branchId": $("#LegalizeCheckModal").find("#LegalizeRejectionBranch").val(),
            "descriptionLegalize": Resources.DueCheckLegalization.replace('&#243;', 'o')
        },
        success: function (data) {

            $("#SaveCheckSuccessModal").find('.ptitle:first').html(Resources.CheckLegalization + " " + Resources.SuccessfullySaved);
            $("#SaveCheckSuccessModal").find('#title').html("  " + Resources.VoucherNumber + ": " + BillId + ". " + Resources.GeneratedBillingIncomeNo + ": " + data.TechnicalTransaction);


            $("#CheckDescription").val(Resources.DueCheckLegalization.replace('&#243;', 'o') + " No. " + $("#LegalizeCheckNumber").val());
            $("#TotalAmountCheck").val($("#LegalizeAmount").val());
            $("#ReceiptNumberCheck").val("00000" + data.TechnicalTransaction);

            GetAccountingDateCheckSearch();
            GetUserNickCheckSearch();

            if (data.ShowMessage == "False") {
                $("#SaveCheckSuccessModal").find("#accountingMessageDiv").hide();
            } else {
                $("#SaveCheckSuccessModal").find("#accountingMessageDiv").show();
            }

            setLegalizeCheckDataFieldsAEmpty();

            $("#LegalizeCheckModal").find("#CancelLegalize").trigger('click');
            $('#SaveCheckSuccessModal').UifModal('showLocal', show);

            $("#Exchange").hide();
            $("#Reject").hide();
            $("#Legalize").hide();
            $("#Regularize").hide();

            if ($("#CheckBank").val() != "") {
                refreshTable();
            }
        }
    });
}

function parseLegalJsonDate(jsonDate) {
    var offset = new Date().getTimezoneOffset() * 60000;
    var parts = /\/Date\((-?\d+)([+-]\d{2})?(\d{2})?.*/.exec(jsonDate);

    if (parts[2] == undefined)
        parts[2] = 0;

    if (parts[3] == undefined)
        parts[3] = 0;

    return new Date(+parts[1] + offset + parts[2] * 3600000 + parts[3] * 60000);
}

function ValidateDateLegalize() {
    var result = false;

    if (CompareDates($("#LegalizeCheckModal").find("#LegalizeCheckRejectedDate").val(), $("#LegalizeCheckModal").find("#CheckLegalTransferDate").val()) == 2) {
        result = true;
    }
    else if (CompareDates($("#LegalizeCheckModal").find("#LegalizeCheckRejectedDate").val(), $("#LegalizeCheckModal").find("#CheckLegalTransferDate").val())) {
        result = false;
    } else {
        result = true;
    }

    return result;
}

function GetAccountingDateCheckSearch() {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Common/GetAccountingDate",
        success: function (data) {
            $("#ReceiptDate").val(data);
            $("#SaveCheckSuccessModal").find("#ReceiptDate").val(data);
        }
    });
}

function GetUserNickCheckSearch() {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Common/GetUserNick",
        success: function (data) {
            $("#UserReceipt").val(data);
            $("#SaveCheckSuccessModal").find("#UserReceipt").val(data);
        }
    });
}

function fillAutocompleteBankCheck(ui) {
    $("#CheckBank").val(ui.Value);
    IssuingBankIdCheckSearch = ui.Id;
    $("#_BankSearch").val(ui.Id);
}

/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

//Variables Globales
var bankId = -1;
var Alert = 0;
var numErrors = 0;
var arrayIds = new Array();
var billIds = new Array();
var bank = "";
var user = "";

var oRejectedPayment = {
    Id: null,
    PaymentId: null,
    RejectionId: null,
    RejectionDate: null,
    Commission: null,
    TaxCommission: null,
    Description: null
};

var oTblPolicies = {
    TblItem: []
};

var oTblItem = {
    Id: null,
    Branch: null,
    Prefix: null,
    Endorsement: null,
    Quote: null,
    Amount: null,
    BillItemId: null
};

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

function setTblPolicies() {
    var selRowIds = $("#tblPolicies").UifDataTable("getSelected");  //checksDialog

    for (var i in selRowIds) {
        cleanObjectTbl();
        oTblItem.Id = selRowIds[i].QuoteNum;
        oTblItem.Branch = selRowIds[i].BranchDescription;
        oTblItem.Prefix = selRowIds[i].PrefixDescription;
        oTblItem.Endorsement = selRowIds[i].Endorsement;
        oTblItem.Quote = selRowIds[i].QuoteNum;
        oTblItem.Amount = selRowIds[i].Amount;
        oTblItem.BillItemId = selRowIds[i].BillItemCode;
        oTblPolicies.TblItem.push(oTblItem);
    };
    return oTblPolicies;
};

function cleanObjectTbl() {
    oTblItem = {
        Id: null,
        Branch: null,
        Prefix: null,
        Endorsement: null,
        Quote: null,
        Amount: null,
        BillItemId: null
    };
}

function cleanSearchCheckingRejection() {
    $("#CheckDate").val("");
    $("#CurrencyRejection").val("");
    $("#AmountRejection").val("");
    $("#IssuerRejection").val("");
    $("#ReceipNumber").val("");
    $("#DepositDateRejection").val("");
    $("#RejectionDepositBallotNumber").val("");
    $("#ReceiverBank").val("");
    $("#ReceivingAccount").val("");
    $("#DepositTransaction").val("");
    $("#Depositor").val("");
    $("#tblPolicies").dataTable().fnClearTable();

    cleanObjectTbl();
    arrayIds.length = 0;
    billIds.length = 0;
    bankId = -1;
    Alert = 0;
    numErrors = 0;
    BillId = -1;
    PaymentId = -1;
    for (var i in oTblPolicies) {
        TblItem[i] = [];
    }
}

function cleanCheckingRejection() {
    oTblPolicies.TblItem.splice(0, oTblPolicies.TblItem.length);
    $("#CheckDate").val("");
    $("#CurrencyRejection").val("");
    $("#AmountRejection").val("");
    $("#IssuerRejection").val("");
    $("#ReceipNumber").val("");
    $("#DepositDateRejection").val("");
    $("#RejectionDepositBallotNumber").val("");
    $("#ReceiverBank").val("");
    $("#ReceivingAccount").val("");
    $("#DepositTransaction").val("");
    $("#Depositor").val("");
    $("#tblPolicies").dataTable().fnClearTable();
    $("#RejectionDate").val("");
    $("#RejectionMotive").val(-1);
    $("#RejectedCommission").val("");
    $("#RejectedCommissionTax").val("");
    $("#Observations").val("");
    $("#RejectedIssuingBankName").val("");
    $("#RejectedIssuingAccountBank").val("");
    $("#RejectedCheckNumber").val("");

    cleanObjectTbl();
    bankId = -1;
    Alert = 0;
    numErrors = 0;
    BillId = -1;
    PaymentId = -1;
    arrayIds = [];
    billIds = [];
}

function setRejectedPayment() {
    cleanObject();
    if (PaymentId > -1) {
        oRejectedPayment.Id = 0;
        oRejectedPayment.PaymentId = PaymentId;
        oRejectedPayment.RejectionId = $("#RejectionMotive").val();
        oRejectedPayment.RejectionDate = $("#RejectionDate").val();
        oRejectedPayment.Commission = ClearFormatCurrency($("#RejectedCommission").val()).replace(",", ".");
        oRejectedPayment.TaxCommission = ClearFormatCurrency($("#RejectedCommissionTax").val()).replace(",", ".");
        oRejectedPayment.Description = $("#Observations").val();
    };
    return oRejectedPayment;
}

function cleanObject() {
    oRejectedPayment = {
        Id: null,
        PaymentId: null,
        RejectionId: null,
        RejectionDate: null,
        Commission: null,
        TaxCommission: null,
        Description: null
    };
}

function validateReject() {
    if ($("#RejectionDate").val() == "" || $("#RejectionDate").val() == "__/__/____" || $("#RejectionMotive").val() == "-1") {
        numErrors = 1;
    }
    else {
        numErrors = 0;
    }
}

//valida que la comisión rechazo no sea mayor al valor del cheque
function validateCommissionRejection() {
    if ($("#RejectedCommission").val() != "") {
        if (parseFloat(ClearFormatCurrency($("#RejectedCommission").val().replace("", ","))) > parseFloat(ClearFormatCurrency($("#AmountRejection").val()).replace(",", "."))) {

            $("#alert").UifAlert('show', Resources.CommissionRejectionNotGreaterValueOfCheck, 'warning');
            return 1;
        } else {
            return 0;
        }
    }
    else {
        return 0;
    }
}

//valida que el IVA de la comisión rechazo no sea mayor al valor del cheque
function validateTaxCommissionRejection() {
    if ($("#RejectedCommissionTax").val() != "") {
        if (parseFloat(ClearFormatCurrency($("#RejectedCommissionTax").val().replace("", ","))) > parseFloat(ClearFormatCurrency($("#AmountRejection").val()).replace(",", "."))) {

            $("#alert").UifAlert('show', Resources.TaxRejectionNotGreaterValueOfCheck, 'warning');
            return 1;
        } else {
            return 0;
        }
    }
    else {
        return 0;
    }
}

function loadRejectedCheckReport(arrayIds, billIds) {
    return $.ajax({
        async: true,
        type: "POST",
        url: ACC_ROOT + "Report/RejectedCheckReport",
        data: { 'ArrayIds': arrayIds, 'CollectIds': billIds }
    });
}

function showReportCheckSearch() {
    window.open(ACC_ROOT + "Report/ShowRejectedCheckReport", 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

//////////////////////////////////////////////////
// Da formato a un número para su visualización //
//////////////////////////////////////////////////
function FormatCurrencyRejected(number, decimals, decimalSeparator, thousandsSeparator) {
    var parts, array;

    if (!isFinite(number) || isNaN(number = parseFloat(number))) {
        return "";
    }
    if (typeof decimalSeparator === "undefined") {
        decimalSeparator = ",";
    }
    if (typeof thousandsSeparator === "undefined") {
        thousandsSeparator = "";
    }

    // Redondeamos
    if (!isNaN(parseInt(decimals))) {
        if (decimals >= 0) {
            number = number.toFixed(decimals);
        }
        else {
            number = (
                Math.round(number / Math.pow(10, Math.abs(decimals))) * Math.pow(10, Math.abs(decimals))
            ).toFixed();
        }
    }
    else {
        number = number.toString();
    }

    // Damos formato
    parts = number.split(".", 2);
    array = parts[0].split("");
    for (var i = array.length - 3; i > 0 && array[i - 1] !== "-"; i -= 3) {
        array.splice(i, 0, thousandsSeparator);
    }
    number = array.join("");

    if (parts.length > 1) {
        number += decimalSeparator + parts[1];
    }

    return number;
}

/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//                                    FORMATOS BUTTONS /FECHAS/ #CARACTERES / NÚMEROS-DECIMALES
/*-----------------------------------------------------------------------------------------------------------------------------------------*/


//Control número máximo de caracteres
$("#RejectedCommission").attr("maxlength", 15);
$("#RejectedCommissionTax").attr("maxlength", 15);
$("#Observations").attr("maxlength", 70);

//Control solo se puede ingresar números
$("#RejectedCommission").keypress(function (event) {
    if (event.keyCode != 8 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        if ((event.which < 46 || event.which > 57) || (event.which == 47)) {
            event.preventDefault();
        }
    }
});

//Control solo se puede ingresar números
$("#RejectedCommissionTax").keypress(function (event) {
    if (event.keyCode != 8 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
        if ((event.which < 46 || event.which > 57) || (event.which == 47)) {
            event.preventDefault();
        }
    }
});

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

//Formato Moneda (se activa cuando pierde el foco)
$("#RejectedCommission").blur(function () {
    var commission = $.trim(ClearFormatCurrency($("#RejectedCommission").val()).replace(",", "."));
    if (commission != "" && commission != "$") {        
        $("#RejectedCommission").val("$ " + FormatCurrencyRejected(commission, "2", ".", ","));
    } else {
        $("#RejectedCommission").val('');
    }
});

//Formato Moneda (se activa cuando pierde el foco)
$("#RejectedCommissionTax").blur(function () {
    var commission = $.trim(ClearFormatCurrency($("#RejectedCommissionTax").val()).replace(",", "."));
    if (commission != "" && commission != "$") {        
        $("#RejectedCommissionTax").val("$ " + FormatCurrencyRejected(commission, "2", ".", ","));
    } else {
        $("#RejectedCommissionTax").val('');
    }
});

//Valida que no se ingresen cheques postfechados cuando esta seleccionado "Cheque Corriente"
$("#RejectionDate").blur(function () {

    if ($("#RejectionDate").val() != '') {

        if (IsDate($("#RejectionDate").val()) == true) {

            if (CompareDates($("#RejectionDate").val(), getCurrentDate()) == 2) {
                $("#RejectionDate").val($("#ViewBagRejectionDate").val());
            }
            else if (CompareDates($("#RejectionDate").val(), getCurrentDate()) == 1) {
                $("#RejectionDate").val($("#ViewBagRejectionDate").val());
            }
        }
        else {
            $("#alert").UifAlert('show', Resources.InvalidDates, 'warning');
            $("#RejectionDate").val($("#ViewBagRejectionDate").val());
        }
    }
});

/**********************************************************************************************************/
//ACCIONES DE BOTONES DE MODALES
$(document).ready(function () {

    //BOTÓN ACEPTAR DEL MODAL CANJE DE CHEQUES 
    $("#ExchangeCheckModal").find("#SaveExchange").click(function () {

        $("#ExchangeAlert").UifAlert('hide');
        $("#ExchangeForm").validate();
        $("#ExchangeInformationForm").validate();

        if ($("#ExchangeForm").valid() &&
        $("#ExchangeInformationForm").valid()) {
            confirmMainCheckSearch("", params);
        }
    });

    //BOTÓN ACEPTAR DEL MODAL DE  legalización de cheques
    $("#LegalizeCheckModal").find("#SaveLegalize").click(function () {

        $("#LegalizeCheckModal").find("#LegalizeForm").validate();

        if ($("#LegalizeCheckModal").find("#LegalizeForm").valid()) {
            if (ValidateDateLegalize() == true) {
                confirmLegalize("", params);
            }
            else {
                $("#LegalizeCheckModal").find("#LegalizeAlert").UifAlert('show', Resources.LegalizationDateGreaterRejection, 'warning');
            }
        }
    });

    //BOTÓN ACEPTAR DEL MODAL DE RECHAZO DE CHEQUES
    $('#RejectionModal').find("#SaveReject").click(function () {
        $("#InformationRejectedCheck").validate();
        validateReject();

        if (numErrors == 0 || numErrors == undefined) {
            Alert = 0;
        } else {
            Alert = 1;
            numErrors = 0;
        }
        if (Alert == 0 && $("#InformationRejectedCheck").valid()) {

            if (PaymentId > -1) {

                if (validateCommissionRejection() == 1) {
                    return;
                }
                if (validateTaxCommissionRejection() == 1) {
                    return;
                }

                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "CheckingRejection/SaveCheckingRejectionNew",
                    data: { "checkingRejectionModel": setRejectedPayment(), billId: BillId, payerId: payerId, branchId: $('#Branches').val() },
                    success: function (data) {
                        oTblPolicies = setTblPolicies();
                        if (oTblPolicies.TblItem.length > 0) {
                            for (var i in oTblPolicies.TblItem) {
                                arrayIds[i] = oTblPolicies.TblItem[i].BillItemId;
                            }
                        }
                        arrayIds.unshift(PaymentId);
                        billIds.unshift(BillId);
                        $('#SaveRejectedCheckSuccessModal').find("#RejectedCheckNumber").val("00000" + checkNumber);

                        var selectMotive = document.getElementById("RejectionMotive");
                        var selected;
                        if (selectMotive.options[selectMotive.selectedIndex] == undefined)
                            selected = "";
                        else
                            selected = selectMotive.options[selectMotive.selectedIndex].text;

                        $("#RejectionMotiveDialog").val(selected);
                        $("#RejectedCheckDate").val($("#RejectionDate").val());
                        $("#RejectedCheckNewRecive").val(data.BillId);
                        $("#RejectedCheckNewTechnicalTransaction").val(data.TechnicalTransaction);

                        if (data.ShowMessage == "False") {
                            $("#accountingMessageDiv").hide();
                        } else {
                            $("#accountingMessageDiv").show();
                        }
                        $("#checkAccountingMessage").val(data.imputationMessage);

                        $("#RejectionModal").hide();
                        $(".modal-backdrop:first").hide();

                        $('#SaveRejectedCheckSuccessModal').UifModal('showLocal', Resources.RejectedCheckSaved);
                        cancelClick = 0;
                        $('#SaveRejectedCheckSuccessModal').find("#RejectedCheckNumber").val("00000" + checkNumber);
                    }
                });
            }
            else {
                $("#RejectionAlert").UifAlert('show', Resources.PleaseSearchACheck, 'warninig');
            }
        }
    });

    //BOTÓN CANCELAR DEL MODAL DE RECHAZO DE CHEQUES
    $("#CancelReject").click(function () {
        cleanCheckingRejection();
        cancelClick = 1;
    });

    //BOTÓN DE IMPRESIÒN Modal de información de datos guardados 
    $("#PrintRejectedCheck").click(function () {
        loadRejectedCheckReport(arrayIds, billIds).done(function (data) {
            //Muestra reporte cargado
            showReportCheckSearch();
            setTimeout(function () {
                $("#LegalizeCheckModal").find("#AcceptRejectedCheck").trigger('click');
            }, 1000);
        });
    });

    //BOTÓN ACEPTAR Modal de información de datos guardados 
    $("#AcceptRejectedCheck").click(function () {
        cancelClick = 1;
        rejectionRefresh();
    });

    $('#SaveRejectedCheckSuccessModal').on('closed.modal', function () {
        if (cancelClick != 1) { 
            rejectionRefresh();
        }
    });
});//fin del dom

function rejectionRefresh() {
    cleanCheckingRejection();
    location.href = $("#ViewBagMainCheckSearchLink").val(); 
}



