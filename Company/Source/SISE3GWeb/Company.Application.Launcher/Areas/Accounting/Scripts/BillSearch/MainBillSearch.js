/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
var branchId = 0;
var billingConceptId = -1;
var methodTypeId = 0;
var startDate = "";
var endDate = "";
var authorizationUserId = 0;
var billControlId = 0;
var billId = -1;
var transactionNumber = -1;
var userIdBillSearch = -1;
var paymentValidator = true;
var paymentId = 0;
var accountingDate = "";

var val;
var receiptStatus = -1;
var validateDepositedSearchBillPromise;
var validatePaymentSearchBillPromise;
var validateRevertImputationPromise;
var imputationTypeId = $("#ViewBagImputationType").val();
var hasPremiumReceivableBillSearchValidationPromise;

//***********************
//REEMPLAZAN A HIDDENS
var billControlDialog = 0; //_BillControlCode
var billingConceptDialog = 0; //BillingConcept

var tempImputationId = 0;
var amount = 0;

/* Aplicar recibo */
var applyBillId;
var applyReceiptNumber;
var applyDepositer;
var applyAmount;
var applyLocalAmount;
var applyBranch;
var applyIncomeConcept;
var applyPostedValue;
var applyDescription;
var applyAccountingDate;
var applyComments;
var applyTransactionNumber;
/* Fin aplicar recibo */

var oBrokerCheckingAccountModel = {
    ImputationId: 0,
    BrokersCheckingAccountTransactionItems: []
};

var oBrokerCheckingAccountItemModel = {
    BrokerCheckingAccountItemId: 0,
    AgentTypeId: 0,
    AgentId: 0,
    AgentAgencyId: 0,
    AccountingNature: 0,
    BranchId: 0,
    SalePointId: 0,
    AccountingCompanyId: 0,
    AccountingDate: null,
    CheckingAccountConceptId: 0,
    CurrencyCode: 0,
    ExchangeRate: 0,
    IncomeAmount: 0,
    Amount: 0,
    UserId: 0,
    Description: null,
    BillId: 0
};


/*--------------------------------------------------------------------------------------------------------------------------------------------------*/
//      ACCIONES / EVENTOS
/*--------------------------------------------------------------------------------------------------------------------------------------------------*/

setTimeout(function () {
    loadReceiptFromCancel();
}, 1500);


if ($("#ViewBagBranchDisable").val() == "1") {
    setTimeout(function () {
        $("#BranchBillSearch").attr("disabled", "disabled");
    }, 300);
}
else {
    $("#BranchBillSearch").removeAttr("disabled");
}

$(document).ready(function () {
    // De entrada oculta los botones
    initialButtons();
});

// Autocomplete de usuario
$('#UserSearch').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.id > 0) {
        userIdBillSearch = selectedItem.id;
    }
    else
        userIdBillSearch = -1;
});

$('#UserSearch').on('blur', function () {
    setTimeout(function () {
        if ($('#UserSearch').val() == "") {
            userIdBillSearch = -1;
        }
    }, 1000);
});



$('#SearchBillsIncomeTable').on('rowSelected', function (event, data, position) {
    $("#alertBillSearch").UifAlert('hide');
    setButtons(data);
});

// Valida que no ingresen una fecha invalida.
$("#StartDateBillSearch").blur(function () {

    if ($("#StartDateBillSearch").val() != '') {

        if (IsDate($("#StartDateBillSearch").val()) == true) {
            if ($("#EndDateBillSearch").val() != '') {
                if (CompareDates($("#StartDateBillSearch").val(), $("#EndDateBillSearch").val())) {
                    $("#StartDateBillSearch").val(getCurrentDate);
                }
            }
        } else {
            $("#alertBillSearch").UifAlert('show', Resources.InvalidDates, "danger");
            $("#StartDateBillSearch").val("");
        }
    }
});

// Valida que no ingresen una fecha invalida.
$("#EndDateBillSearch").blur(function () {

    if ($("#EndDateBillSearch").val() != '') {

        if (IsDate($("#EndDateBillSearch").val()) == true) {
            if ($("#StartDateBillSearch").val() != '') {
                if (!CompareDates($("#EndDateBillSearch").val(), $("#StartDateBillSearch").val())) {
                    $("#EndDateBillSearch").val(getCurrentDate);
                }
            }
        } else {
            $("#alertBillSearch").UifAlert('show', Resources.InvalidDates, "danger");
            $("#EndDateBillSearch").val("");
        }
    }
});


