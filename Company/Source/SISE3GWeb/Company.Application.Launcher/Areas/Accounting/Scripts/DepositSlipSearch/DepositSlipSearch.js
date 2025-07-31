/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
var searchBankId = -1;
var startDate = "";
var endDate = "";
var paymentTicketId = -1;
var rowSelected = null;
var issuingBankId = "";
var brachId = "";
var paymentStatus;
var checkDate;
var currencyId = "";
var option = 0;
var deletePosition = 0;
var registerDate = null;
var receivingBankId = "";
var idsDelete = [];
var idsLog = [];
var deletedRow = null;
var idsUpdate = [];
var successPaymentMethodType = "";

var arrayTransferChecks = {
    PaymentTicket: []
};

var oPaymentTicket = {
    Id: null,
    Bank: null,
    BankDescription: null,
    Branch: null,
    BranchName: null,
    AccountNumber: null,
    CashAmount: null,
    Checks: null,
    DatePayment: null,
    PaymentMethodId: null,
    Currency: null,
    Amount: null,
    PaymentId: null,
    Select: null,
    PaymentTicketId: null,
    PaymentTicketItemId: null,
    DeleteRecords: null,
    LogRecords: null,
    UpdateRecords: null
};

/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

$("#cancelSlip").hide();
$("#editSlip").hide();

$("#DepositSlipEditCashAmount").blur(function () {
    var cashAmountOriginal = RemoveFormatMoney($('#DepositSlipEditCashAmount').val());
    if (cashAmountOriginal == 0) {
        $('#DepositSlipEditCashAmount').val('');
        return;
    }
    $("#DepositSlipEditCashAmount").val(FormatMoneySymbol(cashAmountOriginal));
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "CheckInternalDepositSlip/ValidateCashAmount",
        data: {
            "branchId": brachId, "currencyId": currencyId,
            "cashAmountAdmitted": ReplaceDecimalPoint(cashAmountOriginal),
            "registerDate": registerDate, "paymentTicketId": parseInt($("#DepositSlipEditPaymentTicketCode").val())
        },
        success: function (data) {
            if (data.success) {
                if (data.result == 0) {
                    $("#duplicateAlert").UifAlert('hide');
                } else {
                    $("#duplicateAlert").UifAlert('show', Resources.ErrorMaximumCashBallotTicket + FormatMoney(data.result), "danger");
                }
            }
            else {
                $("#duplicateAlert").UifAlert('show', data.result, "danger");
            }
        }
    });
});

$("#DepositSlipEditCheckAmount").blur(function () {
    $("#DepositSlipEditCheckAmount").val(FormatCurrency($("#DepositSlipEditCheckAmount").val()));
});

//Valida que no ingresen una fecha invalida.
$("#StartDate").blur(function () {

    if ($("#StartDate").val() != '') {

        if (IsDate($("#StartDate").val()) == true) {
            if ($("#EndDate").val() != '') {
                if (CompareDates($("#StartDate").val(), $("#EndDate").val())) {
                    $("#StartDate").val(getCurrentDate);
                }
            }
        } else {
            $("#alert").UifAlert('show', Resources.InvalidDates, "danger");
            $("#StartDate").val("");
        }
    }
});


$("#EndDate").blur(function () {
    if ($("#EndDate").val() != '') {

        if (IsDate($("#EndDate").val()) == true) {
            if ($("#StartDate").val() != '') {
                if (!CompareDates($("#EndDate").val(), $("#StartDate").val())) {
                    $("#EndDate").val(getCurrentDate);
                }
            }
        } else {
            $("#alert").UifAlert('show', Resources.InvalidDates, "danger");
            $("#EndDate").val("");
        }
    }
});


//Controla que la fecha final sea mayor a la inicial
$('#StartDate').on('datepicker.change', function (event, date) {

    if ($("#EndDate").val() != "") {
        if (compare_dates($('#StartDate').val(), $("#EndDate").val())) {
            $("#alert").UifAlert('show', Resources.ValidateDateTo, "warning");
            $("#StartDate").val('');
        } else {
            $("#StartDate").val($('#StartDate').val());
            $("#alert").UifAlert('hide');
        }
    }
});


