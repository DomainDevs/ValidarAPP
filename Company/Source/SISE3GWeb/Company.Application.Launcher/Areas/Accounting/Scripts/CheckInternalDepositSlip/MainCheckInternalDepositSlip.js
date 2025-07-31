/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
var accountCurrency = -1;
var issuingBankCode;
var checkNumber = -1;
var validateEffective = 0;
var errorNumber = 0;
var controlError = 0;
var ballotNumber;
var issuingBankName;
var accountNumber;
var cashAmount = "0";
var totalChecks;
var totalBallot;
var registerDate;

setTimeout(function () {
    registerDate = $("#ViewBagAccountingDate").val();
}, 2000);

var index = 0;
var ids = 0;
var bankId = -1;

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
    PaymentId: null,
    Select: null
};

var oParameterModel = {
    Id: 0,
    Name: "",
    Description: "",
    IsObject: false,
    Branch: 0,
    DateFrom: null,
    DateTo: null,
    PaymentTicketCode: null,
    PaymentMethodTypeCode: null,
    BranchName: "",
};


/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/


if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#Branch").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#Branch").removeAttr("disabled");
}

//FORMATOS/#CARACTERES
$("#CashAmount").blur(function () {
    $("#alert").UifAlert('hide');
    var cashAmountOriginal = RemoveFormatMoney($('#CashAmount').val());
    if (cashAmountOriginal == 0) {
        $('#CashAmount').val('');
        return;
    }
    $('#CashAmount').val(FormatMoneySymbol(cashAmountOriginal));
    if (!isNull($("#Branch").UifSelect('getSelected'))) {
        MainCheckInternalDepositSlipRequest.ValidateCashAmount($("#Branch").UifSelect('getSelected'),
            accountCurrency, cashAmountOriginal, registerDate, 0)
            .done(function (data) {
                if (data.success) {
                    if (data.result == 0) {
                        $("#alert").UifAlert('hide');
                    } else {
                        $("#alert").UifAlert('show', Resources.ErrorMaximumCashBallotTicket + " " + FormatMoney(data.result), "danger");
                    }
                } else {
                    $("#alert").UifAlert('show', data.result, "danger");
                }
            });
    }
});

$('#TableAChecksDialog').on('rowDelete', function (event, data, position) {
    index = position;
    $('#alert').UifAlert('hide');
    $('#modalDeleteBallot').appendTo("body").modal('show');
});

$("#DeleteModal").on('click', function () {
    $('#modalDeleteBallot').modal('hide');
    $('#TableAChecksDialog').UifDataTable('deleteRow', index);
    AddCheckSelected();
});

/**********************************
//BOTON DE IMPRESIÒN BOLETA DEL DIALOGO / Impresion de boleta interna
/**********************************/
function PrintInternalBallot(event, modal) {
    $("#" + modal).UifModal('hide');
    LoadReport($("#paymentTicketCode").val(), $("#paymentMethodTypeCode").val(), $("#branchCodeReport").val(), $("#branchNameReport").val());
}


$('#Branch').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        SetDataFieldsAccountBankEmpty();
        var controller = ACC_ROOT + "CheckControl/GetBankBranchsByBranchId?branchId=" + $("#Branch").val();    
        $("#selectReceivingBank").UifSelect({ source: controller });
    }
    else {
        $("#selectReceivingAccountNumber").UifSelect();
        $("#TableChecksDialog").dataTable().fnClearTable();
        $("#TableAChecksDialog").dataTable().fnClearTable();
    }
});

$(document).on('ready', function () {
    $("#Branch").UifSelect('setSelected', $("#Branch").UifSelect('getSelected'));
    $('#Branch').on('binded', function (event, data, index) {
        $("#Branch").trigger('change');
    });

    MainCheckInternalDepositSlipRequest.GeBranchesforUser().done(function (data) {
        if (data != undefined && data.data != undefined) {
            $("#Branch").UifSelect({ sourceData: data.data });

            if ($('#BranchDefault').val() != '') {
                $("#Branch").UifSelect('setSelected', $("#BranchDefault").val());
                $("#Branch").trigger('change');
            }
        }
    });
});