// Controla que la fecha final sea mayor a la inicial
$('#StartDateBillSearch').on('datepicker.change', function (event, date) {

    if ($("#EndDateBillSearch").val() != "") {

        if (compare_dates($('#StartDateBillSearch').val(), $("#EndDateBillSearch").val())) {

            $("#alertBillSearch").UifAlert('show', Resources.ValidateDateTo, "warning");

            $("#StartDateBillSearch").val('');
        } else {
            $("#StartDateBillSearch").val($('#StartDateBillSearch').val());
        }
    }
});

// Controla que la fecha final sea mayor a la inicial
$('#EndDateBillSearch').on('datepicker.change', function (event, date) {

    if ($("#StartDateBillSearch").val() != "") {
        if (compare_dates($("#StartDateBillSearch").val(), $('#EndDateBillSearch').val())) {

            $("#alertBillSearch").UifAlert('show', Resources.ValidateDateFrom, "warning");

            $("#EndDateBillSearch").val('');
        } else {
            $("#EndDateBillSearch").val($('#EndDateBillSearch').val());
        }
    }
});


// levanta los detalles del recibo
$('#SearchBillsIncomeTable').on('rowEdit', function (event, data, position) {

    billId = data.BillCode;
    transactionNumber = data.TechnicalTransaction; //TransactionNumber;
    $('#DetailTransactionDialog').UifModal('showLocal', Resources.MovementDetails);

    var control = ACC_ROOT + "BillSearch/DetailValues?billId=" + data.BillCode;

    $("#DetailTransaction").UifDataTable({ source: control });

    setTimeout(function () { showDetailsBill(); }, 800);
});


$('#tblReverse').on('rowSelected', function (event, data, position) {
    if (parseInt(data.ReceiptNumber) == 0) {
        $($("#tblRejection > thead > tr")[0]).find('button').trigger('click');
        $($("#tblRejection > thead > tr")[0]).find('button').trigger('click');
    }
});


// BOTON BUSCAR
$("#BillSearch").click(function () {

    if ($("#BillSearchForm").valid()) {
        $.msg({ content: Resources.MessagePleaseWait, klass: 'white-on-black' });

        $("#alertBillSearch").UifAlert('hide');
        $("#SearchBillsIncomeTable").dataTable().fnClearTable();
        paymentValidator = true;
        accountingDate = "";

        $("#CancelBill").hide();
        $("#IncomeChangeConcept").hide();
        $("#ApplyBill").hide();
        $("#PrintBill").hide();

        branchId = $("#BranchBillSearch").val();
        if ($("#IncomeConceptBillSearch").val() != "" && $("#IncomeConceptBillSearch").val() != null) {
            billingConceptId = $("#IncomeConceptBillSearch").val();
        }
        else {
            var billingConceptId = -1;
        }

        if ($("#BillSearchNumber").val() != "") {
            billId = $("#BillSearchNumber").val();
            transactionNumber = $("#BillSearchNumber").val();
        }
        else {
            billId = -1;
            transactionNumber = -1;
        }

        if ($("#StartDateBillSearch").val() != "") {
            startDate = $("#StartDateBillSearch").val();
        }
        else {
            startDate = "";
        }

        if ($("#EndDateBillSearch").val() != "") {
            endDate = $("#EndDateBillSearch").val();
        }
        else {
            endDate = "";
        }

        methodTypeId = $("#PaymentMethodType").val();

        if ($("#ReceiptStatus").val() != "" && $("#ReceiptStatus").val() != null) {
            receiptStatus = $("#ReceiptStatus").val();
        }
        else {
            receiptStatus = -1;
        }

        if ($("#UserSearch").val() == "") {
            userIdBillSearch = -1;
        }


        var control = ACC_ROOT + "BillSearch/SearchBills?branchId=" + branchId + "&billingConceptId=" +
            billingConceptId + "&startDate=" + $("#StartDateBillSearch").val() + "&endDate=" +
            $("#EndDateBillSearch").val() + "&userId=" + userIdBillSearch + "&billId=" + transactionNumber +
            "&receiptStatus=" + receiptStatus + "&imputationTypeId=" + imputationTypeId;


        $("#SearchBillsIncomeTable").UifDataTable({ source: control });
        $("#ExportExcel").show();
    }
    else {
        return false;
    }
});