$('#EndDate').on('datepicker.change', function (event, date) {

    if ($("#StartDate").val() != "") {
        if (compare_dates($("#StartDate").val(), $('#EndDate').val())) {
            $("#alert").UifAlert('show', Resources.ValidateDateFrom, "warning");
            $("#EndDate").val('');
        } else {
            $("#EndDate").val($('#EndDate').val());
            $("#alert").UifAlert('hide');
        }
    }
});

$("#SearchDepositSlip").click(function () {
    //$("#depositSlipSearchForm").validate();
    if ($("#depositSlipSearchForm").valid()) {
        startDate = $("#StartDate").val();
        endDate = $("#EndDate").val();
        if ($("#InternalBallotNumber").val() == "") {
            paymentTicketId = -1;
        } else {
            paymentTicketId = $("#InternalBallotNumber").val();
        }
        var controller = ACC_ROOT + "DepositSlipSearch/SearchInternalBallot?bankId=" + searchBankId +
                                    "&startDate=" + startDate + "&endDate=" + endDate +
                                    "&paymentTicketId=" + paymentTicketId;
        $("#SearchBallotsTable").UifDataTable({ source: controller });
    }
});

$("#clear").click(function () {
    clearFieldsDepositSlip();
});

$('#Bank').on('itemSelected', function (event, selectedItem) {
    searchBankId = selectedItem.Id;
});

//Evento de selección en tabla de movimientos
$('#SearchBallotsTable').on('rowSelected', function (event, data) {
    rowSelected = data;
    if (data.Status == 1) {
        $("#cancelSlip").show();
        $("#editSlip").show();
    } else if (status != 1) {
        $("#cancelSlip").hide();
        $("#editSlip").hide();
    }
});

//Evento de edición de la tabla
$('#SearchBallotsTable').on('rowEdit', function (event, data) {
    loadInternalBallotsDetails(data.PaymentTicketCode);
    $('#checkDetailsModal').UifModal('showLocal', Resources.SearchBallotCheckDetails + " " + data.PaymentTicketCode);
});

//click en anular boleta
$("#cancelSlip").click(function () {
    option = 1;
    $("#deleteMessageModal").text(Resources.InternalBallotCancelationQuestion);
    $('#modalDeleteBallot').UifModal('showLocal', Resources.Annul);
});


//Click en botón Si de modal de advertencia de Anulación
function DeleteModalBallot(modal) {
    Delete(option, modal);
}


$("#editSlip").click(function () {
    $("#duplicateAlert").UifAlert('hide');
    arrayTransferChecks = {
        PaymentTicket: []
    };
    loadInternalBallotDeposit();
    $('#editModal').UifModal('showLocal', Resources.InternalBallotEditModalTitle + ' No. ' + rowSelected.PaymentTicketCode);
    $("#editModal").find("#DepositSlipEditPaymentTicketCode").val(rowSelected.PaymentTicketCode);
    //setTimeout(function () {
    getChecksTotal();
    //}, 500);
});

//cuando se cierra el modal de edición
$('#editModal').on('hidden.bs.modal', function () {
    clearEditModalFields();
    //$('#editModal').UifModal('destroy');
});

///////////////////////////////////////////////////
//  Muestra mensaje de confirmación              //
//////////////////////////////////////////////////
function showConfirm() {
    var message = Resources.DeleteConfirmationMessage;
    message = message.replace("&#191;", "¿");

    $.UifDialog('confirm', { 'message': message, 'title': Resources.Delete }, function (result) {
        if (result) {
            option = 2;
            Delete(option);
        }
    });
};