/*************************************/
// Select Banco Receptor
/*************************************/
$('#selectReceivingBank').on('itemSelected', function (event, selectedItem) {
                
    if ($('#selectReceivingBank').val() != "") {        

        var controller = ACC_ROOT + "CheckControl/GetAccountByBankIdByBranchId?bankId=" + $('#selectReceivingBank').val() + '&branchId=' + $("#Branch").val();        

        $("#selectReceivingAccountNumber").UifSelect({ source: controller });
        SetDataFieldsAccountBankEmpty();
    }
    else {
        $("#selectReceivingAccountNumber").UifSelect();
        $("#TableChecksDialog").dataTable().fnClearTable();
        $("#TableAChecksDialog").dataTable().fnClearTable();
    }
});

            
/*************************/
// Select Número de Cuenta
/************************/
$('#selectReceivingAccountNumber').on('itemSelected', function (event, selectedItem) {

    if ($('#selectReceivingAccountNumber') != "") {

        $.ajax({
            type: "POST",
            url: ACC_ROOT + "CheckInternalDepositSlip/GetAccountCurrencyByBankId",
            data: JSON.stringify({ "bankId": $("#selectReceivingBank").val(), "accountNumber": $("#selectReceivingAccountNumber option:selected").html() }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data != null) {
                    $("#AccountCurrency").val(data.Currency.Description);
                    accountCurrency = data.Currency.Id;
                    SetDataFieldsAccountBankEmpty();
                    $("#CashAmount").removeAttr("disabled");
                }
            }
        });
    }
    else {
        $("#AccountCurrency").UifSelect();
        accountCurrency = -1;
        $("#CashAmount").attr("disabled", "true");
        $("#TableChecksDialog").dataTable().fnClearTable();
        $("#TableAChecksDialog").dataTable().fnClearTable();
    }
});

$('#SelectPaymentMethodType').on('itemSelected', function (event, selectedItem) {

    if ($('#SelectPaymentMethodType').val() == $("#ViewBagParamPaymentMethodCash").val()) {
        $("#btnSearch").attr("disabled", true);
    } else {
        $("#btnSearch").attr("disabled", false);
    }

    $("#inputIssuingBank").val('');
    issuingBankCode = -1;
    $("#CheckNumber").val('');

    $("#TableChecksDialog").dataTable().fnClearTable();
    $("#TableAChecksDialog").dataTable().fnClearTable();

    oTblChecks = {
        PaymentTicket: []
    };
});

/************************************/
//AutoComplete Get Bank Name
/***********************************/
$('#inputIssuingBank').on('itemSelected', function (event, selectedItem) {
    issuingBankCode = selectedItem.bankId;
});

/***********************************/
//Button Search
/***********************************/
$('#btnSearch').click(function () {
    
    if ($("#InternalBallot").valid()) {
        if ($('#SelectPaymentMethodType').val() != "") {
            if ($("#selectReceivingAccountNumber").val() != "") {
                if ($("#Branch").val() != "") {
                    $("#TableChecksDialog").UifDataTable();
                    RefreshGrid();
                }
                else {
                    $("#alert").UifAlert('show', Resources.SelectBranch, "danger");
                }
            }
            else {
                $("#alert").UifAlert('show', Resources.SelectAccountNumber, "danger");
            }
        }
        else {
            $("#alert").UifAlert('show', Resources.PaymentTypeRequired, "danger");
        }
    }
});

$('#TableChecksDialog').on('rowAdd', function (event, data) {
    //var existsCheckNumber = false;
    var dataChecksDeposit = $("#TableChecksDialog").UifDataTable("getSelected");
    var dataSelectedChecks = $("#TableAChecksDialog").UifDataTable("getData");
    var totalChecks = 0;

    if (dataChecksDeposit != null) {
        for (var i in dataChecksDeposit) {
            if (DuplicateRecordsCheckInternal(dataChecksDeposit[i].PaymentCode) == false) {
                $('#TableAChecksDialog').UifDataTable('addRow', dataChecksDeposit[i]);

                var valueChecks = parseFloat(ClearFormatCurrency(dataChecksDeposit[i].IncomeAmount.toString().replace("", ",")));
                if ($("#CheckAmount").val() == "")
                    totalChecks = 0;
                else
                    totalChecks = parseFloat(ClearFormatCurrency($("#CheckAmount").val().replace("", ",")));

                totalChecks += valueChecks;

                $("#CheckAmount").val(totalChecks);
                var checkAmount = $("#CheckAmount").val();
                $("#CheckAmount").val("$ " + NumberFormatDecimal(checkAmount, "2", ".", ","));
            }
            else {
                $("#alert").UifAlert('show', Resources.MInternalBallotDuplicateRecord, "danger");
            }
        }
    }
});