// BOTON LIMPIAR
$("#CleanSearch").click(function () {
    billSearchSetDataFieldsEmpty();
});

//********************************************************************************************************************************//
//  PROCESO DE ANULACION DE RECIBO

// BOTON ANULAR
$("#CancelBill").click(function () {
    var selectedBills = $("#SearchBillsIncomeTable").UifDataTable('getSelected');
    if (selectedBills != null) {
        $('#CancelDialog').appendTo("body").UifModal('showLocal');
    }
    else {
        $("#alertBillSearch").UifAlert('show', Resources.SelectOneItem, "warning");
    }
});


//********************************************************************************************************************************//
//  PROCESO DE CAMBIO CONCEPTO DE INGRESO

// BOTON CAMBIO CONCEPTO DE INGRESO
$("#IncomeChangeConcept").click(function () {

    var rowSelected = $("#SearchBillsIncomeTable").UifDataTable("getSelected");

    if (rowSelected != null) {
        billId = rowSelected[0].BillCode;
        transactionNumber = rowSelected[0].TechnicalTransaction; //TransactionNumber;
        var titleDialog = Resources.IncomeChangeConceptDialogTitle + ": " + billId;
        $('#IncomeTitleDialog').text(titleDialog);

        $("#IncomeConceptDialog").find("#ReceiptNumberDialog").val(billId);

        loadIncomeConcept();

        if ($("#IncomeConceptDialog").find("#BillingConceptDialog").val() == "") {
            $("#alertBillSearch").UifAlert('show', Resources.IncomeChangeConceptMessageNull, "warning");
        } else {
            // Se valida el mes fecha contable recibo sea igual a mes fecha contable modulo
            if (ValidateAccountingDate(billId)) {
                $('#IncomeConceptDialog').UifModal('showLocal', titleDialog);
            } else {
                $("#alertBillSearch").UifAlert('show', Resources.IncomeChangeConceptMessageAccountingDate, "warning");
                setDataFieldsIncomeEmpty();
            }
        }
    }
    else {
        $("#alertBillSearch").UifAlert('show', Resources.SelectOneItem, "warning");
    }
});


// BOTON CANCELAR DEL DIALOGO
$("#CancelConceptDialogCancel").click(function () {
    setDataFieldsIncomeEmpty();
    $("#IncomeConceptDialog").modal('hide');
});

// ejecuta la reversiòn
$("#PremiumReceivableListSelectionDialog").find("#ConfirmReverseApplication").click(function () {

    billSearchReverseApplication();
});


//********************************************************************************************************************************//
//  PROCESO DE APLICACION

// Llama a la aplicación
$("#ApplyBill").click(function () {
    var rowSelected = $("#SearchBillsIncomeTable").UifDataTable("getSelected");

    if (rowSelected != null && rowSelected != undefined
        && Array.isArray(rowSelected) && rowSelected.length > 0) {
        var incomeConcept = '';
        if ($('#IncomeConceptBillSearch').UifSelect('getSelected') != "") {
            incomeConcept = $('#IncomeConceptBillSearch').UifSelect('getSelectedText');
        }
        //applyComments
        loadReceiptApplication(rowSelected[0]);

        window.location.href = $("#ViewBagLoadApplicationReceiptLink").val()
            + "?incomeConcept=" + incomeConcept + "&technicalTransaction=" + rowSelected[0].TechnicalTransaction
            + "&pagetoredirect=" + Resources.ToRedirectMainbillSearch;//Búsqueda
    }
    else {
        $("#alertBillSearch").UifAlert('show', Resources.SelectOneItem, "warning");
    }
});

//********************************************************************************************************************************//
//  PROCESO DE IMPRESION

$("#PrintBill").click(function () {
    var rowSelected = $("#SearchBillsIncomeTable").UifDataTable("getSelected");
    if (rowSelected != null) {
        SearchLoadBillingReport(rowSelected[0].BranchCode, rowSelected[0].BillCode, 1);
    }
    else {
        $("#alertBillSearch").UifAlert('show', Resources.SelectOneItem, "warning");
    }
});

//********************************************************************************************************************************//
//  PROCESO DE REVERSIÓN