//para que funcionen los controles dentro del modal
$(document).ready(function () {

    //FUNCIONALIDAD DEL MODAL DE EDICIÓN

    //borrar cheques que contiene la boleta 
    $("#SelectedChecksToDeposit").on('rowDelete', function (event, data, position) {
        deletedRow = data;
        option = 2;
        deletePosition = position;
        showConfirm();
    });

    //click en el botón buscar del modal
    $("#depositSlipEditSearch").click(function () {
        //$("#checkToDepositForm").validate();
        if ($("#checkToDepositForm").valid()) {
            var paymentMethodTypeCode = $("#DepositSlipEditPaymentMethod").val();
            var controller = ACC_ROOT + "CheckInternalDepositSlip/GetChecksToDepositBallot?branchId=" + brachId +
                                        "&paymentMethodTypeCode=" + paymentMethodTypeCode +
                                        "&issuingBankCode=" + issuingBankId + "&checkNumber=" + $("#DepositSlipEditCheckNumber").val() +
                                        "&currencyCode=" + currencyId;
            $("#SearchedChecksToDeposit").UifDataTable({ source: controller });
        }
    });

    //autocomplete de banco en modal de edición.
    $('#DepositSlipEditIssuingBank').on('itemSelected', function (event, selectedItem) {
        issuingBankId = selectedItem.Id;
    });

    //click en botón limpiar de modal de edición
    $("#depositSlipEditClear").click(function () {
        $("#DepositSlipEditPaymentMethod").val("");
        $("#DepositSlipEditCheckNumber").val("");
        $("#DepositSlipEditIssuingBank").val("");
        $("#DepositSlipEditCashAmount").val("");
        issuingBankId = "";
        $("#SearchedChecksToDeposit").dataTable().fnClearTable();
    });

    //Agrega una boleta al grid 
    $('#SearchedChecksToDeposit').on('rowAdd', function (event, data) {
        var data = $("#SearchedChecksToDeposit").UifDataTable('getSelected');
        if (data != null) {
            addCheck();
        }
        getChecksTotal();
    });
});

//BOTÓN ACEPTAR DEL MODAL DE EDICIÓN 
function EditDepositSlip() {
    if (ValidateFieldsDepositSlip()) {
        $.ajax({
            type: "POST",
            async: false,
            url: ACC_ROOT + "CheckInternalDepositSlip/UpdateInternalBallot",
            data: { "checksModel": setTicket() }
        }).done(function (data) {
            if (data.success === true) {
                successPaymentMethodType = data.result.MethodType;
                var bankName = $("#DepositSlipEditReceivingBank").val();
                $("#DepositSlipSuccessReceivingBank").text(bankName);
                $("#DepositSlipSuccessPaymentTicketCode").text(("000000000" + data.result.Id).slice(-10));
                var accountNumber = $("#DepositSlipEditReceivingAccountNumber").val();
                $("#DepositSlipSuccessReceivingAccountNumber").text(accountNumber);
                $("#DepositSlipSuccessDate").text(data.result.Date);
                var currencyName = $("#DepositSlipEditCurrency").val();
                $("#DepositSlipSuccessCurrency").text(currencyName);
                $("#DepositSlipSuccessUser").text(data.result.User);
                $("#DepositSlipSuccessTotal").text(data.result.Total);
                $("#DepositSlipSuccessCashTotal").text(data.result.CashAmount);
                $("#DepositSlipSuccessChecksTotal").text(data.result.Amount);
                $('#editModal').UifModal('hide');
                $('#successModal').UifModal('showLocal', Resources.InternalSlipSuccessfulSaved);
            } else {
                $.UifNotify('show', { type: 'danger', message: Resouces.ErrorUpdatingPaymentTicket, autoclose: true });
            }
        }).fail(function () {
            $.UifNotify('show', { type: 'danger', message: Resouces.ErrorUpdatingPaymentTicket, autoclose: true });
        });
        // Se deja no restrictivo para pruebas
        /*$.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "CheckInternalDepositSlip/ValidateCashAmount",
            data: {
                "branchId": brachId, "currencyId": currencyId,
                "cashAmountAdmitted": RemoveFormatMoney($("#DepositSlipEditCashAmount").val()),
                "registerDate": registerDate, "paymentTicketId": parseInt($("#DepositSlipEditPaymentTicketCode").val())
            },
            success: function (data) {
                if (data.success) {
                    if (data.result == 1) {
                        $.ajax({
                            type: "POST",
                            async: false,
                            url: ACC_ROOT + "CheckInternalDepositSlip/UpdateInternalBallot",
                            data: { "checksModel": setTicket() }
                        }).done(function (data) {
                            if (data.success === true) {
                                successPaymentMethodType = data.result.MethodType;
                                var bankName = $("#DepositSlipEditReceivingBank").val();
                                $("#DepositSlipSuccessReceivingBank").text(bankName);
                                $("#DepositSlipSuccessPaymentTicketCode").text(("000000000" + data.result.Id).slice(-10));
                                var accountNumber = $("#DepositSlipEditReceivingAccountNumber").val();
                                $("#DepositSlipSuccessReceivingAccountNumber").text(accountNumber);
                                $("#DepositSlipSuccessDate").text(data.result.Date);
                                var currencyName = $("#DepositSlipEditCurrency").val();
                                $("#DepositSlipSuccessCurrency").text(currencyName);
                                $("#DepositSlipSuccessUser").text(data.result.User);
                                $("#DepositSlipSuccessTotal").text(data.result.Total);
                                $("#DepositSlipSuccessCashTotal").text(data.result.CashAmount);
                                $("#DepositSlipSuccessChecksTotal").text(data.result.Amount);
                                $('#editModal').UifModal('hide');
                                $('#successModal').UifModal('showLocal', Resources.InternalSlipSuccessfulSaved);
                            } else {
                                $.UifNotify('show', { type: 'danger', message: Resouces.ErrorUpdatingPaymentTicket, autoclose: true });
                            }
                        }).fail(function () {
                            $.UifNotify('show', { type: 'danger', message: Resouces.ErrorUpdatingPaymentTicket, autoclose: true });
                        });
                    } else {
                        $("#duplicateAlert").UifAlert('show', Resources.ValidateCash, "danger");
                    }
                }
                else {
                    $("#duplicateAlert").UifAlert('show', data.result, "danger");
                }
            }
        });*/
    } else {
        $("#duplicateAlert").UifAlert('show', Resources.InternalBallotValidateSaveMessage, "danger");
    }
}

