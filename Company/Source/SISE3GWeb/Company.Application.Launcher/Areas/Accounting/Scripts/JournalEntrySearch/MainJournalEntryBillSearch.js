
/*-----------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS
/*-----------------------------------------------------------------------------------------------------------------------------------------*/

var branchId = 0;
var billingConceptId = -1;
var methodTypeId = 0;
var startDate = "";
var endDate = "";
var authorizationUserId = 0;
var technicalTransaction = -1;
var journalEntryBillSearchUserId = -1;
var paymentValidator = true;
var paymentId = 0;
var accountingDate = "";
var val;
var receiptStatus = -1;
var validateDepositedJournalPromise;
var validatePaymentJournalPromise;
var imputationTypeId = $("#ViewBagImputationType").val();
var journalEntryId = 0;
var accountingTransaction = 0;
//***********************
var tempImputationId = 0;
var amount = 0;

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
//BOTONES DE MODALES

$(document).ready(function () {
    // Ejecuta la reversiòn
    $("#PremiumReceivableListSelectionDialogJournal").find("#ConfirmReverseApplicationJournal").click(function () {
        journalEntrySearchReverseApplication();
    });
    // De entrada oculta los botones
    $("#ReverseApplicationJournal").hide();
    $("#ExportExcelJournal").hide();
    $("#PrintJournalEntry").hide();
});

// Autocomplete de usuario
$('#UserSearch').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.id > 0) {
        journalEntryBillSearchUserId = selectedItem.id;
    }
    else {
        journalEntryBillSearchUserId = -1;
    }
});

$('#UserSearch').on('blur', function (event) {
    if ($("#UserSearch").val() == "") {
        journalEntryBillSearchUserId = -1;
    }
});



$('#SearchJournalEntryTable').on('rowSelected', function (event, data, position) {
    $("#alertJurnalEntry").UifAlert('hide');
    setButtonsSearchJournal(data);
});

/*Control Fecha Desde*/
$("#DateFromJournalEntry").on("datepicker.change", function (event, date) {
    $("#alertJurnalEntry").UifAlert('hide');;
    if ($("#DateFromJournalEntry").val() != '') {
        if (IsDate($("#DateFromJournalEntry").val()) == true) {
            if ($("#DateToJournalEntry").val() != '') {
                if (CompareDates($("#DateFromJournalEntry").val(), $("#DateToJournalEntry").val())) {
                    $("#alertJurnalEntry").UifAlert('show', Resources.ValidateDateTo, 'warning');
                    $("#DateFromJournalEntry").val("");
                }
            }
        } else {
            $("#alertJurnalEntry").UifAlert('show', Resources.InvalidDates, "warning");
            $("#DateFromJournalEntry").val("");
        }
    }
});

/*Control Fecha Hasta*/
$("#DateToJournalEntry").on("datepicker.change", function (event, date) {
    $("#alertJurnalEntry").UifAlert('hide');;
    if ($("#DateToJournalEntry").val() != '') {
        if (IsDate($("#DateToJournalEntry").val()) == true) {
            if ($("#PayExpDateQuotaFrom").val() != '') {
                if (CompareDates($("#DateFromJournalEntry").val(), $("#DateToJournalEntry").val())) {
                    $("#alertJurnalEntry").UifAlert('show', Resources.ValidateDateFrom, 'warning');
                    $("#DateToJournalEntry").val("");
                }
            }
        } else {
            $("#alertJurnalEntry").UifAlert('show', Resources.InvalidDates, "warning");
            $("#DateToJournalEntry").val("");
        }
    }
});