// levanta el diálogo de confirmación
$("#ReverseApplication").click(function () {

    $("#alertBillSearch").UifAlert('hide');
    var rowSelected = $("#SearchBillsIncomeTable").UifDataTable("getSelected");
    if (rowSelected != null) {
        billSearchValidateImputationRevert(billId, parseInt(imputationTypeId));
        validateRevertImputationPromise.then(function (revertData) {

            if ((revertData)) {

                validateSearchBillDeposited();
                validateDepositedSearchBillPromise.then(function (depositedData) {

                    if (!depositedData) { //pago en boleta de depósito       
                        //valida si tiene primas por cobrar.
                        //billSearchHasPremiumReceivable(billId, parseInt(imputationTypeId));
                        //hasPremiumReceivableBillSearchValidationPromise.then(function (validateData) {
                        //    if (validateData) {
                                billSearchValidatePayment(billId, parseInt(imputationTypeId));
                                validatePaymentSearchBillPromise.then(function (paymentData) {
                                    if (paymentData)
                                        billSearchShowPremiumReceivableListDialog();
                                    else
                                        $("#alertBillSearch").UifAlert('show', Resources.PaymentRevertValidation, "warning");
                         //       });
                         //   } else {
                         //       $("#alertBillSearch").UifAlert('show', Resources.CannotReverseAppliedBillWarningMessage, "warning");
                         //   }
                        });
                    }
                    else
                        $("#alertBillSearch").UifAlert('show', Resources.BillValuesAlreadyDeposited, "warning");
                });
            }
            else {
                console.log('Revert->' + revertData);

                $("#alertBillSearch").UifAlert('show', Resources.ReceiptIsAlreadyReversed, "warning");
            }
        });
    }
    else {
        $("#alertBillSearch").UifAlert('show', Resources.SelectedRecordBeforeContinuing, "warning");
    }
});


////////////////////////////////////
/// Botón Exportar a Excel       ///
////////////////////////////////////
$('#ExportExcel').click(function () {
    $("#BillSearchForm").validate();

    if ($("#BillSearchForm").valid()) {
        branchId = $("#BranchBillSearch").val();
        if ($("#IncomeConceptBillSearch").val() != "") {
            billingConceptId = $("#IncomeConceptBillSearch").val();
        } else {
            var billingConceptId = -1;
        }

        if ($("#BillSearchNumber").val() != "") {
            billId = $("#BillSearchNumber").val();
        } else {
            billId = -1;
        }

        if ($("#StartDateBillSearch").val() != "") {
            startDate = $("#StartDateBillSearch").val();
        } else {
            startDate = "";
        }

        if ($("#EndDateBillSearch").val() != "") {
            endDate = $("#EndDateBillSearch").val();
        } else {
            endDate = "";
        }

        methodTypeId = $("#PaymentMethodType").val();

        if ($("#ReceiptStatus").val() != "") {
            receiptStatus = $("#ReceiptStatus").val();
        } else {
            receiptStatus = -1;
        }

        var url = ACC_ROOT + "BillSearch/GenerateReceiptsToExcel?branchId=" + branchId +
            "&billingConceptId=" + billingConceptId + "&startDate=" + $("#StartDateBillSearch").val() +
            "&endDate=" + $("#EndDateBillSearch").val() + "&userId=" + userIdBillSearch + "&billId=" + billId +
            "&receiptStatus=" + receiptStatus + "&imputationTypeId=" + imputationTypeId;

        var newPage = window.open(url, '_self', 'width=5, height=5, scrollbars=no');
        setTimeout(function () {
            newPage.open('', '_self', '');
        }, 100);
    }
});

/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
function loadReceiptFromCancel() {

    if ($("#ViewBagReceiptNumber").val() > 0) {

        $("#BranchBillSearch").val($("#ViewBagBranchId").val());
        $("#BillSearchNumber").val($("#ViewBagReceiptNumber").val());
        $("#BillSearch").trigger('click');
    }
}