//BOTÓN DE IMPRESIÓN DEL DIÁLOGO
function PrintDepositSlip(modal) {
    loadReportDepositSlip(paymentTicketId, successPaymentMethodType, brachId, $("#DepositSlipEditBranch").val());
    $('#editModal').UifModal('hide');
    $("#" + modal).UifModal('hide');
    $("#SearchDepositSlip").trigger('click');
}

//cuando se cierra el modal de grabación exitosa
$('#successModal').on('hidden.bs.modal', function () {
    $('#editModal').UifModal('hide');
    $('#successModal').UifModal('hide');
});

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

function clearFieldsDepositSlip() {
    searchBankId = -1;
    startDate = "";
    endDate = "";
    paymentTicketId = -1;
    rowSelected = null;
    $("#Bank").val("");
    $("#StartDate").val("");
    $("#EndDate").val("");
    $("#InternalBallotNumber").val("");
    $('#SearchBallotsTable').dataTable().fnClearTable();
    $("#alert").UifAlert('hide');
}

function clearEditModalFields() {
    $("#DepositSlipEditReceivingBank").val("");
    $("#DepositSlipEditBranch").val("");
    $("#DepositSlipEditReceivingAccountNumber").val("");
    $("#DepositSlipEditCurrency").val("");
    $("#DepositSlipEditCashAmount").val("");
    $("#DepositSlipEditCheckAmount").val("");
    $('#SearchedChecksToDeposit').dataTable().fnClearTable();
    $('#SelectedChecksToDeposit').dataTable().fnClearTable();
    $("#DepositSlipEditPaymentMethod").val("");
    $("#DepositSlipEditCheckNumber").val("");
    issuingBankId = "";
}

//funcion para anular la boleta
function cancelBallot() {
    $.ajax({
        url: ACC_ROOT + "DepositSlipSearch/ValidateExternalBallotDeposited",
        data: { "paymentTicketCode": rowSelected.PaymentTicketCode },
        success: function (data) {
            if (data[0].resp == true) {
                $("#alert").UifAlert('show', Resources.InternalBallotAlreadyInExternalBallot, "danger");
                $("#cancelSlip").hide();
                $("#editSlip").hide();
                $("#SearchDepositSlip").trigger('click');
            } else if (data[0].resp == false) {
                $("#alert").UifAlert('hide');
                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "DepositSlipSearch/CancelInternalBallot",
                    async: false,
                    data: { "paymetTicketCode": rowSelected.PaymentTicketCode },
                    success: function (data) {
                        $("#alert").UifAlert('show', Resources.CancelBallotSuccess, "success");
                        $('#modalDeleteBallot').appendTo("body").modal('hide');
                        $("#cancelSlip").hide();
                        $("#editSlip").hide();
                        $("#SearchDepositSlip").trigger('click');
                    }
                });
            }
        }
    });
}