$('#btnAdd').hide();

$('#btnAdd').click(function () {
    //var existsCheckNumber = false;
    var dataChecksDeposit = $("#TableChecksDialog").UifDataTable("getSelected");
    var dataSelectedChecks = $("#TableAChecksDialog").UifDataTable("getData");
    var totalChecks = 0;

    if (dataChecksDeposit != null) {
        for (var i in dataChecksDeposit) {
            if (DuplicateRecordsCheckInternal(dataChecksDeposit[i].PaymentCode) == false) {
                $('#TableAChecksDialog').UifDataTable('addRow', dataChecksDeposit[i]);

                var valueChecks = parseFloat(ClearFormatCurrency(dataChecksDeposit[i].IncomeAmount.toString().replace("", ",")));
                if ($("#CheckAmount").val() == "")
                    totalChecks = 0;
                else
                    totalChecks = parseFloat(ClearFormatCurrency($("#CheckAmount").val().replace("", ",")));

                totalChecks += valueChecks;

                $("#CheckAmount").val(totalChecks);
                var checkAmount = $("#CheckAmount").val();
                $("#CheckAmount").val("$ " + NumberFormatDecimal(checkAmount, "2", ".", ","));
            }
            else {
                $("#alert").UifAlert('show', Resources.MInternalBallotDuplicateRecord, "danger");
            }
        }
    }
});

$("#SaveTicket").click(function () {
    $("#InternalBallot").validate();
    if ($("#InternalBallot").valid()) {
        if (ValidateFieldsCheckInternal()) {
            saveBallot();
            // Se deja no restrictivo para pruebas
            /*if (!isNull($("#Branch").UifSelect('getSelected'))) {
                var cashAmountAdmitted = RemoveFormatMoney($("#CashAmount").val());
                if (cashAmountAdmitted > 0) {
                    MainCheckInternalDepositSlipRequest.ValidateCashAmount($("#Branch").UifSelect('getSelected'),
                        accountCurrency, cashAmountAdmitted, registerDate, 0)
                        .done(function (data) {
                            if (data.success) {
                                if (data.result == 0) {
                                    saveBallot();
                                } else {
                                    $("#alert").UifAlert('show', Resources.ErrorMaximumCashBallotTicket + FormatMoney(data.result), "danger");
                                }
                            } else {
                                $("#alert").UifAlert('show', data.result, "danger");
                            }
                        });
                } else {
                    saveBallot();
                }
            }*/
        } else {
            $("#alert").UifAlert('show', Resources.InternalBallotValidateSaveMessage, "danger");
        }
    }
});

function saveBallot() {
    lockScreen();
    MainCheckInternalDepositSlipRequest.SaveInternalBallotDeposit(SetTicketCheckInternal())
        .done(function (data) {
            if (data.success == false) {
                $("#alert").UifAlert('show', data.result, "danger");
            }
            else {
                $("#BallotForm").validate();
                $("#paymentTicketCode").val(data[0].Id);
                $("#paymentMethodTypeCode").val(data[0].MethodType);
                ballotNumber = ("000000000" + data[0].Id).slice(-10);
                $("#paymentTicketItemId").text(ballotNumber);
                issuingBankName = $("#selectReceivingBank option:selected").html();
                $("#receivingBankName").text(issuingBankName);
                accountNumber = $("#selectReceivingAccountNumber option:selected").html();
                $("#accountBank").text(accountNumber);
                $("#dateBallot").text(data[0].Date);
                accountCurrency = $("#AccountCurrency").val();
                $("#accountCurrency").text(accountCurrency);
                $("#userBallot").text(data[0].User);

                $("#cashBallot").text(FormatMoneySymbol($("#CashAmount").val()));
                $("#totalChecks").text(FormatMoneySymbol(data[0].Total));
                var totalBallot = fix(RemoveFormatMoney($("#CashAmount").val()) + parseFloat(data[0].Total));
                $("#totalBallot").text(FormatMoneySymbol(totalBallot));

                $("#branchCodeReport").val($("#Branch").val());
                $("#branchNameReport").val($("#Branch option:selected").text());
                $('#formPolicies').hide();
                $('#btnSendRequestsPolicies').hide();
                $('#buttonsModalSummary').hide();
                $('#modalPoliciesSummary').find("#formPolicies").hide();
                $('#ModalSaveIBallot').UifModal('showLocal', Resources.InternalSlipSuccessfulSaved);
                //$('#ModalSaveIBallot').appendTo("body").modal('show');
            }
            SetDataFieldsAEmpty();
            oTblChecks = {
                PaymentTicket: []
            };
        })
        .always(function () {
            unlockScreen();
        });
}