// BOTON BUSCAR
$("#SearchJournal").click(function () {
    $("#ExportExcelJournal").hide();
    $("#PrintJournalEntry").hide();
    $("#ReverseApplicationJournal").hide();
    $("#alertJurnalEntry").UifAlert('hide');

    if ($("#JurnalEntrySearchForm").valid()) {
  
        $("#SearchJournalEntryTable").dataTable().fnClearTable();
        paymentValidator = true;
        accountingDate = "";

        $("#ReverseApplicationJournal").hide();
        branchId = $("#BranchJurnalEntrySearch").val();

        if ($("#technicalTransactionJE").val() != "") {
            technicalTransaction = $("#technicalTransactionJE").val();
        } else {
            technicalTransaction = -1;
        }

        if ($("#DateFromJournalEntry").val() != "") {
            startDate = $("#DateFromJournalEntry").val();
        } else {
            startDate = "";
        }

        if ($("#DateToJournalEntry").val() != "") {
            endDate = $("#DateToJournalEntry").val();
        } else {
            endDate = "";
        }

        methodTypeId = $("#PaymentMethodType").val();

        if ($("#ReceiptStatus").val() != "") {
            receiptStatus = $("#ReceiptStatus").val();
        } else {
            receiptStatus = -1;
        }

        if (journalEntryBillSearchUserId == undefined) {
            journalEntryBillSearchUserId = -1;
        }

        lockScreen();

        setTimeout(function () {
            $.ajax({
                async: false,
                type: "POST",
                url: ACC_ROOT + "BillSearch/SearchJournalEntry",
                data: JSON.stringify(
                    {
                        "branchId": branchId,
                        "journalEntryStatusId": ($("#ReceiptStatus").val() == "" ? -1 : $("#ReceiptStatus").val()),
                        "startDate": $("#DateFromJournalEntry").val(),
                        "endDate": $("#DateToJournalEntry").val(),
                        "userId": journalEntryBillSearchUserId,
                        "billId": technicalTransaction,
                        "receiptStatus": receiptStatus,
                        "imputationTypeId": imputationTypeId
                    }
                ),
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.success == true) {

                        if (data.aaData.length > 0) {
                            $("#SearchJournalEntryTable").UifDataTable({ sourceData: data.aaData })
                            $("#ExportExcelJournal").show();
                        }
                    }
                    else {
                        $("#alertJurnalEntry").UifAlert('show', data.result, "danger");
                    }
                    unlockScreen();
                }
            });
        }, 1000);       

    } else {
        return false;
    }
});

// BOTON LIMPIAR
$("#CleanSearchJournal").click(function () {
    CleanSearchJournalControl();
});