//carga los datos en el modal de edición
function loadInternalBallotDeposit() {
    $.ajax({
        url: ACC_ROOT + "CheckInternalDepositSlip/GetBallotsNotDeposited",
        data: { "internalBallotNumber": rowSelected.PaymentTicketCode },
        success: function (data) {
            if (data.length > 0) { //DepositSlipEditPaymentMethod
                receivingBankId = data[0].BankCode;
                $("#DepositSlipEditReceivingBank").val(data[0].BankName);
                brachId = data[0].BranchCode;
                $("#DepositSlipEditBranch").val(data[0].BranchName);
                $("#DepositSlipEditReceivingAccountNumber").val(data[0].AccountNumber);
                currencyId = data[0].CurrencyCode;
                $("#DepositSlipEditCurrency").val(data[0].CurrencyName);
                $("#DepositSlipEditCashAmount").val(data[0].CashAmount);
                $("#DepositSlipEditCashAmount").trigger('blur');
                $("#DepositSlipEditCheckAmount").val(data[0].CheckAmount);
                $("#DepositSlipEditCheckAmount").trigger('blur');
                registerDate = data[0].RegisterDate;
                $('#SearchedChecksToDeposit').dataTable().fnClearTable();
                paymentTicketId = rowSelected.PaymentTicketCode;
                $('#DepositSlipEditPaymentMethod').val(data[0].PaymentMethodTypeCode);

                paymentStatus = "-1";
                checkDate = "-1";

                var controller = ACC_ROOT + "CheckInternalDepositSlip/GetDetailInternalBallotNotDeposited?branchCode=" + brachId +
                                            "&paymentTicketCode=" + paymentTicketId + "&paymentStatus=" + paymentStatus +
                                            "&checkDate=" + checkDate;

                $('#SelectedChecksToDeposit').UifDataTable({ source: controller });
                setTimeout(function () {
                    updateRecords();
                }, 500);
            }
            else {
                $("#DepositSlipEditReceivingBank").val("");
                $("#DepositSlipEditBranch").val("");
                $("#DepositSlipEditReceivingAccountNumber").val("");
                $("#DepositSlipEditCurrency").val("");
                $("#DepositSlipEditCashAmount").val("");
                $("#DepositSlipEditCheckAmount").val("");
                $('#SearchedChecksToDeposit').dataTable().fnClearTable();
                $('#SelectedChecksToDeposit').dataTable().fnClearTable();
            }
        }
    });
}

//Agrega cheques a depositar a cheques seleccionados
function addCheck() {
    var totalChecks = 0;
    var searched = $("#SearchedChecksToDeposit").UifDataTable('getSelected');

    if (searched != null) {
        for (var i = 0; i < searched.length; i++) {
            if (duplicateRecords(searched[i].PaymentCode) == false) {

                $('#SelectedChecksToDeposit').UifDataTable('addRow', {
                    "PaymentCode": searched[i].PaymentCode,
                    "PaymentMethodTypeCode": searched[i].PaymentMethodTypeCode,
                    "CurrencyCode": searched[i].CurrencyCode,
                    "BankName": searched[i].BankName,
                    "IssuingAccountNumber": searched[i].IssuingAccountNumber,
                    "CheckNumber": searched[i].CheckNumber,
                    "ReceiptNumber": searched[i].ReceiptNumber,
                    "CurrencyName": searched[i].CurrencyName,
                    "IncomeAmount": searched[i].IncomeAmount,
                    "CheckDate": searched[i].CheckDate,
                    "Holder": searched[i].Holder,
                    "PaymentTicketItemCode": searched[i].PaymentTicketItemId
                });
                totalChecks = parseFloat(totalChecks) + parseFloat(searched[i].IncomeAmount);
            } else {
                $("#duplicateAlert").UifAlert('show', Resources.InternalBallotAlreadyInExternalBallot, "danger");
            }
        }

        var previousTotal = parseFloat(ClearFormatCurrency($("#DepositSlipEditCheckAmount").val().replace("", ",")));
    }
}

//Validar que no se agregue duplicados en cheques seleccionados
function duplicateRecords(paymentCode) {
    var result = false;

    var selected = $("#SelectedChecksToDeposit").UifDataTable('getData');

    if (selected.length > 0) {
        for (var j = 0; j < selected.length; j++) {
            if (selected[j].PaymentCode == paymentCode) {
                result = true;
                break;
            }
        }
    }
    return result;
}