function loadReceiptApplication(rowSelected) {
    // conversión si un número es negativo
    var delimiters = /\(([^)]+)\)/; //el valor negativo se envía entre paréntesis
    var value = delimiters.exec(rowSelected.PaymentsTotal);
    var amount = 0;
    if (value == null) {
        amount = ClearFormatCurrency(rowSelected.PaymentsTotal);
    }
    else {
        amount = ClearFormatCurrency(value[1]) * -1;
    }

    applyBillId = rowSelected.BillCode;
    applyAccountingDate = rowSelected.AccountingDate;
    applyReceiptNumber = rowSelected.BillCode;
    applyTransactionNumber = rowSelected.TechnicalTransaction;
    applyDepositer = rowSelected.DocumentNumber + " - " + rowSelected.Payer;
    applyAmount = amount;
    applyBranch = rowSelected.BranchDescription;

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "ReceiptApplication/GetReceiptApplicationInformationByBillId",
        data: { "billId": applyBillId },
        success: function (data) {

            if (data.length > 0) {
                applyPostedValue = data[0].PostdatedValue;
                applyIncomeConcept = data[0].CollectConceptDescription;
                applyPostedValue = FormatCurrency(FormatDecimal(data[0].PostdatedValue));
                applyDescription = data[0].CollectDescription;
                applyComments = data[0].Comments;
            }
            else {
                applyIncomeConcept = "";
                applyPostedValue = "";
                applyDescription = "";
                applyComments = "";
            }
        }
    });
}

function showDetailsBill() {

    var title = Resources.SearchBillsValuesEnteredBillNumber + ": " + billId;
    $("#DetailTitleDialog").text(title);

    var idsTransactions = $("#DetailTransaction").UifDataTable("getData");

    var validator = false;
    for (var x in idsTransactions) {
        // Dependiendo de los pagos del Recibo muestra los campos

        var rowData = idsTransactions[x];

        if (rowData.PaymentTypeId == $("#ViewBagParamPaymentMethodCreditableCreditCard").val() ||
            rowData.PaymentTypeId == $("#ViewBagParamPaymentMethodUncreditableCreditCard").val()) {

            rowData.DocumentNumber = replaceWithAsterisks(rowData.DocumentNumber);

            $('#DetailTransaction').UifDataTable('editRow', rowData, x);
        }
    }

    // Establece la descripción del status de ""CANJEADO" a --> "ANULADO"  si esque tiene status 0 en la tabla payment
    for (var j in idsTransactions) {
        var rowData = idsTransactions[j];
        if (rowData.PaymentStatus == 0) {
            rowData.StatusDescription = Resources.Annulled;

            $('#DetailTransaction').UifDataTable('editRow', rowData, j);
        }
    }
}

function replaceWithAsterisks(cellValue) {
    var row = 0;
    var valueWithAsterisk = "";
    var firstNumber = cellValue.substring(0, 1);
    var lastNumber = cellValue.substring(cellValue.length - 1);

    for (var row = 0; row < cellValue.length - 2; row++) {
        valueWithAsterisk += "*";
    }

    valueWithAsterisk = firstNumber + valueWithAsterisk + lastNumber;
    cellValue = valueWithAsterisk;
    return cellValue;
}

function billSearchReverseApplication() {

    lockScreen();

    $.ajax({
        type: "POST",
        url: ACC_ROOT + "ReceiptApplication/ReverseImputationRequest",
        data: { "sourceId": billId, "imputationTypeId": imputationTypeId, "accountingDate": accountingDate }
    }).done(function (data) {
        if (data.success == false) {
            //TRANSAC ACC              
            $("#alertBillSearch").UifAlert('show', data.message, "danger");
        } else {
            /*var description = "";

            if (data.result.length > 0) {
                for (var i = 0; i < data.result.length; i++) {
                    description = description + data.result[i] + ". ";
                }
            }*/
            $("#ReverseApplication").hide();
            $("#alertBillSearch").UifAlert('show', Resources.ApplicationReverseSuccessLabel + '. ' + Resources.AccountingTransactionNumberGenerated + " " + data.technicalTransaction, "success");

            if ($("#ReceiptStatus").val() != "") {
                receiptStatus = $("#ReceiptStatus").val();
            } else {
                receiptStatus = -1;
            }

            if ($("#BillSearchNumber").val() != "") {
                transactionNumber = $("#BillSearchNumber").val();
            }
            else {
                transactionNumber = -1;
            }

            // refresca la grilla
            var control = ACC_ROOT + "BillSearch/SearchBills?branchId=" + branchId + "&billingConceptId=" +
                billingConceptId + "&startDate=" + $("#StartDateBillSearch").val() + "&endDate=" + $("#EndDateBillSearch").val() +
                "&userId=" + userIdBillSearch + "&billId=" + transactionNumber + "&receiptStatus=" + receiptStatus + "&imputationTypeId=" + imputationTypeId;
            $("#SearchBillsIncomeTable").UifDataTable({
                source: control
            });
        }
    }).fail(function () {
        $("#alertBillSearch").UifAlert('show', Resources.ApplicationReverseFailureLabel, "danger");
    }).always(function () {
        unlockScreen();
        $("#PremiumReceivableListSelectionDialog").modal('hide');
        $("#CancelBill").hide();
    });
}