//BOTONES MENU INFERIOR
////////////////////////////////////
/// Botón Exportar a Excel       ///
////////////////////////////////////
$('#ExportExcelJournal').click(function () {
    $("#alertJurnalEntry").UifAlert('hide');
    $("#JurnalEntrySearchForm").validate();

    if ($("#JurnalEntrySearchForm").valid()) {
        branchId = $("#BranchJurnalEntrySearch").val();

        if ($("#technicalTransactionJE").val() != "") {
            technicalTransaction = $("#technicalTransactionJE").val();
        } else {
            technicalTransaction = -1;
        }

        if ($("#DateFromJournalEntry").val() != "") {
            startDate = $("#DateFromJournalEntry").val();
        } else {
            startDate = "";
        }

        if ($("#DateToJournalEntry").val() != "") {
            endDate = $("#DateToJournalEntry").val();
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
            "&billingConceptId=" + billingConceptId + "&startDate=" + $("#DateFromJournalEntry").val() +
            "&endDate=" + $("#DateToJournalEntry").val() + "&userId=" + journalEntryBillSearchUserId + "&billId=" + technicalTransaction +
            "&receiptStatus=" + receiptStatus + "&imputationTypeId=" + imputationTypeId;

        var newPage = window.open(url, '_self', 'width=5, height=5, scrollbars=no');
        setTimeout(function () {
            newPage.open('', '_self', '');
        }, 100);
    }
});

//********************************************************************************************************************************//
//  PROCESO DE REVERSIÒN
// levanta el diálogo de confirmación
$("#ReverseApplicationJournal").click(function () {
    $("#alertJurnalEntry").UifAlert('hide');
    validatePaymentSearchJournal(journalEntryId, parseInt(imputationTypeId));
    validatePaymentJournalPromise.then(function (paymentData) {        
        if (paymentData) {
            showPremiumReceivableListDialog();
        } else {
            $("#alertJurnalEntry").UifAlert('show', Resources.PaymentRevertValidation, "warning");
        }
    });
});

$("#PrintJournalEntry").click(function () {
    $("#alertJurnalEntry").hide();
    showJournaEntryReport(accountingDate, technicalTransaction, journalEntryId)
});


/*---------------------------------------------------------------------------------------------------------------------------------------------------*/
//DEFINICION DE FUNCIONES
/*---------------------------------------------------------------------------------------------------------------------------------------------------*/

function journalEntrySearchReverseApplication() {

    lockScreen();
    $.ajax({
        type: "POST",
        url: ACC_ROOT + "ReceiptApplication/ReverseImputationRequest",
        data: { "sourceId": journalEntryId, "imputationTypeId": imputationTypeId, "accountingDate": accountingDate }
    }).done(function (data) {
        $("#PremiumReceivableListSelectionDialogJournal").modal('hide');
        if (data.success) {
            var message = Resources.ApplicationReverseSuccessLabel + '. ' + Resources.AccountingTransactionNumberGenerated + " " + data.technicalTransaction;
            $("#ReverseApplicationJournal").hide();
            $("#alertJurnalEntry").UifAlert('show', message, "success");

            if ($("#ReceiptStatus").val() != "") {
                receiptStatus = $("#ReceiptStatus").val();
            } else {
                receiptStatus = -1;
            }

            // refresca la grilla
            var control = ACC_ROOT + "BillSearch/SearchJournalEntry?branchId=" + branchId + "&journalEntryStatusId=" +
                ($("#ReceiptStatus").val() == "" ? -1 : $("#ReceiptStatus").val()) + "&startDate=" + $("#DateFromJournalEntry").val() + "&endDate=" + $("#DateToJournalEntry").val() +
                "&userId=" + journalEntryBillSearchUserId + "&billId=" + technicalTransaction + "&receiptStatus=" + receiptStatus + "&imputationTypeId=" + imputationTypeId;
            $("#SearchJournalEntryTable").UifDataTable({ source: control });
        } else {
            if (!isNull(data.message)) {
                $("#alertJurnalEntry").UifAlert('show', data.message, "danger");
            } else {
                $("#alertJurnalEntry").UifAlert('show', Resources.ApplicationReverseFailureLabel, "danger");
            }
        }
    }).fail(function () {
        $("#alertJurnalEntry").UifAlert('show', Resources.ApplicationReverseFailureLabel, "danger");
    }).always(function () {
        $("#ReverseApplicationJournal").hide();
        unlockScreen();
    });
}

function showPremiumReceivableListDialog() {
    var control = ACC_ROOT + "ReceiptApplication/GetPremiumRecievableAppliedByBillIdByImputationTypeId?billId=" + journalEntryId + "&imputationTypeId=" + imputationTypeId;
    $("#PremiumReceivableListSelectionTable").UifDataTable({ source: control });

    var title = Resources.ReverseApplicationJournalEntryTitleDialog + " " + technicalTransaction;

    $("#PremiumReceivableTitleDialog").text(title);
    $('#PremiumReceivableListSelectionDialogJournal').appendTo("body").UifModal('showLocal');
}

function setButtonsSearchJournal(rowSelected) {

    paymentId = rowSelected.PaymentCode;
    branchId = rowSelected.BranchCode;
    accountingDate = rowSelected.AccountingDate;
    journalEntryId = rowSelected.JournalEntryId;
    technicalTransaction = rowSelected.TechnicalTransaction;
    accountingTransaction = rowSelected.AccountingTransaction;

    var status = rowSelected.StatusDescription;

    if (status == Resources.Applied) {
        if ($("#ViewBagHasRevertImputation").val() == 'True') {
            $("#ReverseApplicationJournal").show();
        } else {
            $("#ReverseApplicationJournal").hide();
        }
    }
    $("#PrintJournalEntry").show();
}

/*Vista en PDF*/
function showJournaEntryReport(accountingDate, technicalTransaction, journalEntryId) {

    var controller = ACC_ROOT + "Report/LoadRePrintJournalEntryBillReport?accountingDate=" + accountingDate +
        '&journalEntryId=' + accountingTransaction + '&technicalTransaction=' + technicalTransaction;   

    window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

function CleanSearchJournalControl() {
    $("#BranchJurnalEntrySearch").val("");
    $("#UserSearch").val("");
    $("#DateFromJournalEntry").val("");
    $("#DateToJournalEntry").val("");
    $("#technicalTransactionJE").val("");
    $("#alertJurnalEntry").UifAlert('hide');;
    $("#ReverseApplicationJournal").hide();
    $("#SearchJournalEntryTable").dataTable().fnClearTable();

    branchId = 0;
    billingConceptId = 0;
    methodTypeId = 0;
    startDate = "";
    endDate = "";
    paymentValidator = true;
    journalEntryBillSearchUserId = -1;
    accountingDate = "";
    technicalTransaction = -1;
}

function validatePaymentSearchJournal(journalEntryId, imputationTypeId) {
    return validatePaymentJournalPromise = new Promise(function (resolve, reject) {

        lockScreen();
        setTimeout(function () {
        $.ajax({
            type: "POST",
            url: ACC_ROOT + "BillSearch/ValidatePaymentRevertion",
            data: { "sourceId": journalEntryId, "imputationTypeId": imputationTypeId }
            }).done(function (paymentData) {
                unlockScreen();
                resolve(paymentData);
            });
        }, 500);
    });
}