//para distinguir entre anulación de boleta o borrado de registro al presionar el botón aceptar del modal de eliminación
//1.- anulación
//2.- eliminación
function Delete(option, modal) {
    if (option == 1) {
        if (rowSelected != null)
            cancelBallot();
    }

    if (option == 2) {

        $('#SelectedChecksToDeposit').UifDataTable('deleteRow', deletePosition);

        $("#duplicateAlert").UifAlert('show', Resources.DeleteSuccessfully, "success");

        //$('#modalDeleteBallot').appendTo("body").modal('hide');
        $("#" + modal).UifModal('hide');

        deletePosition = 0;
        if (deletedRow.PaymentTicketItemCode != 0) {
            cleanObjectDepositSlipSerach();

            oPaymentTicket.PaymentTicketId = paymentTicketId;
            oPaymentTicket.PaymentTicketItemId = deletedRow.PaymentTicketItemCode;
            oPaymentTicket.PaymentId = deletedRow.PaymentCode;

            arrayTransferChecks.PaymentTicket.push(oPaymentTicket);
        }

        getChecksTotal();
    }
    option = 0;
    deletedRow = null;
}

function ValidateFieldsDepositSlip() {
    var checks = $("#SelectedChecksToDeposit").UifDataTable('getData').length;

    if ((isMoneyEmpty($("#DepositSlipEditCashAmount").val()) && checks == 0)
        || (isMoneyEmpty($("#DepositSlipEditCheckAmount").val()) && checks > 0)) {
        return false;
    }
    return true;
}

function cleanObjectDepositSlipSerach() {
    oPaymentTicket = {
        Id: null,
        Bank: null,
        BankDescription: null,
        Branch: null,
        BranchName: null,
        AccountNumber: null,
        CashAmount: null,
        Checks: null,
        DatePayment: null,
        PaymentMethodId: null,
        Currency: null,
        Amount: null,
        PaymentId: null,
        Select: null,
        PaymentTicketId: null,
        PaymentTicketItemId: null,
        DeleteRecords: null,
        LogRecords: null,
        UpdateRecords: null
    };
}

function setTicket() {
    var issuingBankCode = receivingBankId;
    var issuingBankName = $("#DepositSlipEditReceivingBank").val();
    var accountNumber = $("#DepositSlipEditReceivingAccountNumber").val();
    var cashAmount = $("#DepositSlipEditCashAmount").val();
    var paymentMethodTypeCode = $("#DepositSlipEditPaymentMethod").val();
    var branchCode = brachId;
    var branchName = $("#DepositSlipEditBranch").val();

    var checks = $("#SelectedChecksToDeposit").UifDataTable('getData');

    if (cashAmount == null || cashAmount == "")
        cashAmount = 0;
    else
        cashAmount = RemoveFormatMoney(cashAmount);

    //registro los cheques añadidos
    if (checks.length != 0) {
        for (var l = 0; l < checks.length; l++) {
            idsLog = [];
            if (checks[l].PaymentTicketItemCode == 0)
                idsLog.push(checks[l].PaymentCode);
        }
    }
    arrayTransferChecks = {
        PaymentTicket: []
    };

    if (checks.length != 0) {
        for (var k = 0; k < checks.length; k++) {
            cleanObjectDepositSlipSerach();
            oPaymentTicket.Id = checks[k].PaymentCode;//checks[k].ItemId;
            oPaymentTicket.Bank = issuingBankCode;
            oPaymentTicket.BankDescription = issuingBankName;
            oPaymentTicket.Branch = branchCode;
            oPaymentTicket.BranchName = branchName;
            oPaymentTicket.Checks = checks[k].CheckNumber;
            oPaymentTicket.DatePayment = checks[k].CheckDate;
            oPaymentTicket.PaymentMethodId = paymentMethodTypeCode;
            oPaymentTicket.Currency = checks[k].CurrencyCode;
            oPaymentTicket.Amount = RemoveFormatMoney(checks[k].IncomeAmount);
            oPaymentTicket.PaymentId = checks[k].PaymentCode;
            oPaymentTicket.AccountNumber = accountNumber;
            oPaymentTicket.CashAmount = parseFloat(cashAmount);
            oPaymentTicket.PaymentTicketId = paymentTicketId;
            oPaymentTicket.PaymentTicketItemId = checks[k].PaymentTicketItemCode;
            oPaymentTicket.PaymentMethodId = $("#DepositSlipEditPaymentMethod").val();

            arrayTransferChecks.PaymentTicket.push(oPaymentTicket);
        }
    } else {
        cleanObjectDepositSlipSerach();
        oPaymentTicket.Bank = issuingBankCode;
        oPaymentTicket.BankDescription = issuingBankName;
        oPaymentTicket.Branch = branchCode;
        oPaymentTicket.BranchName = branchName;
        oPaymentTicket.PaymentMethodId = $("#DepositSlipEditPaymentMethod").val();
        oPaymentTicket.AccountNumber = accountNumber;
        oPaymentTicket.CashAmount = parseFloat(cashAmount);
        oPaymentTicket.PaymentTicketId = paymentTicketId;

        arrayTransferChecks.PaymentTicket.push(oPaymentTicket);
    }

    return prepareRequest(arrayTransferChecks)  ;
}

