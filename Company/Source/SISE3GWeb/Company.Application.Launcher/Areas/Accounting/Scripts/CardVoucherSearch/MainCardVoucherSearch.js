
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
var numErrors = 0;
var validateError = 0;

var oTblPolicies = {
    TblItem: []
};

var oTblTaxes = {
    oTblItemTax: []
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

var oTblItemTax = {
    Id: null,
    Taxes: null,
    Percent: null,
    Amount: null,
    Quote: null,
    PaymentId: null,
    TaxId: null,
    PaymentTaxId: null,
    TaxBase: null
};

var oRejectedPayment = {
    Id: null,
    PaymentId: null,
    RejectionId: null,
    RejectionDate: null,
    Commission: null,
    TaxCommission: null,
    Description: null
};


var arrayIds = new Array();
var paymentId = 0;
var billId = 0;


$("#RejectCardVoucherSearch").hide();
$("#RegularizeCardVoucherSearch").hide();

if ($("#ViewBagBranchDisable").val() == "1") {

    setTimeout(function () {
        $("#CardBranch").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#CardBranch").removeAttr("disabled");
}

$("#CardVoucherAmount").blur(function () {
    var cardVoucherAmount = $("#CardVoucherAmount").val();
    $("#CardVoucherAmount").val(FormatCurrency(FormatDecimal(cardVoucherAmount)));
});


$("#CardVoucherCommission").blur(function () {
    var cardVoucherCommission = $("#CardVoucherCommission").val();
    $("#CardVoucherCommission").val(FormatCurrency(FormatDecimal(cardVoucherCommission)));
});

/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//  ACCIONES / EVENTOS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

$("#RejectTax").blur(function () {
    var rejectTax = $("#RejectTax").val();
  $("#RejectTax").val(FormatCurrency(FormatDecimal(rejectTax)));
});

$("#RejectDateRejection").blur(function () {

    if ($("#RejectDateRejection").val() != '') {
        if (IsDate($("#RejectDateRejection").val()) == true) {

            if (CompareDates($("#RejectDateRejection").val(), getCurrentDate()) == 2) {
                $("#RejectDateRejection").val(getCurrentDate());
            }
            else if (CompareDates($("#RejectDateRejection").val(), getCurrentDate()) == 1) {
                $("#RejectDateRejection").val(getCurrentDate());
            }
        }
        else {
            $("#alertRejection").UifAlert('show', Resources.InvalidDates, "danger");
            $("#RejectDateRejection").val(getCurrentDate());
        }
    }
});


$('#VoucherPolicies').on('rowEdit', function (event, data, position) {
    var rowId = $("#VoucherPolicies").UifDataTable("getSelected");

    paymentId = rowId[0].PaymentCode;
    if (rowId != null) {
        showDialogCardDetails("VoucherSearchModal", paymentId, rowId);
        refreshGridtaxes();
    }
});


    
//BOTÓN RECHAZO
$("#RejectCardVoucherSearch").click(function () {

    var rowId = $("#VoucherPolicies").UifDataTable("getSelected");

    paymentId = rowId[0].PaymentCode;
    billId = rowId[0].BillCode;

    if (rowId != null) {
        showDialogCardDetails("ModalRejectVoucher", paymentId, rowId);
        refreshGridPolicies();
    }
});
//validateCheckCardDeposited
//BOTÓN REGULARIZACION
$("#RegularizeCardVoucherSearch").click(function () {
        
    var rowId = $("#VoucherPolicies").UifDataTable("getSelected");

    if (rowId != null) {
        var paymentId = rowId[0].PaymentCode;
        var voucherNumber = rowId[0].Voucher;
        var branchIdOrigin = rowId[0].BranchCode;
      
        location.href = $("#ViewBagLoadVoucherRegularizationLink").val() + "?paymentId=" + paymentId +
                               "&voucherNumber=" + voucherNumber + "&branchIdOrigin=" + branchIdOrigin + "";
    }
});

/**********************************************************************************************************/
//ACCIONES DE BOTONES DE MODALES
$(document).ready(function () {

    $("#ModalRejectVoucher").find('#SaveReject').click(function () {
            
        if ($("#RejectedVoucherFormulario").valid()) {
            showConfirmCardVoucherSearch();
        }
        else {
            if ($("#RejectDateRejection").val() == "" || $("#RejectionVoucherMotive").val() == "") {
                numErrors = 1;
            }
            else {
                numErrors = 0;
            }

            if (numErrors == 0 || numErrors == undefined) {
                validateError = 0;
            }
            else {
                validateError = 1;
                numErrors = 0;
            }
            if (validateError != 0) {

                $("#alertRejection").UifAlert('show', Resources.RequiredFieldsMissing, "danger");
                setTimeout(function () {
                    $("#alertRejection").UifAlert('hide');
                }, 4000);
            }
        }
    });
 

    // Botón Imprimir Detalle
    $("#RejectVoucherSuccessDialog").find('#PrintDetail').click(function () {
        $('#RejectVoucherSuccessDialog').UifModal('hide');
        ShowRejectedCheckReport(arrayIds);
        showReportCardVoucherSearch();
    });
});

    

/*Buscar Tarjetas*/
$("#SearchCardVoucher").click(function () {
    $("#SearchCardVoucherPartial").validate();
    if ($("#SearchCardVoucherPartial").valid()) {
        if ($("#CreditCardType").val() != "") {
            refreshGridCardVoucherSearch();
        }
    }
  
});

$("#RejectVoucherSuccessDialog").find("#TechnicalTransaction").keypress(function (event) {
    if ($("#CreditCardType").val() != "") {

        if (event.keyCode == 13 || event.keyCode == 9) {
            refreshGridCardVoucherSearch();
        }
    }
});

$("#CardVoucherNumber").keypress(function (event) {
    if ($("#CreditCardType").val() != "") {

        if (event.keyCode == 13 || event.keyCode == 9) {
            refreshGridCardVoucherSearch();
        }
    }
});

$("#CardNumber").keypress(function (event) {
    if ($("#CreditCardType").val() != "-1") {

        if (event.keyCode == 13 || event.keyCode == 9) {
            refreshGridCardVoucherSearch();
        }
    }
});

$("#Clean").click(function () {
    cleanFieldsCardVoucherSearch();
    $("#CreditCardType").UifSelect();
    $("#SearchCardVoucherPartial").formReset();
    $('#CreditCardType-error').remove();
});

$('#VoucherPolicies').on('rowSelected', function (event, data, position) {
    if (data != null) {
        if (data.Status == 3) {
            $("#RejectCardVoucherSearch").show();
        } else {
            $("#RejectCardVoucherSearch").hide();
        }

        if ($("#ViewBagRegularizationCards").val() == "true") {
            if (data.Status == 4) {
                $("#RegularizeCardVoucherSearch").show();
            } else {
                $("#RegularizeCardVoucherSearch").hide();
            }
        }
    }
});

$("#CardVoucherNumber").keypress(function (event) {
    if ($("#CreditCardType").val() != "-1") {

        if (event.keyCode == 13 || event.keyCode == 9) {
            refreshGridCardVoucherSearch();
        }
    }
});

$("#CardNumber").keypress(function (event) {
    if ($("#CreditCardType").val() != "-1") {

        if (event.keyCode == 13 || event.keyCode == 9) {
            refreshGridCardVoucherSearch();
        }
    }
});

$("#RejectVoucherSuccessDialog").find("#TechnicalTransaction").keypress(function (event) {
    if ($("#CreditCardType").val() != "") {

        if (event.keyCode == 13 || event.keyCode == 9) {
            refreshGridCardVoucherSearch();
        }
    }
});

/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
function showConfirmCardVoucherSearch() {
    $.UifDialog('confirm', { 'message': Resources.QuestionVoucherReject + ' ?',
                                 'title':   Resources.RejectCard }, function (result) {
        if (result) {

            $('#ModalRejectVoucher').modal('hide');
            // Graba el rechazo
            $.ajax({
                type: "POST",
                url: ACC_ROOT + "CheckingRejection/SaveCheckingRejection",
                data: JSON.stringify({ "checkingRejectionModel": setRejectedPaymentCardVoucherSearch(),
                                       "billId": billId, "payerId": 0 }),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data == -1) {
                        $("#alertSearchPartial").UifAlert('show', Resources.ErrorTransaction, "danger");
                    }
                    else {
                        refreshGridCardVoucherSearch();

                        oTblPolicies = setTblPoliciesCardVoucherSearch();
                        for (var i in oTblPolicies.TblItem) {
                            arrayIds[i] = oTblPolicies.TblItem[i].BillItemId;
                        }

                        arrayIds.unshift(paymentId);
                        $("#rejectTextMessage").val(Resources.CardVoucherSaved + ": " + data.BillId);
                        $("#RejectVoucherSuccessDialog").find("#TechnicalTransaction").val(Resources.CardVoucherSaved + ": " + data.TechnicalTransaction);

                        if (data.ShowMessage == "False") {
                            $("#accountingMessageDiv").hide();
                        } else {
                            $("#accountingMessageDiv").show();
                        }
                        $("#accountingMessage").val(data.Message);
                        $("#RejectVoucherSuccessDialog").appendTo("body").modal('show');
                        cleanFieldsCardVoucherSearch();
                    }
                }
            });
        }
        else {
            $('#ModalRejectVoucher').modal('hide');
            cleanFieldsInfoReject();
        }
    });
};

function ShowRejectedCheckReport(arrayIds) {
    $.ajax({
        type: "POST",
        async: false,
        url: ACC_ROOT + "Report/RejectedCheckReport",
        data: JSON.stringify(arrayIds),
        dataType: "json",
        contentType: "application/json",
        success: function () {
        }
    });
}

function showReportCardVoucherSearch() {
    window.open(ACC_ROOT + "Report/ShowRejectedCheckReport", 'mywindow', 'fullscreen=yes, scrollbars=auto');
};

function setTblPoliciesCardVoucherSearch() {
    var rowid = $("#VoucherPoliciesRejection").UifDataTable("getSelected");
    if (rowid != null) {
        for (var i in rowid) {
            cleanObjectTbl();
            oTblItem.Id = rowid[i].Id;
            oTblItem.Branch = rowid[i].BranchDescription;
            oTblItem.Prefix = rowid[i].PrefixDescription;
            oTblItem.Endorsement = rowid[i].Endorsement;
            oTblItem.Quote = rowid[i].QuoteNum;
            oTblItem.Amount = rowid[i].Amount;
            oTblItem.BillItemId = rowid[i].BillItemCode;
            oTblPolicies.TblItem.push(oTblItem);
        };
    }

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

function cleanObjectTblTaxes() {
    oTblItemTax = {
        Id: null,
        Taxes: null,
        Percent: null,
        Amount: null,
        Quote: null,
        PaymentId: null,
        TaxId: null,
        PaymentTaxId: null,
        TaxBase: null
    };
}


function setRejectedPaymentCardVoucherSearch() {
    cleanObject();
    oRejectedPayment.Id = 0;
    oRejectedPayment.PaymentId = paymentId;
    oRejectedPayment.RejectionId = $("#RejectionVoucherMotive").val();
    oRejectedPayment.RejectionDate = $("#RejectDateRejection").val();
    oRejectedPayment.Description = $("#RejectObservations").val();

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

function cleanFieldsInfoReject() {

    $("#RejectDateRejection").val("");
    $("#RejectionVoucherMotive").val("");
    $("#RejectObservations").val("");
}

function refreshGridCardVoucherSearch() {
    var controller = ACC_ROOT + "CardVoucherSearch/GetCardVoucherSearch?creditCardTypeCode=" + $('#CreditCardType').val()
                              + "&voucher=" + $('#CardVoucherNumber').val()
                              + "&documentNumber=" + $('#CardNumber').val()
                              + "&technicalTransaction=" + $("#TechnicalTransaction").val()
                              + "&branchCode=" + $("#CardBranch").val();
    $("#VoucherPolicies").UifDataTable({ source: controller });
}

function cleanFieldsCardVoucherSearch() {
    $("#CreditCardType").val("");
    $("#CardVoucherNumber").val("");
    $("#CardNumber").val("");
    $("#CardBranch").val("");
    $("#VoucherPolicies").dataTable().fnClearTable();
    $("#RejectCardVoucherSearch").hide();
}

function showDialogCardDetails(nameDialog, paymentCode, rowId) {
    var message = "";
    var dialogName = $("#" + nameDialog);

    if (nameDialog == "VoucherSearchModal") {

        $("#InternalBallotDateCardVoucher").val("");
        $("#InternalBallotNumberCardVoucher").val("");
        $("#ReceivingBankCardVoucher").val("");
        $("#ReceivingAccountNumberCardVoucher").val("");
        $("#DateDepositCardVoucher").val("");
        $("#DepositBallotNumberCardVoucher").val("");
        $("#DepositTransactionCardVoucher").val("");
        $("#CardVoucherDateRejection").val("");
        $("#CardVoucherReasonRejection").val("");
        $("#CardVoucherRejectedTransaction").val("");
        $("#RegularizedDateCard").val("");
        $("#RegularizedTransactionCard").val("");
    
        message = Resources.CardDetailedSearch;

        $.ajax({
            async: false,
            url: ACC_ROOT + "CardVoucherSearch/GetInformationPayment",
            data: { "paymentCode": paymentCode },
            success: function (data) {
                if (data.length > 0) {
                    //Detalle de Vouchers
                    $("#CardVoucherDuct").val(data[0].Description);
                    $("#CardVoucherCardNumber").val(rowId[0].DocumentNumber);
                    $("#CardVoucherVoucherNumber").val(data[0].Voucher);
                    $("#CardVoucherIncomeReceiptNumber").val(data[0].CollectCode);
                    $("#CardVoucherCurrentStatus").val(data[0].StatusDescription);
                    $("#CardVoucherCardDate").val(data[0].CardDate);
                    $("#CardVoucherCurrency").val(data[0].CurrencyDescription);
                    $("#CardVoucherAmount").val(data[0].Amount);
    
                    $("#CardVoucherBusinessNameCard ").val(data[0].Holder);
                    $("#CardVoucherIncomeConcept").val(data[0].CollectConceptDescription);
                    $("#CardVoucherRegisterDate").val(data[0].DatePayment);
                    $("#CardVoucherBranch").val(data[0].BranchDescription);
                    $("#CardVoucherIssuingBank").val(data[0].BankDescription);
                        
                    $("#CardVoucherCommission").val(data[0].Commission);
                    $("#CardVoucherPayer").val(data[0].Name);

                    $("#CardVoucherAmount").blur();
                    $("#CardVoucherTax").blur();
                    $("#CardVoucherCommission").blur();
                    $("#CardVoucherTaxRetention").blur();
                    $("#CardVoucherIcaRetention").blur();
                    $("#CardVoucherFteRetention").blur();

                    $.ajax({
                        async: false,
                        url: ACC_ROOT + "Transaction/GetInternalBallotInformation",
                        data: { "paymentCode": paymentCode },
                        success: function (depositData) {
                            if (depositData.length > 0) {
                                //DEPLIEGUE BOLETA INTERNA
                                $("#InternalBallotDateCardVoucher").val(depositData[0].RegisterDate);
                                $("#InternalBallotNumberCardVoucher").val(depositData[0].PaymentTicketCode);
                                $("#ReceivingBankCardVoucher").val(depositData[0].BankDescription);
                                $("#ReceivingAccountNumberCardVoucher").val(depositData[0].ReceivingAccountNumber);
                            }
                        }
                    });

                    $.ajax({
                        async: false,
                        url: ACC_ROOT + "Transaction/GetDepositInformation",
                        data: { "paymentCode": paymentCode },
                        success: function (depositInfo) {

                            if (depositInfo.length > 0) {
                                //DEPLIEGUE VOUCHERS DEPOSITADOS
                                $("#DateDepositCardVoucher").val(depositInfo[0].DepositRegisterDate);
                                $("#DepositBallotNumberCardVoucher").val(depositInfo[0].PaymentBallotNumber);
                                $("#DepositTransactionCardVoucher").val(depositInfo[0].PaymentBallotCode);

                                $.ajax({
                                    async: false,
                                    url: ACC_ROOT +  "Transaction/GetRejectedInformation",
                                    data: { "paymentCode": paymentCode },
                                    success: function (rejectionData) {
                                        if (rejectionData.length > 0) {
                                            //DEPLIEGUE VOUCHERS RECHAZADOS
                                            $("#CardVoucherDateRejection").val(rejectionData[0].RejectionDate);
                                            $("#CardVoucherReasonRejection").val(rejectionData[0].RejectionDescription);
                                            $("#CardVoucherRejectedTransaction").val(rejectionData[0].RejectedPaymentCode);

                                            $.ajax({
                                                async: false,
                                                url: ACC_ROOT + "Transaction/GetRegularizedInformation",
                                                data: { "paymentCode": paymentCode },
                                                success: function (regularizedData) {
                                                    if (regularizedData.length > 0) {
                                                        //DEPLIEGUE VOUCHERS REGULARIZADOS
                                                        $("#RegularizedDateCard").val(regularizedData[0].RegisterDate);
                                                        $("#RegularizedTransactionCard").val(regularizedData[0].RegularizePaymentCode);
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
    }
    else if (nameDialog == "ModalRejectVoucher") {
        $("#RejectReceivingBank").val("");
        $("#RejectReceivingAccount").val("");
        $("#RejectDateDeposit").val("");
        $("#RejectDepositTransaction").val("");
        $("#RejectTicketDepositNumber").val("");

        message = Resources.RejectCard;

        $.ajax({
            async: false,
            url: ACC_ROOT +  "CardVoucherSearch/GetInformationPayment",
            data: { "paymentCode": paymentCode },
            success: function (data) {
                if (data.length > 0) {

                    //Detalle de Vouchers
                    $("#RejectVoucherDuct").val(data[0].Description);
                    $("#RejectVoucherCardNumber").val(rowId[0].DocumentNumber);
                    $("#RejectVoucherVoucherNumber").val(data[0].Voucher);
                    $("#RejectVoucherBillCode").val(data[0].CollectCode);
                    $("#RejectVoucherTransactionNumber").val(data[0].TechnicalTransaction)
                    billId = data[0].CollectCode;
                    $("#RejectVoucherCurrency").val(data[0].CurrencyDescription);
                    $("#RejectVoucherAmount").val(data[0].Amount);
                    $("#RejectVoucherBusinessNameCard").val(data[0].Holder);
                    $("#RejectVoucherRegisterDate").val(data[0].DatePayment);
                    $("#RejectTax").val(data[0].Taxes);
                    $("#RejectVoucherAmount").blur();
                    $("#RejectTax").blur();
                    $("#RejectPayer").val(data[0].Name);

                    $.ajax({
                        async: false,
                        url: ACC_ROOT + "Transaction/GetInternalBallotInformation",
                        data: { "paymentCode": paymentCode },
                        success: function (depositData) {

                            if (depositData.length > 0) {
                                //DEPLIEGUE BOLETA INTERNA
                                $("#RejectReceivingBank").val(depositData[0].BankDescription);
                                $("#RejectReceivingAccount").val(depositData[0].ReceivingAccountNumber);
                            }
                        }
                    });

                    $.ajax({
                        async: false,
                        url: ACC_ROOT + "Transaction/GetDepositInformation",
                        data: { "paymentCode": paymentCode },
                        success: function (depositInfo) {

                            if (depositInfo.length > 0) {
                                //DEPLIEGUE VOUCHERS DEPOSITADOS
                                $("#RejectDateDeposit").val(depositInfo[0].DepositRegisterDate);
                                $("#RejectDepositTransaction").val(depositInfo[0].PaymentBallotCode);
                                $("#RejectTicketDepositNumber").val(depositInfo[0].PaymentBallotNumber);
                            }
                        }
                    });
                }
            }
        });
    }

    dialogName.UifModal('showLocal', message);
}

function refreshGridPolicies() {
    var controller = ACC_ROOT + "CheckingRejection/GetPoliciesByBillId?billId=" + billId;
    $("#VoucherPoliciesRejection").UifDataTable({ source: controller });
}


function refreshGridtaxes() {
    var controller = ACC_ROOT + "CardVoucherSearch/GetTaxInformationByPaymentId?paymentid=" + paymentId;
    $("#tableTaxes").UifDataTable({ source: controller });
}