function billSearchShowPremiumReceivableListDialog() {

    var control = ACC_ROOT + "ReceiptApplication/GetPremiumRecievableAppliedByBillIdByImputationTypeId?billId=" + billId + "&imputationTypeId=" + imputationTypeId;
    $("#PremiumReceivableListSelectionTable").UifDataTable({ source: control });

    var title = Resources.ReverseApplicationTitleDialog + " " + transactionNumber;

    $("#PremiumReceivableTitleDialog").text(title);
    $('#PremiumReceivableListSelectionDialog').appendTo("body").UifModal('showLocal');
}

// Carga y despliega reporte
function SearchLoadBillingReport(branchId, billCode, waterMark) {

    var controller = ACC_ROOT + "Report/LoadBillingReportCopy?branchId=" + branchId + "&billCode=" +
        billCode + "&reportId=3" + "&waterMark=" + waterMark + "&userId=" + userIdBillSearch;
    window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function ValidateAccountingDate(billCode) {

    var result = false;

    if (billControlDialog == -1) {
        result = false;
    }
    else {
        result = true;
    }
    return result;
}

// Anula el recibo
function cancelBill() {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "BillSearch/GetPaymentTicketItemsByBillId",
        async: false,
        data: { "billId": billId },
        success: function (data) {
            if (data[0].Id < 0) {
                $("#alertBillSearch").UifAlert('show', Resources.BillingMessageWarningInternalBallot, "warning");
            }
            else {
                $.ajax({
                    type: "POST",
                    url: ACC_ROOT + "BillSearch/ConfirmModuleDate",
                    async: false,
                    data: { "date": accountingDate },
                    success: function (dataConfirm) {
                        if (dataConfirm[0].Id < 0) {
                            $("#alertBillSearch").UifAlert('show', Resources.BillingMessageWarningModuleDate, "danger");
                        }
                        else {
                            lockScreen();
                            $.ajax({
                                type: "POST",
                                //url: ACC_ROOT + "Billing/CancelBill",
                                url: ACC_ROOT + "ReceiptApplication/ReverseImputationRequest",
                                data: { "sourceId": billId, "imputationTypeId": imputationTypeId, "accountingDate": accountingDate }
                                //data: { "billId": billId, "billControlId": billControlId, "authorizationUserId": authorizationUserId, "accountingDate": accountingDate },
                            }).done(function (data) {

                                if (data.success == false) {
                                    //TRANSAC ACC              
                                    $("#alertBillSearch").UifAlert('show', data.message, "danger");
                                } else {

                                    $("#alertBillSearch").UifAlert('hide');

                                    if ($("#ReceiptStatus").val() != "") {
                                        receiptStatus = $("#ReceiptStatus").val();
                                    }
                                    else {
                                        receiptStatus = -1;
                                    }
                                    $("#alertBillSearch").UifAlert('show', data.message, "success");
                                    
                                    // RECARGA DE LA GRILLA
                                    var control = ACC_ROOT + "BillSearch/SearchBills?branchId=" + branchId + "&billingConceptId=" +
                                        billingConceptId + "&startDate=" + $("#StartDateBillSearch").val() +
                                        "&endDate=" + $("#EndDateBillSearch").val() + "&userId=" + userIdBillSearch + "&billId=" +
                                        transactionNumber + "&receiptStatus=" + receiptStatus + "&imputationTypeId=" + imputationTypeId;

                                    $("#SearchBillsIncomeTable").UifDataTable({ source: control });

                                    $("#CancelBill").hide();
                                    $("#ExportExcel").hide();
                                    $("#IncomeChangeConcept").hide();
                                    $("#ApplyBill").hide();
                                    $("#PrintBill").hide();
                                }
                            }).fail(function () {
                                $("#alertBillSearch").UifAlert('show', Resources.CollectCancelFailureLabel, "danger");
                            }).always(function () {
                                unlockScreen();
                            });
                        }
                    }
                });
            }
        }
    });
}