function updateRecords() {

    var ids = $("#SelectedChecksToDeposit").UifDataTable('getData');
    idsUpdate = [];
    if (ids.length > 0) {
        for (var m = 0; m < ids.length; m++) {
            idsUpdate.push(ids[m].PaymentCode);
        }
    }
}

//Carga reporte
function loadReportDepositSlip(paymentTicketId, paymentMethodTypeId, branchId, branchName) {

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Report/LoadInternalBallotReport",
        data: {
            "paymentTicketCode": paymentTicketId, "paymentMethodTypeCode": paymentMethodTypeId,
            "branchId": branchId, "branchName": branchName
        },
        success: function (data) {
            showReport();
        }
    });
}

function showReport() {
    window.open(ACC_ROOT + "Report/ShowInternalBallotReport", 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function getChecksTotal() {
    var total = 0;

    var searchedTotals = $("#SelectedChecksToDeposit").UifDataTable('getData');
    if (searchedTotals.length > 0) {
        for (var i = 0; i < searchedTotals.length; i++) {
            var index = String(searchedTotals[i].IncomeAmount).indexOf("$");
            if (index == -1) {
                total = parseFloat(total) + parseFloat(searchedTotals[i].IncomeAmount);
            }
            else {
                total = parseFloat(total) + parseFloat(ClearFormatCurrency(String(searchedTotals[i].IncomeAmount)));
            }
        }
    }

    $("#DepositSlipEditCheckAmount").val(total);
    $("#DepositSlipEditCheckAmount").trigger('blur');
}

function loadInternalBallotsDetails(paymentTicketCode) {
    var controller = ACC_ROOT + "DepositSlipSearch/GetDetailChecks?paymentTicketId=" + paymentTicketCode;
    $('#CheckDetailsTable').UifDataTable({ source: controller });
}

function ticketDepositSlipSearch() {
    $.each($('#SelectedChecksToDeposit').UifDataTable("getData"), function (i, value) {
        cleanObjectCardSearch();

        oPaymentTicket.PaymentTicketId = paymentTicketId;
        oPaymentTicket.PaymentTicketItemId = 0;
        oPaymentTicket.PaymentId = value.PaymentCode;

        arrayTransferChecks.PaymentTicket.push(oPaymentTicket);
    });
    cleanObjectCardSearch();
    /*
    oPaymentTicket.PaymentTicketId = paymentTicketId;
    oPaymentTicket.PaymentTicketItemId = 0;
    oPaymentTicket.PaymentId = rowSelected.PaymentCode;
    */
    arrayTransferChecks.PaymentTicket.push(oPaymentTicket);
    return arrayTransferChecks;
};

function cleanObjectCardSearch() {
    oPaymentTicket = {
        Id: null,
        Bank: null,
        BankDescription: null,
        Branch: null,
        BranchName: null,
        AccountNumber: null,
        CashAmount: null,
        Checks: null,
        DatePayment: null,
        PaymentMethodId: null,
        Currency: null,
        Amount: null,
        PaymentId: null,
        Select: null,
        PaymentTicketId: null,
        PaymentTicketItemId: null,
        DeleteRecords: null,
        LogRecords: null,
        UpdateRecords: null
    };
}