$("#CancelTicket").click(function () {
    SetDataFieldsAEmpty();
    CleanObject();
});

$("#Clean").click(function () {
    $("#CancelTicket").click();
    $("#selectReceivingBank").UifSelect();
    $("#InternalBallot").formReset();
    $('#SelectPaymentMethodType-error').remove();
    $('#selectReceivingBank-error').remove();
    $('#selectReceivingAccountNumber-error').remove();

});


/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

function RefreshGrid() {

    checkNumber = $("#CheckNumber").val();
    var controller = ACC_ROOT + "CheckInternalDepositSlip/GetChecksToDepositBallot?branchId=" + $('#Branch').val() + "&paymentMethodTypeCode=" +
                     $('#SelectPaymentMethodType').val() + "&issuingBankCode=" + issuingBankCode + "&checkNumber=" + checkNumber +
                    "&currencyCode=" + accountCurrency;
    $("#TableChecksDialog").UifDataTable({ source: controller });
}

/*******************************************************************/
//Validar que no se agregue duplicados en cheques seleccionados
/*****************************************************************/
function DuplicateRecordsCheckInternal(paymentCode) {
    var result = false;
    var dataSelectedChecks = $("#TableAChecksDialog").UifDataTable("getData");

    if (dataSelectedChecks.length > 0) {
        for (var f in dataSelectedChecks) {
            if (paymentCode == dataSelectedChecks[f].PaymentCode) {
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

function ValidateFieldsCheckInternal() {
    if ($("#selectReceivingAccountNumber").val() == "") {
        return false;
    } else {
        var checks = $("#TableAChecksDialog").UifDataTable('getData').length;

        if ((isMoneyEmpty($("#CashAmount").val()) && checks == 0)
            || (isMoneyEmpty($("#CheckAmount").val()) && checks > 0)) {
            return false;
        }
    }
    return true;
}

function SetTicketCheckInternal() {

    var issuingBankCode = $("#selectReceivingBank").val();
    var issuingBankName = $("#selectReceivingBank option:selected").html();
    var accountNumber = $("#selectReceivingAccountNumber option:selected").html();
    var cashAmount = RemoveFormatMoney($("#CashAmount").val());
    var paymentMethodTypeCode = $("#SelectPaymentMethodType").val();
    var currencyId = accountCurrency;
    var branchCode = $("#Branch").val();
    var branchName = $("#Branch option:selected").html();

    var ids = $("#TableAChecksDialog").UifDataTable("getData");
    
    if (ids.length > 0) {
                
        for (var i in ids) {
            var rowid = ids[i];
            CleanObject();
            oPaymentTicket.Id = rowid.PaymentTicketItemId;
            oPaymentTicket.Branch = branchCode;
            oPaymentTicket.BranchName = branchName;
            oPaymentTicket.Bank = issuingBankCode;
            oPaymentTicket.BankDescription = issuingBankName;
            oPaymentTicket.Checks = rowid.CheckNumber;
            oPaymentTicket.DatePayment = rowid.CheckDate;
            oPaymentTicket.PaymentMethodId = paymentMethodTypeCode;
            oPaymentTicket.Currency = rowid.CurrencyCode;
            oPaymentTicket.IncomeAmount = rowid.IncomeAmount;
            oPaymentTicket.Amount = ReplaceDecimalPoint(RemoveFormatMoney(rowid.IncomeAmount));
            oPaymentTicket.PaymentId = rowid.PaymentCode;
            oPaymentTicket.Select = rowid.Select;
            oPaymentTicket.AccountNumber = accountNumber;
            oPaymentTicket.CashAmount = ReplaceDecimalPoint(cashAmount);
            oTblChecks.PaymentTicket.push(oPaymentTicket);
        }
    }
    else {
        if (ids.length == 0) {

            var paymentMethod = parseInt($("#ViewBagParamPaymentMethodCash").val());  //paymentMethodTypeCode = 1;
            paymentMethodTypeCode = paymentMethod;
            CleanObject();
            oPaymentTicket.Branch = branchCode;
            oPaymentTicket.BranchName = branchName;
            oPaymentTicket.Bank = issuingBankCode;
            oPaymentTicket.BankDescription = issuingBankName;
            oPaymentTicket.PaymentMethodId = paymentMethodTypeCode;
            oPaymentTicket.Currency = currencyId;
            oPaymentTicket.AccountNumber = accountNumber;
            oPaymentTicket.CashAmount = ReplaceDecimalPoint(cashAmount);

            oTblChecks.PaymentTicket.push(oPaymentTicket);
        }
    }//else

    return oTblChecks;
}

function CleanObject() {
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
        PaymentId: null,
        Select: null
    };
    ids = 0;
    bankId = null;
}


function SetDataFieldsEmpty() {
    $("#SelectPaymentMethodType").val('');
    $("#inputIssuingBank").val('');
    issuingBankCode = -1;
    $("#CheckNumber").val('');
    $("#TableChecksDialog").dataTable().fnClearTable();
}

function SetDataFieldsAEmpty() {
    $("#selectReceivingBank").val('');
    $("#CashAmount").val('');
    $("#CheckNumber").val('');
    $("#SelectPaymentMethodType").val(' ');

    $("#CheckAmount").val('');
    $("#selectReceivingAccountNumber").val('');
    $("#selectReceivingBank").trigger('change');

    $("#inputIssuingBank").val('');
    issuingBankCode = -1;
    $("#CheckNumber").val('');
    $("#AccountCurrency").val('');
    accountCurrency = 0;

    $("#TableChecksDialog").dataTable().fnClearTable();
            
    $("#TableAChecksDialog").dataTable().fnClearTable();

    $("#Branch").UifSelect('setSelected', $("#BranchDefault").val());
    $("#Branch").trigger('change');
}//setDataFieldsAEmpty

function SetDataFieldsAccountBankEmpty() {
    $("#SelectPaymentMethodType").val('');
    $("#inputIssuingBank").val('');
    issuingBankCode = -1;
    $("#CheckNumber").val('');
    $("#CheckAmount").val('');
    $("#TableChecksDialog").dataTable().fnClearTable();
    $("#TableAChecksDialog").dataTable().fnClearTable();
}

function LoadReport(paymentTicketId, paymentMethodTypeId, branchId, branchName) {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT +  "Report/LoadInternalBallotReport",
        data: {"paymentTicketCode": paymentTicketId, "paymentMethodTypeCode": paymentMethodTypeId,
               "branchId": branchId, "branchName": branchName
        },
        success: function (data) {
            window.open(ACC_ROOT + "Report/ShowInternalBallotReport", 'mywindow', 'fullscreen=yes, scrollbars=auto');
        }
    });
}//loadReport

//Agrega cheques seleccionados a cheques a depositar
function AddCheckSelected() {
    var ids = $("#TableAChecksDialog").UifDataTable("getData");
    var totalChecks = 0;
    if (ids.length > 0) {
        for (var i in ids) {
            var rowid = ids[i];
            var valueChecks = parseFloat(ClearFormatCurrency(rowid.IncomeAmount.toString().replace("", ",")));
            totalChecks += valueChecks;
        }//for
        $("#CheckAmount").val(totalChecks);
        var checkAmount = $("#CheckAmount").val();
        $("#CheckAmount").val("$ " + NumberFormatDecimal(checkAmount, "2", ".", ","));
    }
    else {
        $("#CheckAmount").val("");
    }
}//addCheckSelected