function getUserId() {
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "Common/GetUserAuthenticated",
        async: false,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            authorizationUserId = data.UserId;
        }
    });
}

function setButtons(rowSelected) {

    if (rowSelected != null) {
        billId = rowSelected.BillCode;
        transactionNumber = rowSelected.TechnicalTransaction;
        branchId = rowSelected.BranchCode;
        accountingDate = rowSelected.AccountingDate;

        var status = rowSelected.StatusDescription;

        if (status == Resources.Global.Active) {
            $("#CancelBill").show();
            $("#IncomeChangeConcept").show();
            $("#ApplyBill").show();
            $("#ReverseApplication").hide();

            ItemsBill(billId).done(function (data) {
                if (data[0].Id > 0) {
                    $("#PrintBill").show();
                } else {
                    $("#PrintBill").hide();
                }
            }).fail(function () {
                $("#PrintBill").hide();
            });
        }
        if (status == Resources.Global.Cancelled) {
            $("#CancelBill").hide();
            $("#IncomeChangeConcept").hide();
            $("#ApplyBill").hide();
            $("#PrintBill").hide();
            $("#ReverseApplication").hide();
        }
        if (status == Resources.Global.PartiallyApplied) {
            $("#ApplyBill").show();
            $("#CancelBill").hide();
            $("#IncomeChangeConcept").hide();
            $("#PrintBill").show();
            $("#ReverseApplication").hide();
        }
        if (status == Resources.Global.Applied) {
            $("#ApplyBill").hide();
            $("#CancelBill").hide();
            $("#IncomeChangeConcept").hide();
            $("#PrintBill").show();
            if ($("#ViewBagHasRevertImputation").val() == 'True') {
                $("#ReverseApplication").show();
            } else {
                $("#ReverseApplication").hide();
            }
        }
        paymentId = rowSelected.PaymentCode;
    }
}

function ItemsBill(billId) {
    return $.ajax({
        type: "POST",
        url: ACC_ROOT + "BillSearch/ItemsBill",
        data: { "billId": billId },
        async: false });
}

function billSearchSetDataFieldsEmpty() {
    $("#BranchBillSearch").val(""); //cmb sucursal
    $("#UserSearch").val(""); //autocomplete usuario
    $("#IncomeConceptBillSearch").val(""); //cmb conceptos ingreso
    $("#StartDateBillSearch").val("");
    $("#EndDateBillSearch").val("");
    $("#BillSearchNumber").val("");

    $("#alertBillSearch").UifAlert('hide');

    $("#CancelBill").hide();
    $("#IncomeChangeConcept").hide();

    $("#SearchBillsIncomeTable").dataTable().fnClearTable();
    $('#ExportExcel').hide();
    $('#ReverseApplication').hide();
    $('#ApplyBill').hide();
    $('#PrintBill').hide();


    branchId = 0;
    billingConceptId = 0;
    methodTypeId = 0;
    startDate = "";
    endDate = "";
    paymentValidator = true;
    userIdBillSearch = -1;
    accountingDate = "";
    billId = -1;
}

function loadIncomeConcept() {
    if ($("#IncomeConceptDialog").find("#ReceiptNumberDialog").val() != "") {
        $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Billing/GetReceiptForExchangeConcept",
            data: { "billId": billId },
            success: function (data) {
                if (data.length > 0) {
                    $("#IncomeConceptDialog").find("#ReceiptDateDialog").val(data[0].RegisterDate);
                    $("#IncomeConceptDialog").find("#BillingConceptDialog").val(data[0].BillingConceptName);
                    billingConceptDialog = data[0].BillingConceptCode;
                    billControlDialog = data[0].BillControlCode;
                }
                else {
                    $("#IncomeConceptDialog").find("#ReceiptDateDialog").val('');
                    $("#IncomeConceptDialog").find("#BillingConceptDialog").val('');
                    billingConceptDialog = 0;
                    billControlDialog = 0;
                }
            }
        });
    }
    else {
        $("#IncomeConceptDialog").find("#ReceiptDateDialog").val('');
        $("#IncomeConceptDialog").find("#BillingConceptDialog").val('');
        billingConceptDialog = 0;
        billControlDialog = 0;
    }
}

function setDataFieldsIncomeEmpty() {
    $("#IncomeConceptComboDialog").val("");
    $("#ReceiptNumberDialog").val("");
    $("#ReceiptDateDialog").val("");
    billControlDialog = 0;
    billControlDialog = 0;
}

function validateSearchBillDeposited() {
    return validateDepositedSearchBillPromise = new Promise(function (resolve, reject) {

        lockScreen();
        setTimeout(function () {
            $.ajax({
                type: "POST",
                url: ACC_ROOT + "BillSearch/ValidateDeposited",
                data: { "billId": billId }
            }).done(function (depositedData) {
                unlockScreen();
                resolve(depositedData);
            });
        }, 500);
    });
}

function billSearchValidatePayment(billId, imputationTypeId) {
    return validatePaymentSearchBillPromise = new Promise(function (resolve, reject) {
        lockScreen();
        setTimeout(function () {
            $.ajax({
                type: "POST",
                url: ACC_ROOT + "BillSearch/ValidatePaymentRevertion",
                data: { "sourceId": billId, "imputationTypeId": imputationTypeId }
            }).done(function (paymentData) {
                unlockScreen();
                resolve(paymentData);
            });
        }, 500);
    });
}

function billSearchValidateImputationRevert(billId, imputationTypeId) {
    return validateRevertImputationPromise = new Promise(function (resolve, reject) {

        lockScreen();
        setTimeout(function () {
            $.ajax({
                type: "POST",
                url: ACC_ROOT + "BillSearch/GetImputationBySourceCodeImputationTypeId",
                data: { "sourceId": billId, "imputationTypeId": imputationTypeId }
            }).done(function (revertData) {
                unlockScreen();
                resolve(revertData);
            });
        }, 500);
    });
}

function billSearchHasPremiumReceivable(billId, imputationTypeId) {

    return hasPremiumReceivableBillSearchValidationPromise = new Promise(function (resolve, reject) {

        lockScreen();
        setTimeout(function () {

            $.ajax({
                type: "POST",
                url: ACC_ROOT + "ReceiptApplication/HasPremiumReceivable",
                data: { "billId": billId, "imputationTypeId": imputationTypeId }
            }).done(function (validateData) {
                unlockScreen();
                resolve(validateData);
            });
        }, 500);
    });
}

//************************************************************************************************************************
// P R O G R A M A C I Ó N    D E    B O T O N E S    D E     M O D A L E S

//BOTON ANULAR DE MODAL ANULAR RECIBO
function CancelBillDialog(event, modal) {

    //$("#CancelDialog").modal('hide');
    $("#" + modal).UifModal('hide');
    var rowSearch = $("#SearchBillsIncomeTable").UifDataTable("getSelected");

    billId = rowSearch[0].BillCode;
    billControlId = rowSearch[0].BillControlCode;
    getUserId();
    cancelBill();
}

//BOTON ACEPTAR DEL DIALOGO
function SaveConceptDialogAccept(event, modal) {
    $("#IncomeConceptDialog").find("#IncomeChangeForm").validate();

    if ($("#IncomeConceptDialog").find("#IncomeChangeForm").valid()) {
        $.ajax({
            url: ACC_ROOT + "Billing/UpdateIncomeChangeConcept",
            data: { "billId": $("#ReceiptNumberDialog").val(), "billControlId": billControlDialog, "billingConceptId": $("#IncomeConceptComboDialog").val() },
            success: function (data) {
                if (data.success == false) {
                    $("#" + modal).UifModal('hide');
                    $("#alertBillSearch").UifAlert('show', data.result, "danger");
                } else {
                    $("#alertBillSearch").UifAlert('show', Resources.IncomeChangeConceptSaveSuccessfully, "success"); setDataFieldsIncomeEmpty(); $("#" + modal).UifModal('hide');
                }
            }
        });
    }
}

//Inicializa Botones
function initialButtons() {
    $("#CancelBill").hide();
    $("#IncomeChangeConcept").hide();
    $("#ApplyBill").hide();
    $("#PrintBill").hide();
    $("#ReverseApplication").hide();
    $("#ExportExcel").hide();
}
