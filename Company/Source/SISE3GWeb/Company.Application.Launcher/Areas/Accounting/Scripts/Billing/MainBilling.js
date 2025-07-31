/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                        DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                        */
/*--------------------------------------------------------------------------------------------------------------------------*/

var timerLoadBilling = 0;
var payerId = -1;
var totalPolicy = 0;
var totalReceipt = 0;
var difference = 0;
var paymentMethod = "";
var isOpen = false;
var billControlId = 0;
var tempImputationId = -1;
var imputationTotalControl = 0;
var totalAmount = 0;
var editCash = -1;
var editCheck = -1;
var editCreditCard = -1;
var editConsignment = -1;
var editConsignmentCheck = -1;
var editTransfer = -1;
var editDatafono = -1;
var editRetention = -1;
var branchId = 0;
var editPremium = 0;
var totalPremium = 0;
var issuingBankId = -1;
var exchangeRate = -1;
var insuredId = 0;
var retentionPolicyId = 0;
var currentRate = 0;
var newRate = 0;

/* Aplicar recibo */
var applyBillId;
var applyReceiptNumber;
var applyDepositerBilling;
var applyAmount;
var applyLocalAmount;
var applyBranch;
var applyIncomeConcept;
var applyPostedValue;
var applyDescription;
var applyAccountingDate;
var applyComments;
var applyTechnicalTransaction;
/* Fin aplicar recibo */

/* Agente */
var agentId = 0;
var agentEnabled = -1;
var itemsEnabled = 0;
var currencyId = -1;
var paymentConceptId = -1;
var percentageDifference = 0;
var agentType = 0;
var agentTypeId = 0;
var agentAgencyId = 0;
var editAgent = -1;
/* Fin agente */

function RowModel() {
    this.PaymentMethod;
    this.CurrencyId;
    this.CurrencyDescription;
    this.Amount;
    this.ExchangeRate;
    this.LocalAmount;
}

function RowPaymentModel() {
    this.PaymentTypeId;
    this.PaymentTypeDescription;
    this.CurrencyId;
    this.Currency;
    this.Amount;
    this.ExchangeRate;
    this.LocalAmount;
    this.DocumentNumber;
    this.VoucherNumber;
    this.CardNumber;
    this.CardType;
    this.CardTypeName;
    this.AuthorizationNumber;
    this.CardsName;
    this.ValidThruMonth;
    this.ValidThruYear;
    this.IssuingBankId;
    this.IssuingBankName;
    this.IssuingBankAccountNumber;
    this.IssuerName;
    this.RecievingBankId;
    this.RecievingBankName;
    this.RecievingBankAccountNumber;
    this.SerialVoucher;
    this.SerialNumber;
    this.Date;
    this.TaxBase;
    this.Information;
    this.IssueDate;
    this.ExpirationDate;
    this.BranchId;
    this.Branch;
    this.PrefixId;
    this.Prefix;
    this.PolicyNumber;
    this.EndorsementNumber;
    this.RetentionConceptId;
    this.RetentionConcept;
    this.InvoiceNumber;
    this.InvoiceDate;

}

function RowPremiumModel() {
    this.BranchId;
    this.BranchName;
    this.CurrencyId;
    this.CurrencyName;
    this.EndorsementNumber;
    this.PayerDocumentNumber;
    this.PayerId;
    this.PayerName;
    this.PolicyId;
    this.PolicyNumber;
    this.QuotaNumber;
    this.QuotaValue;
    this.Amount;
    this.ExchangeRate;
    this.LocalAmount;
    this.ItemId;
    this.EndorsementId;
    this.DocumentType;
    this.BillId;
    this.ItemTypeId;
    this.Reference;
    this.IndividualId;
}

/* Agente */
function RowAgentModel() {
    this.BranchId;
    this.BranchName;
    this.SalePointId;
    this.SalePointName;
    this.CompanyId;
    this.CompanyName;
    this.Description;
    this.AgentId;
    this.AgentDocumentNumber;
    this.AgentName;
    this.ConceptId;
    this.ConceptName;
    this.NatureId;
    this.NatureName;
    this.CurrencyId;
    this.CurrencyName;
    this.ExchangeRate;
    this.Amount;
    this.LocalAmount;
    this.BrokerCheckingAccountItemId;
    this.AgentTypeCode;
    this.AgentAgencyCode;
    this.BillNumber;
    this.Status;
}

/* Fin agente */

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
    TaxBase: 0,

    IssueDate: "",
    ExpirationDate: "",
    BranchId: 0,
    Branch: null,
    PrefixId: 0,
    Prefix: null,
    PolicyNumber: null,
    EndorsementNumber: null,
    RetentionConceptId: 0,
    RetentionConcept: null
};

var oItemsToPayGrid = {
    billItem: []
};

var oBillItem = {
    billItemId: 0,
    billID: 0,
    itemTypeId: 0,
    description: "",
    amount: 0,
    currencyId: 0,
    exchangeRate: 0,
    paidAmount: 0,
    column1: "",
    column2: "",
    column3: "",
    column4: "",
    column5: "",
    column6: "",
    column7: ""
};

var oBillModel = {
    BillId: 0,
    BillingConceptId: 0,
    BillControlId: 0,
    RegisterDate: null,
    Description: null,
    PaymentsTotal: 0,
    PayerId: -1,
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
    Date: null,
    IssueDate: null,
    ExpirationDate: null,
    BranchId: 0,
    PrefixId: 0,
    PolicyNumber: null,
    EndorsementNumber: null,
    RetentionConceptId: 0,
    InvoiceNumber: null,
    InvoiceDate: null
};

// Variables para arreglar el problema de borrado que tienen los campos autocomplete cuando pierde foco
var personDocumentNumber;
var personName;
var IsNewMainBilling = true;
var preliquidationBranch = 0;
var selectedInsured;
var selectedCompany;
var selectedEmployee;
var selectedAgent;
var selectedSupplier;
var uiScreen = 0;
var selectedThird;
var defaultValueBranch;

$(document).ready(function () {
    $("#alert-risk-list").UifAlert('hide');
    personDocumentNumber = $('#inputDocumentNumber').val();
    personName = $('#inputName').val();

    $('#RetentionBillDate').UifDatepicker();
    $('#RetentionIssueDate').UifDatepicker();
    $('#RetentionExpirationDate').UifDatepicker();
    $('#TransferDate').UifDatepicker();

    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                  ACCIONES / EVENTOS                                                      */
    /*--------------------------------------------------------------------------------------------------------------------------*/
    $("#DocumentType").attr("disabled", true);
    $("#DocumentType").prop("disabled", true);
    $("#rowInsured").show();
    $("#rowInsuredName").show();
    $("#rowAgent").hide();
    $("#rowAgentName").hide();
    $("#rowCompany").hide();
    $("#rowCompanyName").hide();
    $("#rowOther").hide();
    $("#rowOtherName").hide();
    $("#rowContractor").hide();
    $("#rowContractorName").hide();
    $("#rowSupplier").hide();
    $("#rowSupplierName").hide();
    $("#rowReinsurance").hide();
    $("#rowReinsuranceName").hide();
    $("#rowThird").hide();
    $("#rowThirdName").hide();
    $("#rowPerson").hide();
    $("#rowPersonName").hide();

    if ($("#ViewBagBranchDisable").val() == "1") {
        $("#selectBranch").attr("disabled", "disabled");
    }
    else {
        $("#selectBranch").removeAttr("disabled");
    }

    //Valida si viene de la aplicaciòn de Preliquidación ?
    if ($("#ViewBagIsApply").val() == "1") {
        IsNewMainBilling = false;
        lockScreen();
        loadBilling();
    }
    else {
        $("#TotalApplication").removeAttr("disabled", true);
        $("#idPremium").show();
    }

    defaultValueBranch = $("#ViewBagBranchDefault").val();
    loadBranchs();

    $('#inputDocumentNumber').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListPerson();
        }
    });

    $('#inputName').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListPerson();
        }
    });

    $('#inputAgentDocumentNumber').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListAgent();
        }
    });

    $('#inputAgentName').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListAgent();
        }
    });

    $('#inputCompanyDocumentNumber').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListCompany();
        }
    });

    $('#inputCompanyName').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListCompany();
        }
    });

    $('#inputOtherDocumentNumber').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListOther();
        }
    });

    $('#inputOtherName').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListOther();
        }
    });

    $('#inputThirdDocumentNumber').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListThird();
        }
    });

    $('#inputThirdName').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListThird();
        }
    });

    $('#inputInsuredDocumentNumber').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListInsured();
        }
    });

    $('#inputInsuredName').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListInsured();
        }
    });

    $('#inputContractorDocumentNumber').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListContractor();
        }
    });

    $('#inputContractorName').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListContractor();
        }
    });

    $('#inputSupplierDocumentNumber').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListSuplier();
        }
    });

    $('#inputSupplierName').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListSuplier();
        }
    });

    $('#inputReinsuranceDocumentNumber').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListReinsurance();
        }
    });

    $('#inputReinsuranceName').on('itemSelected', function (event, selectedItem) {
        if (!isNull(selectedItem) && !isNull(selectedItem.Id)) {
            validateRiksListReinsurance();
        }
    });
});

function loadBranchs() {
    MainBillingRequest.GetBranchs().done(function (data) {
        if (isEmpty(defaultValueBranch) && defaultValueBranch != '0') {
            $("#selectBranch").UifSelect({ sourceData: data.data });
        }
        else {
            $("#selectBranch").UifSelect({ sourceData: data.data, selectedId: defaultValueBranch });
            OpenBranchDefault();
        }
    });
}

//Verifica si están llenados todos los campos
function loadAllFieldsBilling() {
    if (($("#policiesListView").UifListView("getData").length > 0) && (

        ($("#inputDocumentNumber").val() != "" && $("#inputDocumentNumber").val() != null) &&
        ($("#inputName").val() != "" && $("#inputName").val() != null) &&
        ($("#Observation").val() != "" && $("#Observation").val() != null) &&
        ($("#selectBranch").val() != "" && $("#selectBranch").val() != null) &&
        ($("#TotalApplication").val() != "" && $("#TotalApplication").val() != null) &&
        ($("#CollectionTo").val() != "" && $("#CollectionTo").val() != null) &&
        ($("#DocumentType").val() != "" && $("#DocumentType").val() != null))
    ) {

        $("#TotalApplication").attr("disabled", true);
        $("#idPremium").hide();

        if (uiScreen != 1) {
            unlockScreen();

        }
    }
}

//Carga las primas 
function loadPremiumsReceivable(tempImputationIdPreliquidation) {

    $("#policiesListView").UifListView({
        height: '50%',
        theme: 'dark',
        source: ACC_ROOT + "PremiumsReceivable/GetTempPremiumReceivableItemByTempImputationId?tempImputationId=" + tempImputationIdPreliquidation,
        customDelete: false,
        customEdit: false,
        edit: false,
        delete: false,
        displayTemplate: "#policies-template"
    });
}


$("#BillingDate").val($("#ViewBagAccountingDate").val());

var deleteSelectedPremiumCallback = function (deferred, data) {
    deferred.resolve();

    setTimeout(function () {
        SetPremiumsSummary();
        SetTotalApplicationPayment();
    }, 1000);
};

// ListView pólizas
$("#policiesListView").UifListView(
    {
        height: '50%',
        customDelete: false,
        customAdd: false,
        customEdit: false,
        edit: false,
        delete: true,
        deleteCallback: deleteSelectedPremiumCallback,
        displayTemplate: "#policies-template"
    });

var deleteSummaryCashCallback = function (deferred, data) {
    if (data.PaymentMethod == "Efectivo") {
        $("#modalCashAdd").find("#cashsListView").UifListView({
            autoHeight: true, customAdd: false, customEdit: true, localMode: true, customDelete: false, add: false, edit: true,
            delete: true, displayTemplate: "#cash-display-template", editCallback: editCashCallback, deleteCallback: deleteCashCallback
        });
        $("#TotalApplicationCash").val("");
        SetTotalPayment("E");
        SetCashSummary();
    }

    if (data.PaymentMethod == "Cheque") {
        $("#modalCheckAdd").find("#checksListView").UifListView({
            autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
            delete: true, displayTemplate: "#check-display-template", editCallback: editCheckCallback, deleteCallback: deleteCheckCallback
        });
        SetTotalPayment("C");
        SetCheckSummary();
    }

    if (data.PaymentMethod == "Consignación") {
        LoadconsigListView();
        SetTotalPayment("B");
        SetConsignmentSummary();
    }

    if (data.PaymentMethod == "Consignación de Cheques") {
        LoadConsignmentCheckListView();
        SetTotalPayment("CH");
        SetConsignmentCheckSummary();
    }

    if (data.PaymentMethod == "Tarjeta") {
        LoadcardsListView();
        SetTotalPayment("T");
        SetCardSummary();
    }

    if (data.PaymentMethod == "Transferencia") {
        $("#modalTransferAdd").find("#transferListView").UifListView({
            autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
            delete: true, displayTemplate: "#transfer-display-template", editCallback: editTransferCallback, deleteCallback: deleteTransferCallback
        });
        SetTotalPayment("TR");
        SetTransferSummary();
    }

    if (data.PaymentMethod == "Datafono") {
        $("#modalDatafonoAdd").find("#datafonoListView").UifListView({
            autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
            delete: true, displayTemplate: "#datafono-display-template", editCallback: editDatafonoCallback, deleteCallback: deleteDatafonoCallback
        });
        SetTotalPayment("D");
        SetDatafonoSummary();
    }

    if (data.PaymentMethod == "Retenciones") {
        $("#modalRetentionAdd").find("#retentionListView").UifListView({
            autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
            delete: true, displayTemplate: "#retention-display-template", editCallback: editRetentionCallback, deleteCallback: deleteRetentionCallback
        });
        SetTotalPayment("R");
        SetRetentionSummary();
    }

    SetTotalApplicationPayment();
    //deferred.resolve();
};

// ListView recibos
$("#summariesListView").UifListView({
    height: '40%', theme: 'dark',
    customAdd: false, customEdit: false, customDelete: false, localMode: true, add: false, edit: false, delete: true,
    displayTemplate: "#summaries-template",
    deleteCallback: deleteSummaryCashCallback
});

$("#CheckExchangeRate").on('blur', function (event, selectedItem) {
    ValidateExchangeRate('CheckExchangeRate', 'selectCheckCurrency', 'CheckAmount', 'CheckLocalAmount', 'modalCheckAdd');
});

$('#summariesListView').on('rowDelete', function (event, data) {

});

$("#CollectionTo").on('binded', function () {
    if ($("#ViewBagPreliquidationPersonTypeId").val() != "0") {
        $("#CollectionTo").val($("#ViewBagPreliquidationPersonTypeId").val());
        $("#CollectionTo").trigger('itemSelected');
    }
});

///////////////////////////////////////////////////////
/// Combo cobranza a                                ///
///////////////////////////////////////////////////////
$("#CollectionTo").on('itemSelected', function (event, selectedItem) {
    //$("#alert").hide();
    $("#alert").UifAlert('hide');
    personDocumentNumber = "";
    personName = "";

    $("#inputDocumentNumber").val("");
    $("#inputName").val("");
    $("#inputAgentDocumentNumber").val("");
    $("#inputAgentName").val("");
    $("#inputCompanyDocumentNumber").val("");
    $("#inputCompanyName").val("");
    $("#inputOtherDocumentNumber").val("");
    $("#inputOtherName").val("");
    $("#inputInsuredDocumentNumber").val("");
    $("#inputInsuredName").val("");
    $("#inputContractorDocumentNumber").val("");
    $("#inputContractorName").val("");
    $("#inputSupplierDocumentNumber").val("");
    $("#inputSupplierName").val("");
    $("#inputReinsuranceDocumentNumber").val("");
    $("#inputReinsuranceName").val("");
    $("#inputThirdDocumentNumber").val("");
    $("#inputThirdName").val("");

    $("#DocumentType").val("");
    $("#DocumentType").attr("disabled", "disabled");
    //debugger
    if ($("#CollectionTo").val() != "") {
        $("#rowPerson").hide();

        if ($("#CollectionTo").val() == $("#ViewBagSupplierCode").val()) { // 10 - Proveedor
            $("#rowSupplier").show();
            $("#rowSupplierName").show();
            $("#rowAgent").hide();
            $("#rowAgentName").hide();
            $("#rowCompany").hide();
            $("#rowOther").hide();
            $("#rowInsured").hide();
            $("#rowContractor").hide();
            $("#rowReinsurance").hide();
            $("#rowThird").hide();
            $("#rowInsuredName").hide();
            $("#inputSupplierDocumentNumber").UifAutoComplete('setValue', $("#ViewBagDocumentNumber").val());
            $("#inputSupplierName").UifAutoComplete('setValue', $("#ViewBagName").val());
        }
        else if ($("#CollectionTo").val() == $("#ViewBagInsuredCode").val()) { // 7 - Asegurado  
            $("#rowInsured").show();
            $("#rowInsuredName").show();
            $("#rowAgent").hide();
            $("#rowAgentName").hide();
            $("#rowCompany").hide();
            $("#rowCompanyName").hide();
            $("#rowOther").hide();
            $("#rowOtherName").hide();
            $("#rowContractor").hide();
            $("#rowContractorName").hide();
            $("#rowSupplier").hide();
            $("#rowSupplierName").hide();
            $("#rowReinsurance").hide();
            $("#rowReinsuranceName").hide();
            $("#rowThird").hide();
            $("#rowThirdName").hide();
            $("#rowPerson").hide();
            $("#rowPersonName").hide();
            $("#inputInsuredDocumentNumber").UifAutoComplete('setValue', $("#ViewBagDocumentNumber").val());
            $("#inputInsuredName").UifAutoComplete('setValue', $("#ViewBagName").val());
        }

        else if ($("#CollectionTo").val() == $("#ViewBagOthersCode").val() ||
            $("#CollectionTo").val() == $("#ViewBagCollectorCode").val() ||
            $("#CollectionTo").val() == $("#ViewBagTradeAdviserCode").val()) { // 3 otros|| 4 cobrador ||14 asesor comercial  
            payerId = -1;
            $("#rowOther").show();
            $("#DocumentType").removeAttr("disabled");
            $("#rowAgent").hide();
            $("#rowCompany").hide();
            $("#rowInsured").hide();
            $("#rowContractor").hide();
            $("#rowSupplier").hide();
            $("#rowReinsurance").hide();
            $("#rowThird").hide();
            $("#inputOtherDocumentNumber").UifAutoComplete('setValue', $("#ViewBagDocumentNumber").val());
            $("#inputOtherName").UifAutoComplete('setValue', $("#ViewBagName").val());
        }
        else if ($("#CollectionTo").val() == $("#ViewBagCoinsurerCode").val()) { // 17 - coaseguradora  
            $("#rowThird").hide();
            $("#rowAgent").hide();
            $("#rowCompany").show();
            $("#rowOther").hide();
            $("#rowInsured").hide();
            $("#rowContractor").hide();
            $("#rowSupplier").hide();
            $("#rowReinsurance").hide();
            $("#inputCompanyDocumentNumber").UifAutoComplete('setValue', $("#ViewBagDocumentNumber").val());
            $("#inputCompanyName").UifAutoComplete('setValue', $("#ViewBagName").val());
        }
        else if ($("#CollectionTo").val() == $("#ViewBagThird").val()) { // 8 - tercero  
            $("#rowAgent").hide();
            $("#rowCompany").hide();
            $("#rowThird").show();
            $("#rowOther").hide();
            $("#rowInsured").hide();
            $("#rowContractor").hide();
            $("#rowSupplier").hide();
            $("#rowReinsurance").hide();
            $("#inputCompanyDocumentNumber").UifAutoComplete('setValue', $("#ViewBagDocumentNumber").val());
            $("#inputCompanyName").UifAutoComplete('setValue', $("#ViewBagName").val());
        }

        else if ($("#CollectionTo").val() == $("#ViewBagEmployeeCode").val()) { // 11-EMPLEADO 
            $("#rowPerson").show();
            $("#rowAgent").hide();
            $("#rowCompany").hide();
            $("#rowThird").hide();
            $("#rowOther").hide();
            $("#rowInsured").hide();
            $("#rowContractor").hide();
            $("#rowSupplier").hide();
            $("#rowReinsurance").hide();
            $("#inputDocumentNumber").UifAutoComplete('setValue', $("#ViewBagDocumentNumber").val());
            $("#inputName").UifAutoComplete('setValue', $("#ViewBagName").val());
        }
        else if ($("#CollectionTo").val() == $("#ViewBagReinsurerCode").val()) { // 2 Reasuguradora  
            $("#rowReinsurance").show();
            $("#rowReinsuranceName").show();
            $("#rowAgent").hide();
            $("#rowCompany").hide();
            $("#rowThird").hide();
            $("#rowOther").hide();
            $("#rowInsured").hide();
            $("#rowInsuredName").hide();
            $("#rowContractor").hide(); 
            $("#rowSupplier").hide();
        }
        else if ($("#CollectionTo").val() == $("#ViewBagAgentCode").val()) {// 1 -AGENTE                
            $("#rowAgent").show();
            $("#rowAgentName").show();
            $("#rowInsured").hide();
            $("#rowInsuredName").hide();
            $("#rowCompany").hide();
            $("#rowCompanyName").hide();
            $("#rowOther").hide();
            $("#rowOtherName").hide();
            $("#rowContractor").hide();
            $("#rowContractorName").hide();
            $("#rowSupplier").hide();
            $("#rowSupplierName").hide();
            $("#rowReinsurance").hide();
            $("#rowReinsuranceName").hide();
            $("#rowThird").hide();
            $("#rowThirdName").hide();
            $("#rowPerson").hide();
            $("#rowPersonName").hide();
            $("#inputAgentDocumentNumber").UifAutoComplete('setValue', $("#ViewBagDocumentNumber").val());
            $("#inputAgentName").UifAutoComplete('setValue', $("#ViewBagName").val());
        }
    }
    else {
        $("#rowInsured").show();
        $("#rowInsuredName").show();
    }
});

/////////////////////////////////////////
// Autocomplete número documento       //
/////////////////////////////////////////
$('#inputDocumentNumber').on('itemSelected', function (event, selectedItem) {

    //$("#alert").hide();
    $("#alert").UifAlert('hide');
    CleanAllFields();
    selectedEmployee = selectedItem;
    payerId = selectedItem.IndividualId;
    if (payerId > 0) {
        $("#inputName").UifAutoComplete('setValue', selectedItem.Name);
        $("#inputDocumentNumber").UifAutoComplete('setValue', selectedItem.DocumentNumber);
        personDocumentNumber = selectedItem.DocumentNumber;
        personName = selectedItem.Name;
        $("#DocumentType").val(selectedItem.DocumentTypeId);
    }
    else {
        CleanAllFields();
        selectedEmployee = undefined;
        personDocumentNumber = "";
        personName = "";
    }
});

////////////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo de número de documento //
////////////////////////////////////////////////////////////////////////
$("#inputDocumentNumber").on('blur', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    setTimeout(function () {
        if (selectedEmployee != undefined) {
            selectedItem = selectedEmployee;
            if (selectedItem.Id > 0) {
                $('#inputDocumentNumber').UifAutoComplete('setValue', personDocumentNumber);
            }
            else {
                CleanAllFields();
            }
        } else {
            CleanAllFields();
            personDocumentNumber = "";
            personName = "";
        }

    }, 50);
});

/////////////////////////////////////
// Autocomplete nombre             //
/////////////////////////////////////
$('#inputName').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();
    selectedEmployee = selectedItem;
    payerId = selectedItem.IndividualId;
    if (payerId > 0) {

        $("#inputName").UifAutoComplete('setValue', selectedItem.Name);
        $("#inputDocumentNumber").UifAutoComplete('setValue', selectedItem.DocumentNumber);
        personDocumentNumber = selectedItem.DocumentNumber;
        personName = selectedItem.Name;
        $("#DocumentType").val(selectedItem.DocumentTypeId);
    }
    else {
        CleanAllFields();
        selectedEmployee = undefined;
        personDocumentNumber = "";
        personName = "";
    }
});

////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo nombre //
////////////////////////////////////////////////////////
$("#inputName").on('blur', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    setTimeout(function () {
        if (selectedEmployee != undefined) {
            selectedItem = selectedEmployee;
            if (selectedItem.Id > 0) {
                $('#inputName').UifAutoComplete('setValue', personName);
            }
            else {
                CleanAllFields();
            }
        } else {
            CleanAllFields();
            personDocumentNumber = "";
            personName = "";
        }
    }, 50);

});

//////////////////////////////////////////
// Autocomplete número documento agente //
//////////////////////////////////////////
$('#inputAgentDocumentNumber').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();
    selectedAgent = selectedItem;
    payerId = selectedItem.IndividualId;
    if (payerId > 0) {
        $("#inputAgentName").UifAutoComplete('setValue', selectedItem.Name);
        $("#inputAgentDocumentNumber").UifAutoComplete('setValue', selectedItem.DocumentNumber);
        personDocumentNumber = selectedItem.DocumentNumber;
        personName = selectedItem.Name;
        $("#DocumentType").val(selectedItem.AgentTypeId);
    }
    else {
        CleanAllFields();
        $('#inputAgentDocumentNumber').UifAutoComplete('setValue', '');
        $('#inputAgentName').UifAutoComplete('setValue', '');
        selectedAgent = undefined;
        personDocumentNumber = "";
        personName = "";
    }
});

///////////////////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo de número de documento agente //
///////////////////////////////////////////////////////////////////////////////
$("#inputAgentDocumentNumber").on('blur', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    setTimeout(function () {
        if (selectedAgent != undefined) {
            selectedItem = selectedAgent;
            if (selectedItem.Id > 0) {
                $('#inputAgentDocumentNumber').val(personDocumentNumber);
            }
            else {
                $('#inputAgentDocumentNumber').UifAutoComplete('setValue', '');
                $('#inputAgentName').UifAutoComplete('setValue', '');
            }

        } else {
            $('#inputAgentDocumentNumber').UifAutoComplete('setValue', '');
            $('#inputAgentName').UifAutoComplete('setValue', '');
            personDocumentNumber = "";
            personName = "";
        }
    }, 50);
});

/////////////////////////////////////
// Autocomplete nombre agente      //
/////////////////////////////////////
$('#inputAgentName').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();
    selectedAgent = selectedItem;
    payerId = selectedItem.IndividualId;
    if (payerId > 0) {
        $("#inputAgentName").UifAutoComplete('setValue', selectedItem.Name);
        $("#inputAgentDocumentNumber").UifAutoComplete('setValue', selectedItem.DocumentNumber);
        personDocumentNumber = selectedItem.DocumentNumber;
        personName = selectedItem.Name;
        setDocumentTypeId(payerId);
    }
    else {
        CleanAllFields();
        $('#inputAgentDocumentNumber').UifAutoComplete('setValue', '');
        $('#inputAgentName').UifAutoComplete('setValue', '');
        selectedAgent = undefined;
        personDocumentNumber = "";
        personName = "";
    }
});

///////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo nombre agente //
///////////////////////////////////////////////////////////////
$("#inputAgentName").on('blur', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    setTimeout(function () {
        if (selectedAgent != undefined) {
            selectedItem = selectedAgent;
            if (selectedItem.Id > 0) {
                $('#inputAgentName').UifAutoComplete('setValue', personName);
            }
            else {
                $('#inputAgentDocumentNumber').UifAutoComplete('setValue', '');
                $('#inputAgentName').UifAutoComplete('setValue', '');
            }
        } else {
            $('#inputAgentDocumentNumber').UifAutoComplete('setValue', '');
            $('#inputAgentName').UifAutoComplete('setValue', '');
            personDocumentNumber = "";
            personName = "";
        }
    }, 50);

});

////////////////////////////////////////////
// Autocomplete número documento compañía //
////////////////////////////////////////////
$('#inputCompanyDocumentNumber').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();
    selectedCompany = selectedItem;
    payerId = selectedItem.CoinsuranceIndividualId;
    if (payerId > 0) {
        $("#inputCompanyName").UifAutoComplete('setValue', selectedItem.CoinsuranceName);
        $("#inputCompanyDocumentNumber").UifAutoComplete('setValue', selectedItem.CoinsuranceDocumentNumber);
        personDocumentNumber = selectedItem.CoinsuranceDocumentNumber;
        personName = selectedItem.CoinsuranceName;
        setDocumentTypeId(payerId);
    }
    else {
        CleanAllFields();
        $('#inputCompanyDocumentNumber').UifAutoComplete('setValue', '');
        $('#inputCompanyName').UifAutoComplete('setValue', '');
        selectedCompany = undefined;
        personDocumentNumber = "";
        personName = "";
    }
});

/////////////////////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo de número de documento compañía //
/////////////////////////////////////////////////////////////////////////////////
$("#inputCompanyDocumentNumber").on('blur', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    setTimeout(function () {
        if (selectedCompany != undefined) {
            selectedItem = selectedCompany;
            if (selectedItem.Id > 0) {
                setTimeout(function () {
                    $('#inputCompanyDocumentNumber').UifAutoComplete('setValue', personDocumentNumber);
                }, 50);
            }
            else {
                $('#inputCompanyDocumentNumber').UifAutoComplete('setValue', '');
                $('#inputCompanyName').UifAutoComplete('setValue', '');
            }

        } else {
            $('#inputCompanyDocumentNumber').UifAutoComplete('setValue', '');
            $('#inputCompanyName').UifAutoComplete('setValue', '');
            personDocumentNumber = "";
            personName = "";
        }
    }, 50);
});

/////////////////////////////////////
// Autocomplete nombre compañía    //
/////////////////////////////////////
$('#inputCompanyName').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();
    selectedCompany = selectedItem;
    payerId = selectedItem.CoinsuranceIndividualId;
    if (payerId > 0) {
        $("#inputCompanyName").UifAutoComplete('setValue', selectedItem.CoinsuranceName);
        $("#inputCompanyDocumentNumber").UifAutoComplete('setValue', selectedItem.CoinsuranceDocumentNumber);
        personDocumentNumber = selectedItem.CoinsuranceDocumentNumber;
        personName = selectedItem.CoinsuranceName;
        setDocumentTypeId(payerId);
    }
    else {
        CleanAllFields();
        $('#inputCompanyDocumentNumber').UifAutoComplete('setValue', '');
        $('#inputCompanyName').UifAutoComplete('setValue', '');
        selectedCompany = undefined;
        personDocumentNumber = "";
        personName = "";
    }
});

/////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo nombre compañía //
/////////////////////////////////////////////////////////////////
$("#inputCompanyName").on('blur', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    setTimeout(function () {
        if (selectedCompany != undefined) {
            selectedItem = selectedCompany;
            if (selectedItem.Id > 0) {
                $('#inputCompanyName').UifAutoComplete('setValue', personName);
            }
            else {
                $('#inputCompanyDocumentNumber').UifAutoComplete('setValue', '');
                $('#inputCompanyName').UifAutoComplete('setValue', '');
            }
        } else {
            $('#inputCompanyDocumentNumber').UifAutoComplete('setValue', '');
            $('#inputCompanyName').UifAutoComplete('setValue', '');
            personDocumentNumber = "";
            personName = "";
        }
    }, 50);
});

/////////////////////////////////////////////
// Autocomplete número documento asegurado //
/////////////////////////////////////////////
$('#inputInsuredDocumentNumber').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();
    selectedInsured = selectedItem;
    payerId = selectedItem.Id;
    if (payerId > 0) {
        $("#inputInsuredDocumentNumber").UifAutoComplete('setValue', selectedItem.DocumentNumber);
        $("#inputInsuredName").UifAutoComplete('setValue', selectedItem.Name);
        personDocumentNumber = selectedItem.DocumentNumber;
        personName = selectedItem.Name;
        $("#DocumentType").val(selectedItem.DocumentTypeId);
        $("#formBilling").validate().element("#inputInsuredName");
    }
    else {
        CleanAllFields();
        $('#inputInsuredDocumentNumber').UifAutoComplete('setValue', '');
        $('#inputInsuredName').UifAutoComplete('setValue', '');
        selectedInsured = undefined;
        personDocumentNumber = "";
        personName = "";
    }
});

//////////////////////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo de número de documento asegurado //
//////////////////////////////////////////////////////////////////////////////////
$("#inputInsuredDocumentNumber").on('blur', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    setTimeout(function () {
        if (selectedInsured != undefined) {
            selectedItem = selectedInsured;
            if (selectedItem.Id > 0) {
                $('#inputInsuredDocumentNumber').UifAutoComplete('setValue', personDocumentNumber);
            }
            else {
                $('#inputInsuredDocumentNumber').UifAutoComplete('setValue', '');
                $('#inputInsuredName').UifAutoComplete('setValue', '');
            }
        } else {
            $('#inputInsuredDocumentNumber').UifAutoComplete('setValue', '');
            $('#inputInsuredName').UifAutoComplete('setValue', '');
            personDocumentNumber = "";
            personName = "";
        }
    }, 50);
});

/////////////////////////////////////
// Autocomplete nombre asegurado   //
/////////////////////////////////////
$('#inputInsuredName').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();
    selectedInsured = selectedItem;
    payerId = selectedItem.Id;
    if (payerId > 0) {
        $("#inputInsuredName").UifAutoComplete('setValue', selectedItem.Name);
        $("#inputInsuredDocumentNumber").UifAutoComplete('setValue', selectedItem.DocumentNumber);
        personDocumentNumber = selectedItem.DocumentNumber;
        personName = selectedItem.Name;
        $("#DocumentType").val(selectedItem.DocumentTypeId);
    }
    else {
        CleanAllFields();
        $('#inputInsuredDocumentNumber').UifAutoComplete('setValue', '');
        $('#inputInsuredName').UifAutoComplete('setValue', '');
        selectedInsured = undefined;
        personDocumentNumber = "";
        personName = "";
    }
});

//////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo nombre asegurado //
//////////////////////////////////////////////////////////////////
$("#inputInsuredName").on('blur', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    setTimeout(function () {
        if (selectedInsured != undefined) {
            selectedItem = selectedInsured;
            if (selectedItem.Id > 0) {
                $('#inputInsuredName').val(personName);
            }
            else {
                $('#inputInsuredDocumentNumber').UifAutoComplete('setValue', '');
                $('#inputInsuredName').UifAutoComplete('setValue', '');
            }
        } else {
            $('#inputInsuredDocumentNumber').UifAutoComplete('setValue', '');
            $('#inputInsuredName').UifAutoComplete('setValue', '');
            personDocumentNumber = "";
            personName = "";
        }
    }, 50);

});


/////////////////////////////////////////////
// Autocomplete número documento proveedor //
/////////////////////////////////////////////
$('#inputSupplierDocumentNumber').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();
    selectedSupplier = selectedItem;
    payerId = selectedItem.IndividualId;
    if (payerId > 0) {
        setValueAutoComplete('inputSupplier', selectedItem)
    }
    else {
        CleanAllFields();
        $('#inputSupplierDocumentNumber').UifAutoComplete('setValue', '');
        $('#inputSupplierName').UifAutoComplete('setValue', '');
        selectedSupplier = undefined;
        personDocumentNumber = "";
        personName = "";
    }
});


//////////////////////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo de número de documento proveedor //
//////////////////////////////////////////////////////////////////////////////////
$("#inputSupplierDocumentNumber").on('blur', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    setTimeout(function () {
        if (selectedSupplier != undefined) {
            selectedItem = selectedSupplier;
            if (selectedItem.Id > 0) {
                setTimeout(function () {
                    $('#inputSupplierDocumentNumber').UifAutoComplete('setValue', personDocumentNumber);
                }, 50);
            }
            else {
                $('#inputSupplierDocumentNumber').UifAutoComplete('setValue', '');
                $('#inputSupplierName').UifAutoComplete('setValue', '');
            }

        } else {
            $('#inputSupplierDocumentNumber').UifAutoComplete('setValue', '');
            $('#inputSupplierName').UifAutoComplete('setValue', '');
            personDocumentNumber = "";
            personName = "";
        }

    }, 50);
});

/////////////////////////////////////
// Autocomplete nombre proveedor   //
/////////////////////////////////////
$('#inputSupplierName').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();
    selectedSupplier = selectedItem;
    payerId = selectedItem.IndividualId;
    if (payerId > 0) {
        setValueAutoComplete('inputSupplier', selectedItem)
    }
    else {
        CleanAllFields();
        $('#inputSupplierDocumentNumber').UifAutoComplete('setValue', '');
        $('#inputSupplierName').UifAutoComplete('setValue', '');
        selectedSupplier = undefined;
        personDocumentNumber = "";
        personName = "";

    }
});

//////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo nombre proveedor //
//////////////////////////////////////////////////////////////////
$("#inputSupplierName").on('blur', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    setTimeout(function () {
        if (selectedSupplier != undefined) {
            selectedItem = selectedSupplier;
            if (selectedItem.Id > 0) {
                $('#inputSupplierName').UifAutoComplete('setValue', personName);
            }
            else {
                $('#inputSupplierDocumentNumber').UifAutoComplete('setValue', '');
                $('#inputSupplierName').UifAutoComplete('setValue', '');
            }
        } else {
            $('#inputSupplierDocumentNumber').UifAutoComplete('setValue', '');
            $('#inputSupplierName').UifAutoComplete('setValue', '');
            personDocumentNumber = "";
            personName = "";
        }
    }, 50);
});

///////////////////////////////////////////////
// Autocomplete número documento contratante //
///////////////////////////////////////////////
$('#inputContractorDocumentNumber').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();

    payerId = selectedItem.Id;
    if (payerId > 0) {
        $("#inputContractorName").UifAutoComplete('setValue', selectedItem.Name);
        $("#inputContractorDocumentNumber").UifAutoComplete('setValue', selectedItem.DocumentNumber);
        personDocumentNumber = selectedItem.DocumentNumber;
        personName = selectedItem.Name;
        $("#DocumentType").val(selectedItem.DocumentTypeId);
    }
    else {
        CleanAllFields();
    }
});

////////////////////////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo de número de documento contratante //
////////////////////////////////////////////////////////////////////////////////////
$("#inputContractorDocumentNumber").on('blur', function (event) {
    setTimeout(function () {
        $('#inputContractorDocumentNumber').UifAutoComplete('setValue', personDocumentNumber);
    }, 50);
});

/////////////////////////////////////
// Autocomplete nombre contratante //
/////////////////////////////////////
$('#inputContractorName').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();

    payerId = selectedItem.Id;
    if (payerId > 0) {
        $("#inputContractorName").UifAutoComplete('setValue', selectedItem.Name);
        $("#inputContractorDocumentNumber").UifAutoComplete('setValue', selectedItem.DocumentNumber);
        personDocumentNumber = selectedItem.DocumentNumber;
        personName = selectedItem.Name;
        $("#DocumentType").val(selectedItem.DocumentTypeId);
    }
    else {
        CleanAllFields();
    }
});

////////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo nombre contratante //
////////////////////////////////////////////////////////////////////
$("#inputContractorName").on('blur', function (event) {
    setTimeout(function () {
        $('#inputContractorName').UifAutoComplete('setValue', personName);
    }, 50);
});

///////////////////////////////////////////////////////////
// Autocomplete número documento compañía resaseguradora //
///////////////////////////////////////////////////////////
$('#inputReinsuranceDocumentNumber').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();

    payerId = selectedItem.ReinsurerIndividualId;
    if (payerId > 0) {
        $("#inputReinsuranceName").UifAutoComplete('setValue', selectedItem.Name);
        $("#inputReinsuranceDocumentNumber").UifAutoComplete('setValue', selectedItem.DocumentNumber);
        personDocumentNumber = selectedItem.DocumentNumber;
        personName = selectedItem.Name;
        $("#DocumentType").val(selectedItem.DocumentTypeId);
    }
    else {
        CleanAllFields();
    }
});

///////////////////////////////////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo de número de documento compañía reaseguradora //
///////////////////////////////////////////////////////////////////////////////////////////////
$("#inputReinsuranceDocumentNumber").on('blur', function (event) {
    setTimeout(function () {
        $('#inputReinsuranceDocumentNumber').UifAutoComplete('setValue', personDocumentNumber);
    }, 50);
});

////////////////////////////////////////////////
// Autocomplete nombre compañia reaseguradora //
////////////////////////////////////////////////
$('#inputReinsuranceName').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();

    payerId = selectedItem.ReinsurerIndividualId;
    if (payerId > 0) {
        $("#inputReinsuranceName").UifAutoComplete('setValue', selectedItem.Name);
        $("#inputReinsuranceDocumentNumber").UifAutoComplete('setValue', selectedItem.DocumentNumber);
        personDocumentNumber = selectedItem.DocumentNumber;
        personName = selectedItem.Name;
        $("#DocumentType").val(selectedItem.DocumentTypeId);
    }
    else {
        CleanAllFields();
    }
});

///////////////////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo nombre compañía reaseguradora //
///////////////////////////////////////////////////////////////////////////////
$("#inputReinsuranceName").on('blur', function (event) {
    setTimeout(function () {
        $('#inputReinsuranceName').UifAutoComplete('setValue', personName);
    }, 50);
});

///////////////////////////////////////////////////////////
// Autocomplete número documento compañía tercero        //
//////////////////////////////////////////////////////////

$('#inputThirdDocumentNumber').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();

    payerId = selectedItem.DocumentNumber;
    if (payerId > 0) {
        $("#inputThirdName").UifAutoComplete('setValue', selectedItem.Fullname);
        $("#inputThirdDocumentNumber").UifAutoComplete('setValue', selectedItem.DocumentNumber);
        personDocumentNumber = selectedItem.DocumentNumber;
        personName = selectedItem.Fullname;
        $("#DocumentType").val(selectedItem.DocumentTypeId);
    }
    else {
        CleanAllFields();
    }
});
///////////////////////////////////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo de número de documento compañía terceros      //
///////////////////////////////////////////////////////////////////////////////////////////////
$("#inputThirdDocumentNumber").on('blur', function (event) {
    setTimeout(function () {
        $('#inputThirdDocumentNumber').UifAutoComplete('setValue', personDocumentNumber);
    }, 50);
});

////////////////////////////////////////////////
// Autocomplete nombre compañia terceros      //
////////////////////////////////////////////////
$('#inputThirdName').on('itemSelected', function (event, selectedItem) {
    $("#alert").UifAlert('hide');
    CleanAllFields();

    payerId = selectedItem.DocumentNumber;
    if (payerId > 0) {
        $("#inputThirdName").UifAutoComplete('setValue', selectedItem.Fullname);
        $("#inputThirdDocumentNumber").UifAutoComplete('setValue', selectedItem.DocumentNumber);
        personDocumentNumber = selectedItem.DocumentNumber;
        personName = selectedItem.Name;
        $("#DocumentType").val(selectedItem.DocumentTypeId);
    }
    else {
        CleanAllFields();
    }
});

///////////////////////////////////////////////////////////////////////////////
// Control de borrado de autocomplete en campo nombre compañía terceros //
///////////////////////////////////////////////////////////////////////////////
$("#inputThirdName").on('blur', function (event) {
    setTimeout(function () {
        $('#inputThirdName').UifAutoComplete('setValue', personName);
    }, 50);
});


///////////////////////////////////////////////////////
/// Combo sucursal                                  ///
///////////////////////////////////////////////////////
$("#selectBranch").on('itemSelected', function (event, selectedItem) {
    OpenBranchDefault();
});

//////////////////////////////////////
// Botón Aceptar - Cierre Caja      //
//////////////////////////////////////
$("#modalBillingClosure").find('#AcceptClosure').click(function () {
    $("#modalBillingClosure").hide();

    window.location.href = $("#ViewBagBillingClosureIdLink").val() + "?billControlId=" + billControlId +
        "&branchId=" + $("#selectBranch").val();
});

//////////////////////////////////////
// Campo total aplicación           //
//////////////////////////////////////
$("#TotalApplication").blur(function () {
    $("#alert").UifAlert('hide');
    $("#TotalApplication").val(FormatMoneySymbol($("#TotalApplication").val()));
    SetTotalApplicationPayment();
});

//////////////////////////////////////
// Campo efectivo                   //
//////////////////////////////////////
$("#TotalApplicationCash").blur(function () {
    $("#alert").UifAlert('hide');
    if ($("#TotalApplication").val() == "") {
        $("#alert").UifAlert('show', Resources.EntryTotalApplication, "warning");
        $("#TotalApplication").removeAttr("disabled", false);
        return;
    }
    else if (RemoveFormatMoney($("#TotalApplication").val()) === 0) {
        $("#alert").UifAlert('show', Resources.TotalApplicationGreaterZero, "warning");
        return;
    }

    if ($("#TotalApplicationCash").val() != "") {
        var totalApplicationCash = RemoveFormatMoney($("#TotalApplicationCash").val());
        $("#TotalApplicationCash").val(FormatMoneySymbol(totalApplicationCash));
        AddApplicationCash(0);
        $("#TotalApplicationCash").val("");
    }
});

//////////////////////////////////////
// Botón Imprimir Detalle           //
//////////////////////////////////////
$("#modalSaveBill").find('#PrintDetailBilling').click(function () {
    ShowReceiptReportMainBilling($("#selectBranch").val(), $("#modalSaveBill").find("#BillingId").text(), $("#inputOtherName").val());
    $('#modalSaveBill').UifModal('hide');
    confirmModal();
});

//////////////////////////////////////
// Botón  Cancelar                  //
//////////////////////////////////////
$("#modalSaveBill").find('#SuccessCancell').click(function () {
    confirmModal();
});


$('#modalSaveBill').on('closed.modal', function () {
    confirmModal();
});

//////////////////////////////////////
// Botón Buscar                     //
//////////////////////////////////////
$('#SearchPolicy').click(function () {
    $("#alert").UifAlert('hide');

    //Se valida que ingrese un valor en el campo código
    if ($("#SearchCode").val() == "") {
        $("#alert").UifAlert('show', Resources.ValidationConciliationDate, "warning");
        setTimeout(function () {
            $("#alert").UifAlert('hide');
        }, 3000);

        return true;
    }
    var policyNumber = $("#SearchCode").val();
    var documentNumber = "";
    var payerName = "";
    var prefix = "";
    var branch = "";
    var holderDocumentNumber = "";
    var holderName = "";


    $("#policiesListView").UifListView(
        {
            height: '50%',
            source: ACC_ROOT + "Billing/GetPolicyQuotaListView?policyNumber=" + policyNumber + "&documentNumber="
                + documentNumber + "&payerName=" + payerName + "&prefix=" + prefix + "&branch="
                + branch + "&holderDocumentNumber=" + holderDocumentNumber + "&holderName=" + holderName,
            customDelete: false,
            customAdd: false,
            customEdit: true,
            edit: true,
            displayTemplate: "#policies-template",
            selectionType: 'multiple'
        });
    $("#policiesListView").UifListView('refresh');
});

////////////////////////////////////
// ListView pólizas               //
////////////////////////////////////
$('#policiesListView').on('rowEdit', function (event, selectedRow) {
    totalPolicy = totalPolicy + selectedRow.QuotaValue;
    $("#TotalPolicy").text(FormatMoneySymbol(totalPolicy));
    $("#TotalApplication").text(FormatMoneySymbol(totalPolicy));
    totalReceipt = RemoveFormatMoney($("#TotalReceipt").text());
    difference = totalPolicy - totalReceipt;

    $("#Difference").text(FormatMoneySymbol(difference));
});

///////////////////////////////////////////////////////////////////////
/// Tabla de pólizas: selección de items a pagar                    ///
///////////////////////////////////////////////////////////////////////
$('body').delegate('#tablePolicies tbody tr', "click", function () {

    if ($(this).hasClass('selected')) {
        $(this).removeClass('selected');
    }
    else {
        $(this).siblings('.selected').removeClass('selected');
        $(this).addClass('selected');
    }
    totalPolicy = 0;

    var policies = $("#tablePolicies").UifDataTable("getSelected");

    if (policies != null) {

        for (var j = 0; j < policies.length; j++) {
            totalPolicy += parseFloat(policies[j].PaymentAmount);
        }
    }
    else {
        $("#TotalPolicy").text("");
        $("#TotalApplication").text("");
    }
    totalReceipt = RemoveFormatMoney($("#TotalReceipt").text());
    difference = totalPolicy - totalReceipt;

    $("#Difference").text(FormatMoneySymbol(difference));
    $("#TotalPolicy").text(FormatMoneySymbol(totalPolicy));
    $("#TotalApplication").text(FormatMoneySymbol(totalPolicy));
});

/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                            CHEQUE                                                     ///
/////////////////////////////////////////////////////////////////////////////////////////////////////////////

var saveCheckCallback = function (deferred, data) {
    $("#modalCheckAdd").find("#checksListView").UifListView("addItem", data);
    deferred.resolve();
};

var editCheckCallback = function (deferred, data) {
    data.NombreEmpresa = "Sistran";
    deferred.resolve(data);
};

var deleteCheckCallback = function (deferred, data, index) {
    if (editCheck === index) {
        editCheck = -1;
    } else if (editCheck > index) {
        editCheck -= 1;
    }

    deferred.resolve();
    setTimeout(function () {
        SetTotalPayment("C");
        SetTotalApplicationPayment();
        SetCheckSummary();
    }, 1000);
};

$("#modalCheckAdd").find("#checksListView").UifListView({
    autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
    delete: true, displayTemplate: "#check-display-template", editCallback: editCheckCallback, deleteCallback: deleteCheckCallback
});

//////////////////////////////////////
// Botón Cheque                     //
//////////////////////////////////////
$('#OpenCheck').click(function () {
    paymentMethod = "C";
    editCheck = -1;
    currentRate = 0;
    issuingBankId: -1;
    if ($("#TotalApplication").val() == "") {
        $("#alert").UifAlert('show', Resources.EntryTotalApplication, "warning");
        return;
    }
    else {
        if (RemoveFormatMoney($("#TotalApplication").val()) === 0) {
            $("#alert").UifAlert('show', Resources.TotalApplicationGreaterZero, "warning");
            return;
        }
        $("#modalCheckAdd").find("#alertForm").hide();
        $("#addCheckForm").formReset();
        LoadDefaultCurrency($('#modalCheckAdd').find('#selectCheckCurrency'));
        $("#modalCheckAdd").find("#CheckDate").val($("#ViewBagDateAccounting").val());
        $('#modalCheckAdd').UifModal('showLocal', Resources.DepositingChecks);
    }
});

///////////////////////////////////////////////////////
/// Combo moneda - cheque                           ///
///////////////////////////////////////////////////////
$("#modalCheckAdd").find("#selectCheckCurrency").on('itemSelected', function (event, selectedItem) {
    $("#CheckExchangeRate").removeAttr("disabled");
    SetCurrencyMainBilling("selectCheckCurrency", "CheckExchangeRate", "selectCheckCurrency", "CheckAmount", "CheckLocalAmount", "modalCheckAdd");
    if ($("#modalCheckAdd").find("#CheckAmount").val() != "") {
        SetCheckLocalAmount();
    }
    
});

///////////////////////////////////////////////////////
/// Importe - cheque                                ///
///////////////////////////////////////////////////////
$("#modalCheckAdd").find("#CheckAmount").blur(function () {
    var checkAmount = RemoveFormatMoney($("#modalCheckAdd").find("#CheckAmount").val());
    $("#modalCheckAdd").find("#CheckAmount").val(FormatMoneySymbol(checkAmount));
    SetCheckLocalAmount();
});

/////////////////////////////////////////////////////////////
// Número documento - cheque                               //
/////////////////////////////////////////////////////////////
$("#modalCheckAdd").find("#CheckDocumentNumber").blur(function () {
    if ($("#modalCheckAdd").find("#CheckDocumentNumber").val() != "") {
        var exist = ValidCheckDocumentNumber();
        if (exist == 1) {
            $("#modalCheckAdd").find("#alertForm").UifAlert('show', Resources.WarningCheckNumberValid, "warning");
            $("#modalCheckAdd").find("#CheckDocumentNumber").val("");

            setTimeout(function () {
                $("#modalCheckAdd").find("#alertForm").hide();
            }, 3000);
        }
    }
});

////////////////////////////////////////
// Autocomplete banco emisor - cheque //
////////////////////////////////////////
$("#modalCheckAdd").find('#inputCheckIssuingBank').on('itemSelected', function (event, selectedItem) {
    issuingBankId = selectedItem.Id;
});
$("#modalCheckAdd").find('#inputCheckIssuingBank').blur(function (selectedItem) {
    if (issuingBankId == -1) {
        $.UifNotify('show', { type: 'info', message: Resources.EnterBankName, autoclose: true });
    }
});

/////////////////////////////////////
// ListView - cheque               //
/////////////////////////////////////
$("#modalCheckAdd").find('#checksListView').on('rowEdit', function (event, data, index) {

    if (data.PaymentTypeId == $("#ViewBagParamPaymentMethodPostdatedCheck").val()) {
        $("#modalCheckAdd").find('#checkPostDated').attr('class', "glyphicon glyphicon-check");
    }
    else {
        $("#modalCheckAdd").find('#checkPostDated').attr('class', "glyphicon glyphicon-unchecked");
    }

    $("#modalCheckAdd").find("#CheckAmount").val(data.Amount);
    $("#modalCheckAdd").find("#CheckExchangeRate").val(data.ExchangeRate);
    $("#modalCheckAdd").find("#CheckLocalAmount").val(data.LocalAmount);
    $("#modalCheckAdd").find("#selectCheckCurrency").UifSelect("setSelected", data.CurrencyId);
    $("#modalCheckAdd").find("#selectCheckCurrency").trigger("change");
    $("#modalCheckAdd").find("#inputCheckIssuingBank").val(data.IssuingBankName);
    $("#modalCheckAdd").find("#CheckAccountNumber").val(data.IssuingBankAccountNumber);
    $("#modalCheckAdd").find("#HiddenCheckAccountNumber").val(data.IssuingBankAccountNumber);
    $("#modalCheckAdd").find("#CheckDocumentNumber").val(data.DocumentNumber);
    $("#modalCheckAdd").find("#HiddenCheckDocumentNumber").val(data.DocumentNumber);
    $("#modalCheckAdd").find("#CheckHolderName").val(data.IssuerName);
    $("#modalCheckAdd").find("#CheckDate").val(data.Date);
    issuingBankId = data.IssuingBankId;
    $("#modalCheckAdd").find("#HiddenIssuingBankId").val(data.IssuingBankId);

    editCheck = index;
});

$("#modalCheckAdd").find('#checksListView').on('rowDelete', function (event, data) {
    SetTotalPayment("C");
    SetTotalApplicationPayment();
});

//////////////////////////
// Botón agregar modal ///
//////////////////////////
$("#modalCheckAdd").find('#CheckAdd').click(function () {
    ValidateExchangeRate("CheckExchangeRate", "selectCheckCurrency", "CheckAmount", "CheckLocalAmount", "modalCheckAdd", false);
    if (issuingBankId != -1) {
        checkAdd();
    } else {
        $.UifNotify('show', { type: 'info', message: Resources.EnterBankName, autoclose: true });
    }

});
function checkAdd() {
    $("#modalCheckAdd").find("#alertForm").hide();
    paymentMethod = "C";
    var existCheck = 0;
    $("#addCheckForm").validate();
    var lastAmount = $("#CheckAmount").val();
    previousValidAmount('CheckAmount');
    if ($("#addCheckForm").valid()) {
        $("#CheckAmount").val(lastAmount);
        var nameIsBlankChar = isBlankChar($.trim($("#CheckHolderName").val()));

        if (ValidateAddFormMain(paymentMethod) == true && nameIsBlankChar) {
            // Se valida que no se ingrese el mismo registro
            var keyValid = $("#addCheckForm").find("#CheckDocumentNumber").val() + issuingBankId +
                $("#addCheckForm").find("#CheckAccountNumber").val();

            var checks = $("#modalCheckAdd").find('#checksListView').UifListView("getData");

            if (checks != null) {
                for (var j = 0; j < checks.length; j++) {

                    if (editCheck != j) {
                        var checkIndex = checks[j].DocumentNumber + checks[j].IssuingBankId + checks[j].IssuingBankAccountNumber;
                        if (checkIndex == keyValid) {
                            existCheck = 1;
                            break;
                        }
                    }
                }
            }

            if (editCheck > -1) {
                if (($("#addCheckForm").find("#CheckDocumentNumber").val() == $("#addCheckForm").find("#HiddenCheckDocumentNumber").val()) &&
                    ($("#addCheckForm").find("#CheckAccountNumber").val() == $("#addCheckForm").find("#HiddenCheckAccountNumber").val()) &&
                    (issuingBankId == $("#addCheckForm").find("#HiddenIssuingBankId").val())) {
                    existCheck = 0;
                }
            }

            if (existCheck == 0) {
                var rowModel = new RowPaymentModel();
                var sortingPostDated = ($("#addCheckForm").find('#checkPostDated').hasClass('glyphicon glyphicon-check')) ? 1 : 0;

                if (sortingPostDated == 1) {
                    rowModel.PaymentTypeId = $("#ViewBagParamPaymentMethodPostdatedCheck").val();
                }
                else {
                    rowModel.PaymentTypeId = $("#ViewBagParamPaymentMethodCurrentCheck").val();
                }

                rowModel.PaymentTypeDescription = "CHEQUE";
                rowModel.CurrencyId = $("#addCheckForm").find('#selectCheckCurrency').val();
                rowModel.Currency = $("#addCheckForm").find('#selectCheckCurrency option:selected').text();
                rowModel.Amount = $("#addCheckForm").find('#CheckAmount').val();
                rowModel.ExchangeRate = $("#addCheckForm").find("#CheckExchangeRate").val();
                rowModel.LocalAmount = $("#addCheckForm").find("#CheckLocalAmount").val();
                rowModel.DocumentNumber = $("#addCheckForm").find("#CheckDocumentNumber").val();
                rowModel.VoucherNumber = "";
                rowModel.CardNumber = "";
                rowModel.CardType = "";
                rowModel.CardTypeName = "";
                rowModel.AuthorizationNumber = "";
                rowModel.CardsName = "";
                rowModel.ValidThruMonth = "";
                rowModel.ValidThruYear = "";
                rowModel.IssuingBankId = issuingBankId;
                rowModel.IssuingBankName = $("#addCheckForm").find("#inputCheckIssuingBank").val();
                rowModel.IssuingBankAccountNumber = $("#addCheckForm").find("#CheckAccountNumber").val();
                rowModel.IssuerName = $("#addCheckForm").find("#CheckHolderName").val();
                rowModel.RecievingBankId = "";
                rowModel.RecievingBankName = "";
                rowModel.RecievingBankAccountNumber = "";
                rowModel.SerialVoucher = "";
                rowModel.SerialNumber = "";
                rowModel.Date = $("#addCheckForm").find("#CheckDate").val();
                rowModel.TaxBase = "";
                rowModel.Information = "";

                $("#modalCheckAdd").find('#TotalCheck').text(rowModel.Amount);

                if (editCheck == -1) {
                    $("#modalCheckAdd").find('#checksListView').UifListView("addItem", rowModel);
                }
                else {
                    $("#modalCheckAdd").find('#checksListView').UifListView("editItem", editCheck, rowModel);
                    editCheck = -1;
                }
                $("#addCheckForm").formReset();
                $("#modalCheckAdd").find("#CheckDate").val($("#ViewBagDateAccounting").val());
                SetTotalPayment("C");
                $("#CheckExchangeRate").val("");
                $("#CheckExchangeRate").attr('disabled', 'disabled');
                LoadDefaultCurrency($('#modalCheckAdd').find('#selectCheckCurrency'));
                newRate = 0;
                SetCheckSummary();
            }
            else {
                $("#modalCheckAdd").find("#alertForm").UifAlert('show', Resources.ValidateCheckNumber, "warning");
            }
        }
        else {
            if (!nameIsBlankChar) {
                $("#modalCheckAdd").find("#alertForm").UifAlert('show', Resources.WarningInvalidLogon + " (" + Resources.PayHolderName + ")", "warning");
            }
        }
        SetTotalApplicationPayment();
    }
    else {
        $("#CheckAmount").val(lastAmount);
    }

}
$("#modalCardAdd").find('#CardClose').click(function () {
    setFocusControl("SaveBill");
});

/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                          CONSIGNACIONES                                               ///
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
var saveConsignmentCallback = function (deferred, data) {
    $("#modalConsignmentAdd").find("#consignmentsListView").UifListView("addItem", data);
    deferred.resolve();
};

var editConsignmentCallback = function (deferred, data) {
    data.NombreEmpresa = "Sistran";
    deferred.resolve(data);
};

var deleteConsignmentCallback = function (deferred, data, index) {
    if (editConsignment === index) {
        editConsignment = -1;
    } else if (editConsignment > index) {
        editConsignment -= 1;
    }
    deferred.resolve();
    setTimeout(function () {
        SetTotalPayment("B");
        SetTotalApplicationPayment();
        SetConsignmentSummary();
    }, 1000);
};

//////////////////////////////////////
// Botón Consignación               //
//////////////////////////////////////
$('#OpenConsignment').click(function () {
    paymentMethod = "B";
    editConsignment = -1;
    currentRate = 0;
    if ($("#TotalApplication").val() == "") {
        $("#alert").UifAlert('show', Resources.EntryTotalApplication, "warning");
        return;
    }
    else {
        if (RemoveFormatMoney($("#TotalApplication").val()) === 0) {
            $("#alert").UifAlert('show', Resources.TotalApplicationGreaterZero, "warning");
            return;
        }
        $("#modalConsignmentAdd").find("#alertForm").hide();
        $("#addConsignmentForm").formReset();
        $("#modalConsignmentAdd").find("#ConsignmentDate").val($("#ViewBagDateAccounting").val());
        LoadDefaultCurrency($('#modalConsignmentAdd').find('#selectConsignmentCurrency'));
        getNextTicketId($('#modalConsignmentAdd').find('#ConsignmentBallotNumber'));
        if (!isEmptyorZero(personName)) {
            $('#modalConsignmentAdd').find('#ConsignmentDepositorName').val(personName);
        }
        $('#modalConsignmentAdd').UifModal('showLocal', Resources.ConsignmentsEntry);
    }
});



$("#ConsignmentExchangeRate").on('blur', function (event, selectedItem) {
    ValidateExchangeRate('ConsignmentExchangeRate', "selectConsignmentCurrency", "ConsignmentAmount", "ConsignmentLocalAmount", "modalConsignmentAdd");
});

///////////////////////////////////////////////////////
/// Combo moneda - consignación                     ///
///////////////////////////////////////////////////////
$("#modalConsignmentAdd").find("#selectConsignmentCurrency").on('itemSelected', function (event, selectedItem) {
    $("#ConsignmentExchangeRate").removeAttr("disabled");
    loadBanks('modalConsignmentAdd', 'selectConsignmentReceivingBank', selectedItem.Id, 'selectConsignmentAccountNumber',
        $("#modalConsignmentAdd").find('#HiddenReceivingBankId').val());
    SetCurrencyMainBilling("selectConsignmentCurrency", "ConsignmentExchangeRate", "ConsignmentAmount", "ConsignmentLocalAmount", "modalConsignmentAdd");
    if ($("#modalConsignmentAdd").find("#ConsignmentAmount").val() != "") {
        SetConsignmentLocalAmount();
    }
});

///////////////////////////////////////////////////////
/// Importe - consignación                          ///
///////////////////////////////////////////////////////
$("#modalConsignmentAdd").find("#ConsignmentAmount").blur(function () {

    if ($("#modalConsignmentAdd").find("#ConsignmentAmount").val() != "") {

        var consignmentAmount = RemoveFormatMoney($("#modalConsignmentAdd").find("#ConsignmentAmount").val());

        if (consignmentAmount > 0) {
            $("#modalConsignmentAdd").find("#ConsignmentAmount").val(FormatMoneySymbol(consignmentAmount));
            SetConsignmentLocalAmount();
        } else {
            $("#modalConsignmentAdd").find("#ConsignmentAmount").val(FormatMoneySymbol(""));
            $("#addConsignmentForm").valid();
        }
    }

});

/////////////////////////////////////////////////////////////
// Número de boleta - consignación                         //
/////////////////////////////////////////////////////////////
$("#modalConsignmentAdd").find("#ConsignmentBallotNumber").blur(function () {
    if ($("#modalConsignmentAdd").find("#ConsignmentBallotNumber").val() != "") {
        // Se valida que no se ingrese el mismo número de boleta de depósito
        var exist = ValidDepositVoucherDocumentNumber();
        if (exist == 1) {
            $("#modalConsignmentAdd").find("#alertForm").UifAlert('show', Resources.WarningDepositVoucherValid, "warning");
            $("#modalConsignmentAdd").find("#ConsignmentBallotNumber").val("");

            setTimeout(function () {
                $("#modalConsignmentAdd").find("#alertForm").hide();
            }, 3000);
        }
    }
});

/////////////////////////////////////////
// Combo banco receptor - consignación //
/////////////////////////////////////////
$("#modalConsignmentAdd").find("#selectConsignmentReceivingBank").on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        loadAccountingBanks('modalConsignmentAdd', 'selectConsignmentAccountNumber', parseInt($("#addConsignmentForm").find('#selectConsignmentCurrency').val()),
            selectedItem.Id, $("#modalConsignmentAdd").find('#HiddenAccountNumber').val());
    }
});

/////////////////////////////////////
// ListView - consignación         //
/////////////////////////////////////
$("#modalConsignmentAdd").find('#consignmentsListView').on('rowEdit', function (event, data, index) {
    $("#modalConsignmentAdd").find("#HiddenReceivingBankName").val(data.RecievingBankName);
    $("#modalConsignmentAdd").find("#HiddenReceivingBankId").val(data.RecievingBankId);
    $("#modalConsignmentAdd").find("#HiddenAccountNumber").val(data.SerialVoucher);
    $("#modalConsignmentAdd").find("#HiddenBallotNumber").val(data.DocumentNumber);

    $("#modalConsignmentAdd").find("#ConsignmentAmount").val(data.Amount);
    $("#modalConsignmentAdd").find("#ConsignmentExchangeRate").val(data.ExchangeRate);
    $("#modalConsignmentAdd").find("#ConsignmentLocalAmount").val(data.LocalAmount);
    $("#modalConsignmentAdd").find("#selectConsignmentCurrency").UifSelect("setSelected", data.CurrencyId);
    $("#modalConsignmentAdd").find("#selectConsignmentCurrency").trigger("change");

    $("#modalConsignmentAdd").find("#selectConsignmentReceivingBank").val(data.RecievingBankId);
    $("#modalConsignmentAdd").find("#selectConsignmentAccountNumber").val(data.SerialVoucher);
    $("#modalConsignmentAdd").find("#ConsignmentBallotNumber").val(data.DocumentNumber);
    $("#modalConsignmentAdd").find("#ConsignmentDate").val(data.Date);
    $("#modalConsignmentAdd").find("#ConsignmentDepositorName").val(data.IssuerName);
    editConsignment = index;
});

$("#modalConsignmentAdd").find('#consignmentsListView').on('rowDelete', function (event, data) {

});

//////////////////////////
// Botón agregar modal ///
//////////////////////////
$("#modalConsignmentAdd").find('#ConsignmentAdd').click(function () {
    ValidateExchangeRate('ConsignmentExchangeRate', "selectConsignmentCurrency", "ConsignmentAmount", "ConsignmentLocalAmount", "modalConsignmentAdd", false);
    ConsignamentAdd();
});

function ConsignamentAdd() {
    $("#modalConsignmentAdd").find("#alertForm").hide();
    var existConsignment = 0;
    paymentMethod = "B";

    $("#addConsignmentForm").validate();
    var lastAmount = $("#ConsignmentAmount").val();
    previousValidAmount('ConsignmentAmount');

    if ($("#addConsignmentForm").valid()) {

        $("#ConsignmentAmount").val(lastAmount);
        var nameIsBlankChar = isBlankChar($.trim($("#ConsignmentDepositorName").val()));

        if (ValidateAddFormMain(paymentMethod) == true && nameIsBlankChar) {
            // Se valida que no se ingrese el mismo registro
            var keyValid = $("#addConsignmentForm").find("#selectConsignmentReceivingBank option:selected").text() +
                $("#addConsignmentForm").find("#ConsignmentBallotNumber").val() +
                $("#addConsignmentForm").find("#selectConsignmentAccountNumber option:selected").text();

            var consignments = $("#modalConsignmentAdd").find('#consignmentsListView').UifListView("getData");

            if (consignments != null) {
                for (var j = 0; j < consignments.length; j++) {
                    if (editConsignment != j) {
                        var consignmentIndex = consignments[j].RecievingBankName + consignments[j].DocumentNumber +
                            consignments[j].RecievingBankAccountNumber;
                        if (consignmentIndex == keyValid) {
                            existConsignment = 1;
                            break;
                        }
                    }
                }
            }

            if (editConsignment > -1) {
                if (($("#addConsignmentForm").find("#selectConsignmentReceivingBank option:selected").text() == $("#addConsignmentForm").find("#HiddenReceivingBankName").val()) &&
                    ($("#addConsignmentForm").find("#ConsignmentBallotNumber").val() == $("#addConsignmentForm").find("#HiddenBallotNumber").val()) &&
                    ($("#addConsignmentForm").find("#selectConsignmentAccountNumber option:selected").text() == $("#addConsignmentForm").find("#HiddenAccountNumber").val())) {
                    existConsignment = 0;
                }
            }

            if (existConsignment == 0) {
                var rowModel = new RowModel();

                rowModel.PaymentTypeId = $("#ViewBagParamPaymentMethodDepositVoucher").val();
                rowModel.PaymentTypeDescription = "CONSIGNACIÓN";
                rowModel.CurrencyId = $("#addConsignmentForm").find('#selectConsignmentCurrency').val();
                rowModel.Currency = $("#addConsignmentForm").find('#selectConsignmentCurrency option:selected').text();
                rowModel.Amount = $("#addConsignmentForm").find('#ConsignmentAmount').val();
                rowModel.ExchangeRate = $("#addConsignmentForm").find("#ConsignmentExchangeRate").val();
                rowModel.LocalAmount = $("#addConsignmentForm").find("#ConsignmentLocalAmount").val();
                rowModel.DocumentNumber = $("#addConsignmentForm").find("#ConsignmentBallotNumber").val();
                rowModel.VoucherNumber = "";
                rowModel.CardNumber = "";
                rowModel.CardType = "";
                rowModel.CardTypeName = "";
                rowModel.AuthorizationNumber = "";
                rowModel.CardsName = "";
                rowModel.ValidThruMonth = "";
                rowModel.ValidThruYear = "";
                rowModel.IssuingBankId = "";
                rowModel.IssuingBankName = "";
                rowModel.IssuingBankAccountNumber = "";
                rowModel.IssuerName = $("#addConsignmentForm").find("#ConsignmentDepositorName").val();
                rowModel.RecievingBankId = $("#addConsignmentForm").find("#selectConsignmentReceivingBank").val();
                rowModel.RecievingBankName = $("#addConsignmentForm").find("#selectConsignmentReceivingBank option:selected").text();
                rowModel.RecievingBankAccountNumber = $("#addConsignmentForm").find("#selectConsignmentAccountNumber option:selected").text();
                rowModel.SerialVoucher = $("#addConsignmentForm").find("#selectConsignmentAccountNumber").val(); // id account number
                rowModel.SerialNumber = "";
                rowModel.Date = $("#addConsignmentForm").find("#ConsignmentDate").val();
                rowModel.TaxBase = "";
                rowModel.Information = "";

                $("#modalConsignmentAdd").find('#TotalConsignment').text(rowModel.Amount);
                if (editConsignment == -1) {
                    $("#modalConsignmentAdd").find('#consignmentsListView').UifListView("addItem", rowModel);
                }
                else {
                    $("#modalConsignmentAdd").find('#consignmentsListView').UifListView("editItem", editConsignment, rowModel);
                    editConsignment = -1;
                }
                $("#addConsignmentForm").formReset();
                $("#ConsignmentExchangeRate").val("");
                $("#modalConsignmentAdd").find("#HiddenReceivingBankId").val('');
                $("#modalConsignmentAdd").find("#HiddenReceivingBankName").val('');
                $("#modalConsignmentAdd").find("#HiddenAccountNumber").val('');
                $("#modalConsignmentAdd").find("#HiddenBallotNumber").val('');
                $("#ConsignmentExchangeRate").attr('disabled', 'disabled');
                newRate = 0;
                currentRate = 0;
                LoadDefaultCurrency($('#modalConsignmentAdd').find('#selectConsignmentCurrency'));
                getNextTicketId($('#modalConsignmentAdd').find('#ConsignmentBallotNumber'));
                if (!isEmptyorZero(personName)) {
                    $('#modalConsignmentAdd').find('#ConsignmentDepositorName').val(personName);
                }
                $("#modalConsignmentAdd").find("#ConsignmentDate").val($("#ViewBagDateAccounting").val());
                SetTotalPayment("B");
                SetConsignmentSummary();
            }
            else {
                $("#modalConsignmentAdd").find("#alertForm").UifAlert('show', Resources.ValidateBallotNumber, "warning");
            }
        }
        else {

            if (!nameIsBlankChar) {
                $("#modalConsignmentAdd").find("#alertForm").UifAlert('show', Resources.WarningInvalidLogon + " (" + Resources.DepositorName + ")", "warning");
            }
        }

        SetTotalApplicationPayment();
    }
    else {
        $("#ConsignmentAmount").val(lastAmount);
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                          CONSIGNACIONES  CHEQUES                                      ///
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
var saveConsignmentCheckCallback = function (deferred, data) {
    $("#modalConsignmentCheckAdd").find("#ConsignmentChecksListView").UifListView("addItem", data);
    deferred.resolve();
};

var editConsignmentCheckCallback = function (deferred, data) {
    data.NombreEmpresa = "Sistran";
    deferred.resolve(data);
};

var deleteConsignmentCheckCallback = function (deferred, data, index) {
    deferred.resolve();

    if (editConsignmentCheck === index) {
        editConsignmentCheck = -1;
    } else if (editConsignmentCheck > index) {
        editConsignmentCheck -= 1;
    }

    setTimeout(function () {
        SetTotalPayment("CH");
        SetTotalApplicationPayment();
        SetConsignmentCheckSummary();
    }, 1000);
};

//////////////////////////////////////
// Botón Consignación de cheques    //
//////////////////////////////////////
$('#OpenConsignmentCheck').click(function () {
    paymentMethod = "B";
    editConsignmentCheck = -1;
    currentRate = 0;
    if ($("#TotalApplication").val() == "") {
        $("#alert").UifAlert('show', Resources.EntryTotalApplication, "warning");
        return;
    }
    else {
        if (RemoveFormatMoney($("#TotalApplication").val()) === 0) {
            $("#alert").UifAlert('show', Resources.TotalApplicationGreaterZero, "warning");
            return;
        }
        $("#modalConsignmentCheckAdd").find("#alertForm").hide();
        $("#addConsignmentCheckForm").formReset();
        $("#modalConsignmentCheckAdd").find("#CheckDate").val($("#ViewBagDateAccounting").val());
        $("#modalConsignmentCheckAdd").find("#ConsignmentCheckDate").val($("#ViewBagDateAccounting").val());
        LoadDefaultCurrency($('#modalConsignmentCheckAdd').find('#selectConsignmentCheckCurrency'));
        if (!isEmptyorZero(personName)) {
            $('#modalConsignmentCheckAdd').find('#ConsignmentCheckDepositorName').val(personName);
            $('#modalConsignmentCheckAdd').find('#ConsignmentCheckHolderName').val(personName);
        }
        $('#modalConsignmentCheckAdd').UifModal('showLocal', Resources.ConsignmentsEntry);
    }
});

////////////////////////////////////////
// Autocomplete banco emisor - cheque //
////////////////////////////////////////
$("#modalConsignmentCheckAdd").find('#inputConsignmentCheckIssuingBank').on('itemSelected', function (event, selectedItem) {
    $('#inputConsignmentCheckIssuingBank').UifAutoComplete('setValue', selectedItem.Value);
    $('#HiddenConsignmentIssuingBankId').val(selectedItem.Id);
    issuingBankId = selectedItem.Id;
});

$("#ConsignmentCheckExchangeRate").on('blur', function (event, selectedItem) {
    ValidateExchangeRate("ConsignmentCheckExchangeRate", "selectConsignmentCheckCurrency", "ConsignmentCheckAmount", "ConsignmentCheckLocalAmount", "modalConsignmentCheckAdd");
});

///////////////////////////////////////////////////////
/// Combo moneda - consignación                     ///
///////////////////////////////////////////////////////
$("#modalConsignmentCheckAdd").find("#selectConsignmentCheckCurrency").on('itemSelected', function (event, selectedItem) {
    $("#ConsignmentCheckExchangeRate").removeAttr("disabled");
    loadBanks('modalConsignmentCheckAdd', 'selectConsignmentCheckReceivingBank', selectedItem.Id, 'selectConsignmentCheckAccountNumber',
        $("#modalConsignmentCheckAdd").find('#HiddenReceivingBankId').val());
    SetCurrencyMainBilling("selectConsignmentCheckCurrency", "ConsignmentCheckExchangeRate", "ConsignmentCheckAmount", "ConsignmentCheckLocalAmount", "modalConsignmentCheckAdd");
    if ($("#modalConsignmentCheckAdd").find("#ConsignmentCheckAmount").val() != "") {

        SetConsignmentCheckLocalAmount();
    }
});

///////////////////////////////////////////////////////
/// Importe - consignación                          ///
///////////////////////////////////////////////////////
$("#modalConsignmentCheckAdd").find("#ConsignmentCheckAmount").blur(function () {

    if ($("#modalConsignmentCheckAdd").find("#ConsignmentCheckAmount").val() != "") {

        var ConsignmentCheckAmount = RemoveFormatMoney($("#modalConsignmentCheckAdd").find("#ConsignmentCheckAmount").val());

        if (ConsignmentCheckAmount > 0) {
            $("#modalConsignmentCheckAdd").find("#ConsignmentCheckAmount").val(FormatMoneySymbol(ConsignmentCheckAmount));
            SetConsignmentCheckLocalAmount();
        } else {
            $("#modalConsignmentCheckAdd").find("#ConsignmentCheckAmount").val("");
            $("#addConsignmentCheckForm").valid();
        }
    }

});

/////////////////////////////////////////////////////////////
// Número de boleta - consignación                         //
/////////////////////////////////////////////////////////////
$("#modalConsignmentCheckAdd").find("#ConsignmentCheckDocumentNumber").blur(function () {
    if ($("#modalConsignmentCheckAdd").find("#ConsignmentCheckDocumentNumber").val() != "") {
        // Se valida que no se ingrese el mismo número de boleta de depósito
        ValidDepositCheckVoucherDocumentNumber().done(function (data) {
            if (data > 0) {
                $("#modalConsignmentCheckAdd").find("#alertForm").UifAlert('show', Resources.WarningDepositVoucherValid, "warning");
                $("#modalConsignmentCheckAdd").find("#ConsignmentCheckDocumentNumber").val("");
                $("#modalConsignmentCheckAdd").find("#alertForm").hide();
            }
        });
    }
});

/////////////////////////////////////////
// Combo banco receptor - consignación //
/////////////////////////////////////////
$("#modalConsignmentCheckAdd").find("#selectConsignmentCheckReceivingBank").on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        loadAccountingBanks('modalConsignmentCheckAdd', 'selectConsignmentCheckAccountNumber',
            parseInt($("#modalConsignmentCheckAdd").find('#selectConsignmentCheckCurrency').val()), selectedItem.Id,
            $("#modalConsignmentCheckAdd").find('#HiddenAccountNumber').val());
    }
});

/////////////////////////////////////
// ListView - consignación         //
/////////////////////////////////////
$("#modalConsignmentCheckAdd").find('#ConsignmentChecksListView').on('rowEdit', function (event, data, index) {
    $("#modalConsignmentCheckAdd").find("#HiddenReceivingBankName").val(data.RecievingBankName);
    $("#modalConsignmentCheckAdd").find("#HiddenReceivingBankId").val(data.RecievingBankId);
    $("#modalConsignmentCheckAdd").find("#HiddenAccountNumber").val(data.SerialVoucher);
    $("#modalConsignmentCheckAdd").find("#HiddenBallotNumber").val(data.DocumentNumber);

    $("#modalConsignmentCheckAdd").find("#ConsignmentCheckAmount").val(data.Amount);
    $("#modalConsignmentCheckAdd").find("#ConsignmentCheckExchangeRate").val(data.ExchangeRate);
    $("#modalConsignmentCheckAdd").find("#ConsignmentCheckLocalAmount").val(data.LocalAmount);
    $("#modalConsignmentCheckAdd").find("#selectConsignmentCheckCurrency").UifSelect("setSelected", data.CurrencyId);
    $("#modalConsignmentCheckAdd").find("#selectConsignmentCheckCurrency").trigger("change");

    $("#modalConsignmentCheckAdd").find("#selectConsignmentCheckReceivingBank").UifSelect('setSelected', data.RecievingBankId);
    $("#modalConsignmentCheckAdd").find("#selectConsignmentCheckAccountNumber").val(data.SerialVoucher);
    $("#modalConsignmentCheckAdd").find("#ConsignmentCheckDate").val(data.Date);
    $("#modalConsignmentCheckAdd").find("#CheckDate").val(data.DateCheck);
    $("#modalConsignmentCheckAdd").find("#ConsignmentCheckDepositorName").val(data.IssuerName);

    $("#modalConsignmentCheckAdd").find('#inputConsignmentCheckIssuingBank').UifAutoComplete('setValue', data.IssuingBankCheckName);
    $("#modalConsignmentCheckAdd").find("#ConsignmentCheckAccountNumber").val(data.IssuingBankCheckAccountNumber);
    $("#modalConsignmentCheckAdd").find("#ConsignmentCheckDocumentNumber").val(data.CheckDocumentNumber);
    $("#modalConsignmentCheckAdd").find("#ConsignmentCheckHolderName").val(data.IssuerCheckName);


    editConsignmentCheck = index;
});

$("#modalConsignmentCheckAdd").find('#ConsignmentChecksListView').on('rowDelete', function (event, data) {

});

//////////////////////////
// Botón agregar modal ///
//////////////////////////
$("#modalConsignmentCheckAdd").find('#ConsignmentCheckAdd').click(function () {
    $("#modalConsignmentCheckAdd").find("#alertForm").hide();
    var existConsignmentCheck = 0;
    paymentMethod = "CH";

    $("#addConsignmentCheckForm").validate();
    var lastAmount = $("#ConsignmentCheckAmount").val();
    previousValidAmount('ConsignmentCheckAmount');

    if ($("#addConsignmentCheckForm").valid()) {

        $("#ConsignmentCheckAmount").val(lastAmount);
        var nameIsBlankChar = isBlankChar($.trim($("#ConsignmentCheckDepositorName").val()));

        if (ValidateAddFormMain(paymentMethod) == true && nameIsBlankChar) {
            // Se valida que no se ingrese el mismo registro
            var keyValid = $("#addConsignmentCheckForm").find("#selectConsignmentCheckReceivingBank option:selected").text() +
                $("#addConsignmentCheckForm").find("#ConsignmentCheckDocumentNumber").val() +
                $("#addConsignmentCheckForm").find("#selectConsignmentCheckAccountNumber option:selected").text();

            var ConsignmentChecks = $("#modalConsignmentCheckAdd").find('#ConsignmentChecksListView').UifListView("getData");

            if (ConsignmentChecks != null) {
                for (var j = 0; j < ConsignmentChecks.length; j++) {
                    if (editConsignmentCheck != j) {
                        var ConsignmentCheckIndex = ConsignmentChecks[j].RecievingBankName + ConsignmentChecks[j].CheckDocumentNumber +
                            ConsignmentChecks[j].RecievingBankAccountNumber;
                        if (ConsignmentCheckIndex == keyValid) {
                            existConsignmentCheck = 1;
                            break;
                        }
                    }
                }
            }

            if (editConsignmentCheck > -1) {
                if (($("#addConsignmentCheckForm").find("#selectConsignmentCheckReceivingBank option:selected").text() == $("#addConsignmentCheckForm").find("#HiddenReceivingBankName").val()) &&
                    ($("#addConsignmentCheckForm").find("#ConsignmentCheckDocumentNumber").val() == $("#addConsignmentCheckForm").find("#HiddenConsignmentCheckDocumentNumber").val()) &&
                    ($("#addConsignmentCheckForm").find("#selectConsignmentCheckAccountNumber option:selected").text() == $("#addConsignmentCheckForm").find("#HiddenAccountNumber").val())) {
                    existConsignmentCheck = 0;
                }
            }

            if (existConsignmentCheck == 0) {
                var rowModel = new RowModel();

                rowModel.PaymentTypeId = $("#ViewBagParamPaymentConsignmentCheck").val();
                rowModel.PaymentTypeDescription = "CONSIGNACIÓN DE CHEQUES";
                rowModel.CurrencyId = $("#addConsignmentCheckForm").find('#selectConsignmentCheckCurrency').val();
                rowModel.Currency = $("#addConsignmentCheckForm").find('#selectConsignmentCheckCurrency option:selected').text();
                rowModel.Amount = $("#addConsignmentCheckForm").find('#ConsignmentCheckAmount').val();
                rowModel.ExchangeRate = $("#addConsignmentCheckForm").find("#ConsignmentCheckExchangeRate").val();
                rowModel.LocalAmount = $("#addConsignmentCheckForm").find("#ConsignmentCheckLocalAmount").val();
                rowModel.DocumentNumber = $("#addConsignmentCheckForm").find("#ConsignmentCheckBallotNumber").val();
                rowModel.VoucherNumber = "";
                rowModel.CardNumber = "";
                rowModel.CardType = "";
                rowModel.CardTypeName = "";
                rowModel.AuthorizationNumber = "";
                rowModel.CardsName = "";
                rowModel.ValidThruMonth = "";
                rowModel.ValidThruYear = "";
                rowModel.IssuingBankName = "";
                rowModel.IssuingBankAccountNumber = $("#addConsignmentCheckForm").find('#selectConsignmentCheckAccountNumber').UifSelect('getSelectedText');
                rowModel.IssuerName = $("#addConsignmentCheckForm").find("#ConsignmentCheckDepositorName").val();
                rowModel.RecievingBankId = $("#addConsignmentCheckForm").find("#selectConsignmentCheckReceivingBank").val();
                rowModel.RecievingBankName = $("#addConsignmentCheckForm").find("#selectConsignmentCheckReceivingBank option:selected").text();
                rowModel.RecievingBankAccountNumber = $("#addConsignmentCheckForm").find("#selectConsignmentCheckAccountNumber option:selected").text();
                rowModel.RecievingBankAccountId = $("#addConsignmentCheckForm").find('#selectConsignmentCheckAccountNumber').UifSelect('getSelected');
                rowModel.SerialVoucher = $("#addConsignmentCheckForm").find("#selectConsignmentCheckAccountNumber").val(); // id account number
                rowModel.SerialNumber = "";
                rowModel.Date = $("#addConsignmentCheckForm").find("#ConsignmentCheckDate").val();
                rowModel.TaxBase = "";
                rowModel.Information = "";
                //CHEQUES ROWMODEL
                rowModel.CheckDocumentNumber = $("#addConsignmentCheckForm").find("#ConsignmentCheckDocumentNumber").val();
                rowModel.IssuingBankId = issuingBankId;
                rowModel.IssuingBankCheckName = $("#addConsignmentCheckForm").find("#inputConsignmentCheckIssuingBank").val();
                rowModel.IssuingBankCheckAccountNumber = $("#addConsignmentCheckForm").find('#selectConsignmentCheckAccountNumber').UifSelect('getSelectedText');
                rowModel.IssuerCheckName = $("#addConsignmentCheckForm").find("#ConsignmentCheckHolderName").val();
                rowModel.DateCheck = $("#addConsignmentCheckForm").find("#CheckDate").val();

                $("#modalConsignmentCheckAdd").find('#TotalConsignmentCheck').text(rowModel.Amount);
                if (editConsignmentCheck == -1) {
                    $("#modalConsignmentCheckAdd").find('#ConsignmentChecksListView').UifListView("addItem", rowModel);
                }
                else {
                    $("#modalConsignmentCheckAdd").find('#ConsignmentChecksListView').UifListView("editItem", editConsignmentCheck, rowModel);
                    editConsignmentCheck = -1;
                }

                $("#addConsignmentCheckForm").formReset();
                LoadDefaultCurrency($('#modalConsignmentCheckAdd').find('#selectConsignmentCheckCurrency'));
                if (!isEmptyorZero(personName)) {
                    $('#modalConsignmentCheckAdd').find('#ConsignmentCheckDepositorName').val(personName);
                    $('#modalConsignmentCheckAdd').find('#ConsignmentCheckHolderName').val(personName);
                }
                $("#modalConsignmentCheckAdd").find("#CheckDate").val($("#ViewBagDateAccounting").val());
                $("#modalConsignmentCheckAdd").find("#ConsignmentCheckDate").val($("#ViewBagDateAccounting").val());
                $("#addConsignmentCheckForm").find("#selectConsignmentCheckReceivingBank").UifSelect('setSelected', '');
                $("#modalConsignmentCheckAdd").find("#HiddenReceivingBankId").val('');
                $("#modalConsignmentCheckAdd").find("#HiddenReceivingBankName").val('');
                $("#modalConsignmentCheckAdd").find("#HiddenAccountNumber").val('');
                $("#modalConsignmentCheckAdd").find("#HiddenBallotNumber").val('');

                SetTotalPayment("CH");
                SetConsignmentCheckSummary();
            }
            else {
                $("#modalConsignmentCheckAdd").find("#alertForm").UifAlert('show', Resources.ValidateBallotNumber, "warning");
            }
        }
        else {

            if (!nameIsBlankChar) {
                $("#modalConsignmentCheckAdd").find("#alertForm").UifAlert('show', Resources.WarningInvalidLogon + " (" + Resources.DepositorName + ")", "warning");
            }
        }

        SetTotalApplicationPayment();
    }
    else {
        $("#ConsignmentCheckAmount").val(lastAmount);
    }
});



/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                            DATAFONO                                                   ///
/////////////////////////////////////////////////////////////////////////////////////////////////////////////

var saveDatafonoCallback = function (deferred, data) {
    $("#modalDatafonoAdd").find("#datafonoListView").UifListView("addItem", data);
    deferred.resolve();
};

var editDatafonoCallback = function (deferred, data) {
    data.NombreEmpresa = "Sistran";
    deferred.resolve(data);
};

var deleteDatafonoCallback = function (deferred, data) {
    deferred.resolve();
    setTimeout(function () {
        SetTotalPayment("D");
        SetTotalApplicationPayment();
        SetMovementsSummary("D");
        editDatafono = -1;
    }, 1000);
};

$("#modalDatafonoAdd").find("#datafonoListView").UifListView({
    autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
    delete: true, displayTemplate: "#datafono-display-template", editCallback: editDatafonoCallback, deleteCallback: deleteDatafonoCallback
});

//////////////////////////////////////
// Botón Datafono                   //
//////////////////////////////////////
$('#OpenDatafono').click(function () {
    paymentMethod = "D";
    editDatafono = -1;
    currentRate = 0;
    if ($("#TotalApplication").val() == "") {
        $("#alert").UifAlert('show', Resources.EntryTotalApplication, "warning");
        return;
    }
    else {
        if (RemoveFormatMoney($("#TotalApplication").val()) === 0) {
            $("#alert").UifAlert('show', Resources.TotalApplicationGreaterZero, "warning");
            return;
        }
        $("#addDatafonoForm").formReset();
        $('#modalDatafonoAdd').UifModal('showLocal', Resources.IncomeDatafono);
    }
});

////////////////////////////////////////////////////
// Autocomplete banco emisor - Datafono          ///
////////////////////////////////////////////////////
$("#modalDatafonoAdd").find('#inputDatafonoIssuingBank').on('itemSelected', function (event, selectedItem) {
    issuingBankId = selectedItem.Id;
});

/////////////////////////////////////
// ListView - Datafono             //
/////////////////////////////////////
$("#modalDatafonoAdd").find('#datafonoListView').on('rowEdit', function (event, data, index) {
    $("#modalDatafonoAdd").find("#selectDatafonoCurrency").val(data.CurrencyId);
    $("#modalDatafonoAdd").find("#DatafonoAmount").val(data.Amount);
    $("#modalDatafonoAdd").find("#DatafonoExchangeRate").val(data.ExchangeRate);
    $("#modalDatafonoAdd").find("#DatafonoLocalAmount").val(data.LocalAmount);
    $("#modalDatafonoAdd").find("#selectDatafonoCreditCardType").val(data.CardType);
    issuingBankId = data.IssuingBankId;
    $("#modalDatafonoAdd").find("#inputDatafonoIssuingBank").val(data.IssuingBankName);
    $("#modalDatafonoAdd").find("#DatafonoCreditCardNumber").val(data.CardNumber);
    $("#modalDatafonoAdd").find("#HiddenDatafonoCreditCardNumber").val(data.CardNumber);
    $("#modalDatafonoAdd").find("#DatafonoAuthorizationNumber").val(data.AuthorizationNumber);
    $("#modalDatafonoAdd").find("#DatafonoVoucherNumber").val(data.VoucherNumber);
    $("#modalDatafonoAdd").find("#HiddenDatafonoVoucherNumber").val(data.VoucherNumber);
    $("#modalDatafonoAdd").find("#DatafonoTaxBase").val(data.TaxBase);
    $("#modalDatafonoAdd").find("#DatafonoHolderName").val(data.CardsName);
    $("#modalDatafonoAdd").find("#selectDatafonoCreditCardValidThruMonth").val(data.ValidThruMonth);
    $("#modalDatafonoAdd").find("#DatafonoCreditCardValidThruYear").val(data.ValidThruYear);
    editDatafono = index;
});

$("#DatafonoExchangeRate").on('blur', function (event, selectedItem) {
    ValidateExchangeRate("DatafonoExchangeRate", "selectDatafonoCurrency", "DatafonoAmount", "DatafonoLocalAmount", "modalDatafonoAdd");
});

///////////////////////////////////////////////////////
/// Combo moneda - datafono                         ///
///////////////////////////////////////////////////////
$("#modalDatafonoAdd").find("#selectDatafonoCurrency").on('itemSelected', function (event, selectedItem) {
    $("#DatafonoExchangeRate").removeAttr("disabled");
    SetCurrencyMainBilling("selectDatafonoCurrency", "DatafonoExchangeRate");
    if ($("#modalDatafonoAdd").find("#CashAmount").val() != "") {
        SetDatafonoLocalAmount();
    }
});

///////////////////////////////////////////////////////
/// Importe - datafono                              ///
///////////////////////////////////////////////////////
$("#modalDatafonoAdd").find("#DatafonoAmount").blur(function () {

    if ($("#modalDatafonoAdd").find("#DatafonoAmount").val() != "") {
        var fonoAmount = RemoveFormatMoney($("#modalDatafonoAdd").find("#DatafonoAmount").val());
        $("#modalDatafonoAdd").find("#DatafonoAmount").val(FormatMoneySymbol(fonoAmount));
        SetDatafonoLocalAmount();
    }
});

///////////////////////////////////////////////////////
/// Base impuesto - Datafono                        ///
///////////////////////////////////////////////////////
$("#modalDatafonoAdd").find("#DatafonoTaxBase").blur(function () {
    if ($("#modalDatafonoAdd").find("#DatafonoTaxBase").val() != "") {
        // Se valida que la base impuesto no sea mayor al importe
        var datafonoTaxBase = RemoveFormatMoney($("#modalDatafonoAdd").find("#DatafonoTaxBase"));
        var datafonoAmount = RemoveFormatMoney($("#modalDatafonoAdd").find("#DatafonoAmount").val());

        if (datafonoTaxBase > datafonoAmount) {
            $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.WarningGreaterTaxBase, "warning");
            $("#modalDatafonoAdd").find("#DatafonoTaxBase").val("");

            setTimeout(function () {
                $("#modalDatafonoAdd").find("#alertForm").hide();
            }, 3000);
        }
        else {
            $("#modalDatafonoAdd").find("#DatafonoTaxBase").val(FormatMoneySymbol(datafonoTaxBase));
        }
    }
});

/////////////////////////////////////////////////////////////
// Número voucher - Datafono                               //
/////////////////////////////////////////////////////////////
$("#modalDatafonoAdd").find("#DatafonoVoucherNumber").blur(function () {
    $("#modalDatafonoAdd").find("#alertForm").hide();
    if ($("#modalDatafonoAdd").find("#DatafonoVoucherNumber").val() != "") {
        // Se valida que no se ingrese el mismo número de voucher
        if (ValidDatafonoVoucherNumber() == false) {

            $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.WarningVoucherNumberValid, "warning");
            $("#modalDatafonoAdd").find("#DatafonoVoucherNumber").val("");
        }
    }
});

/////////////////////////////////////////////////////////////
// Año expiración - Datafono                               //
/////////////////////////////////////////////////////////////
$("#modalDatafonoAdd").find("#DatafonoCreditCardValidThruYear").blur(function () {
    if ($("#modalDatafonoAdd").find("#DatafonoCreditCardValidThruYear").val() != "") {
        CheckExpirationDateDatafono();
    }
});

/////////////////////////////////////////////////////////////
// mes expiración - Datafono                               //
/////////////////////////////////////////////////////////////
$("#modalDatafonoAdd").find("#selectDatafonoCreditCardValidThruMonth").on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        CheckExpirationDateDatafono();
    }
});

//////////////////////////
// Botón agregar modal ///
//////////////////////////
$("#modalDatafonoAdd").find('#DatafonoAdd').click(function () {
    ValidateExchangeRate("DatafonoExchangeRate", "selectDatafonoCurrency", "DatafonoAmount", "DatafonoLocalAmount", "modalDatafonoAdd", false);
    DatafonoAdd();
});
function DatafonoAdd() {
    $("#modalDatafonoAdd").find("#alertForm").hide();
    paymentMethod = "D";
    var existDatafono = 0;

    $("#addDatafonoForm").validate();
    var lastAmount = $("#DatafonoAmount").val();
    previousValidAmount('DatafonoAmount');

    if ($("#addDatafonoForm").valid()) {
        $("#DatafonoAmount").val(lastAmount);
        var nameIsBlankChar = isBlankChar($.trim($("#DatafonoHolderName").val()));
        $("#modalDatafonoAdd").find("#alertForm").hide();
        if (ValidateAddFormMain(paymentMethod) == true && CheckExpirationDateDatafono() && nameIsBlankChar) {
            // Se valida que no se ingrese el mismo registro
            var keyValid = $("#addDatafonoForm").find("#DatafonoCreditCardNumber").val() +
                $("#addDatafonoForm").find("#DatafonoVoucherNumber").val();

            var datafono = $("#modalDatafonoAdd").find('#datafonoListView').UifListView("getData");

            if (datafono != null) {
                for (var j = 0; j < datafono.length; j++) {
                    if (editDatafono != j) {
                        var datafonoIndex = datafono[j].CardNumber + datafono[j].VoucherNumber;
                        if (datafonoIndex == keyValid) {
                            existDatafono = 1;
                            break;
                        }
                    }
                }
            }

            if (editDatafono > -1) {
                if (($("#addDatafonoForm").find("#DatafonoCreditCardNumber").val() == $("#addDatafonoForm").find("#HiddenDatafonoCreditCardNumber").val()) &&
                    ($("#addDatafonoForm").find("#DatafonoVoucherNumber").val() == $("#addDatafonoForm").find("#HiddenDatafonoVoucherNumber").val())) {
                    existDatafono = 0;
                }
            }

            if (existDatafono == 0) {
                var rowModel = new RowPaymentModel();

                rowModel.PaymentTypeId = $("#ViewBagParamPaymentMethodDataphone").val();
                rowModel.PaymentTypeDescription = "DATAFONO";
                rowModel.CurrencyId = $("#addDatafonoForm").find('#selectDatafonoCurrency').val();
                rowModel.Currency = $("#addDatafonoForm").find('#selectDatafonoCurrency option:selected').text();
                rowModel.Amount = $("#addDatafonoForm").find('#DatafonoAmount').val();
                rowModel.ExchangeRate = $("#addDatafonoForm").find("#DatafonoExchangeRate").val();
                rowModel.LocalAmount = $("#addDatafonoForm").find("#DatafonoLocalAmount").val();
                rowModel.DocumentNumber = "";
                rowModel.VoucherNumber = $("#addDatafonoForm").find("#DatafonoVoucherNumber").val();
                rowModel.CardNumber = $("#addDatafonoForm").find("#DatafonoCreditCardNumber").val();
                rowModel.CardType = $("#addDatafonoForm").find('#selectDatafonoCreditCardType').val();
                rowModel.CardTypeName = $("#addDatafonoForm").find('#selectDatafonoCreditCardType option:selected').text();
                rowModel.AuthorizationNumber = $("#addDatafonoForm").find("#DatafonoAuthorizationNumber").val();
                rowModel.CardsName = $("#addDatafonoForm").find("#DatafonoHolderName").val();
                rowModel.ValidThruMonth = $("#addDatafonoForm").find('#selectDatafonoCreditCardValidThruMonth').val();
                rowModel.ValidThruYear = $("#addDatafonoForm").find('#DatafonoCreditCardValidThruYear').val();
                rowModel.IssuingBankId = issuingBankId;
                rowModel.IssuingBankName = $("#addDatafonoForm").find("#inputDatafonoIssuingBank").val();
                rowModel.IssuingBankAccountNumber = "";
                rowModel.IssuerName = "";
                rowModel.RecievingBankId = "";
                rowModel.RecievingBankName = "";
                rowModel.RecievingBankAccountNumber = "";
                rowModel.SerialVoucher = "";
                rowModel.SerialNumber = "";
                rowModel.Date = "";
                rowModel.TaxBase = $("#addDatafonoForm").find("#DatafonoTaxBase").val();
                rowModel.Information = "";

                $("#modalDatafonoAdd").find('#TotalDatafono').text(rowModel.Amount);

                if (editDatafono == -1) {
                    $("#modalDatafonoAdd").find('#datafonoListView').UifListView("addItem", rowModel);
                }
                else {
                    $("#modalDatafonoAdd").find('#datafonoListView').UifListView("editItem", editDatafono, rowModel);
                    editDatafono = -1;
                }

                $("#addDatafonoForm").formReset();
                $("#DatafonoExchangeRate").val("");
                $("#DatafonoExchangeRate").attr('disabled', 'disabled');
                newRate = 0;
                currentRate = 0;
                SetTotalPayment("D");
                SetDatafonoSummary();
            }
            else {
                $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.ValidateVoucherNumber, "warning");
            }
        } else {
            if (!nameIsBlankChar) {
                $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.btnSig_In + " " + Resources.PayCreditCardName, "warning");
            }
        }
        SetTotalApplicationPayment();
    }
    else {
        $("#DatafonoAmount").val(lastAmount);
    }
}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                            NUEVO                                                      ///
/////////////////////////////////////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////
// Botón Nuevo                      //
//////////////////////////////////////
//$('#New').click(function () {
//    paymentMethod = "";
//    ClearFieldsMain();
//    setFocusControl('inputInsuredDocumentNumber');
//});

//$("#Neww").click(function (e) { showConfirm(); });

//var showConfirm = function () {
//    $.UifDialog('confirm', { 'message': 'Esta seguro de grabar el recibo?', 'title': 'Caja ingreso' }, function (result) {
//        if (result) {
//            ClearFieldsMain();
//        }

//    });
//};

/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                            TARJETA                                                    ///
/////////////////////////////////////////////////////////////////////////////////////////////////////////////

var saveCreditCardCallback = function (deferred, data) {
    $("#modalCardAdd").find("#cardsListView").UifListView("addItem", data);
    deferred.resolve();
};

var editCreditCardCallback = function (deferred, data) {
    data.NombreEmpresa = "Sistran";
    deferred.resolve(data);
};

var deleteCreditCardCallback = function (deferred, data, index) {
    if (editCreditCard === index) {
        editCreditCard = -1;
    } else if (editCreditCard > index) {
        editCreditCard -= 1;
    }

    deferred.resolve();
    setTimeout(function () {
        SetTotalPayment("T");
        SetTotalApplicationPayment();
        SetCardSummary();
    }, 1000);
};

//////////////////////////////////////
// Botón Tarjeta                    //
//////////////////////////////////////
$('#OpenCard').click(function () {
    paymentMethod = "T";
    editCreditCard = -1;
    currentRate = 0;
    if ($("#TotalApplication").val() == "") {
        $("#alert").UifAlert('show', Resources.EntryTotalApplication, "warning");
        return
    }
    else {
        if (RemoveFormatMoney($("#TotalApplication").val()) === 0) {
            $("#alert").UifAlert('show', Resources.TotalApplicationGreaterZero, "warning");
            return;
        }
        $("#modalCardAdd").find("#alertForm").hide();
        $("#addCardForm").formReset();
        $('#modalCardAdd').UifModal('showLocal', Resources.CardsEntry);
    }
});

$("#CreditCardExchangeRate").on('blur', function (event, selectedItem) {
    ValidateExchangeRate("CreditCardExchangeRate", "selectCreditCardCurrency", "CreditCardAmount", "CreditCardLocalAmount", "modalCardAdd");
});

///////////////////////////////////////////////////////
/// Combo moneda - tarjeta de crédito               ///
///////////////////////////////////////////////////////
$("#modalCardAdd").find("#selectCreditCardCurrency").on('itemSelected', function (event, selectedItem) {
    $("#CreditCardExchangeRate").removeAttr("disabled");
    SetCurrencyMainBilling("selectCreditCardCurrency", "CreditCardExchangeRate", "CreditCardAmount", "CreditCardLocalAmount", "modalCardAdd");
    if ($("#modalCardAdd").find("#CreditCardAmount").val() != "") {
        SetCreditCardLocalAmount();
    }
});

///////////////////////////////////////////////////////
/// Importe - tarjeta de crédito                    ///
///////////////////////////////////////////////////////
$("#modalCardAdd").find("#CreditCardAmount").blur(function () {
    if ($("#modalCardAdd").find("#CreditCardAmount").val() != "") {
        var creditCardAmount = RemoveFormatMoney($("#modalCardAdd").find("#CreditCardAmount").val());
        $("#modalCardAdd").find("#CreditCardAmount").val(FormatMoneySymbol(creditCardAmount));
        SetCreditCardLocalAmount();
    }
});

///////////////////////////////////////////////////////
/// Base impuesto - tarjeta de crédito              ///
///////////////////////////////////////////////////////
$("#modalCardAdd").find("#CreditCardTaxBase").blur(function () {
    if ($("#modalCardAdd").find("#CreditCardTaxBase").val() != "") {
        // Se valida que la base impuesto no sea mayor al importe
        var cardTaxBase = RemoveFormatMoney($("#modalCardAdd").find("#CreditCardTaxBase").val());
        var cardAmount = RemoveFormatMoney($("#modalCardAdd").find("#CreditCardAmount").val());

        if (cardTaxBase > cardAmount) {
            $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.WarningGreaterTaxBase, "warning");
            $("#modalCardAdd").find("#CreditCardTaxBase").val("");

            setTimeout(function () {
                $("#modalCardAdd").find("#alertForm").hide();
            }, 3000);
        }
        else {
            $("#modalCardAdd").find("#CreditCardTaxBase").val(FormatMoneySymbol(cardTaxBase));
        }
    }
});

/////////////////////////////////////////////////////////////
// Número voucher - tarjeta de crédito                     //
/////////////////////////////////////////////////////////////
$("#modalCardAdd").find("#CreditCardVoucherNumber").blur(function () {
    $("#modalCardAdd").find("#alertForm").hide();
    if ($("#modalCardAdd").find("#CreditCardVoucherNumber").val() != "") {
        // Se valida que no se ingrese el mismo número de voucher

        if (ValidCreditCardVoucherNumber() == false) {

            $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.WarningVoucherNumberValid, "warning");
            $("#modalCardAdd").find("#CreditCardVoucherNumber").val("");
        }
    }
});

/////////////////////////////////////////////////////////////
// Año expiración - tarjeta de crédito                     //
/////////////////////////////////////////////////////////////
$("#modalCardAdd").find("#CreditCardValidThruYear").blur(function () {
    if ($("#modalCardAdd").find("#CreditCardValidThruYear").val() > 0) {
        CheckExpirationDate();
    }
});


////////////////////////////////////////////////////
// Autocomplete banco emisor - tarjeta de crédito //
////////////////////////////////////////////////////
$("#modalCardAdd").find('#inputCreditCardIssuingBank').on('itemSelected', function (event, selectedItem) {
    issuingBankId = selectedItem.Id;
});

/////////////////////////////////////
// ListView - tarjeta              //
/////////////////////////////////////
$("#modalCardAdd").find('#cardsListView').on('rowEdit', function (event, data, index) {

    $("#modalCardAdd").find("#CreditCardAmount").val(data.Amount);
    $("#modalCardAdd").find("#CreditCardExchangeRate").val(data.ExchangeRate);
    $("#modalCardAdd").find("#selectCreditCardCurrency").UifSelect("setSelected", data.CurrencyId);
    $("#modalCardAdd").find("#selectCreditCardCurrency").trigger("change");

    if (data.PaymentTypeId == $("#ViewBagParamPaymentMethodCreditableCreditCard").val()) {
        $("#modalCardAdd").find('#checkCreditableCard').attr('class', "glyphicon glyphicon-check");
    }
    else {
        $("#modalCardAdd").find('#checkCreditableCard').attr('class', "glyphicon glyphicon-unchecked");
    }
    $("#modalCardAdd").find("#selectCreditCardType").val(data.CardType);
    issuingBankId = data.IssuingBankId;
    $("#modalCardAdd").find("#inputCreditCardIssuingBank").val(data.IssuingBankName);
    $("#modalCardAdd").find("#CreditCardNumber").val(data.CardNumber);
    $("#modalCardAdd").find("#HiddenCreditCardNumber").val(data.CardNumber);
    $("#modalCardAdd").find("#CreditCardAuthorizationNumber").val(data.AuthorizationNumber);
    $("#modalCardAdd").find("#CreditCardVoucherNumber").val(data.VoucherNumber);
    $("#modalCardAdd").find("#HiddenCreditCardVoucherNumber").val(data.VoucherNumber);
    $("#modalCardAdd").find("#CreditCardTaxBase").val(data.TaxBase);
    $("#modalCardAdd").find("#CreditCardHolderName").val(data.CardsName);
    $("#modalCardAdd").find("#selectCreditCardValidThruMonth").val(data.ValidThruMonth);
    $("#modalCardAdd").find("#CreditCardValidThruYear").val(data.ValidThruYear);
    editCreditCard = index;
});

//////////////////////////
// Botón agregar modal ///
//////////////////////////
$("#modalCardAdd").find('#CardAdd').click(function () {
    ValidateExchangeRate("CreditCardExchangeRate", "selectCreditCardCurrency", "CreditCardAmount", "CreditCardLocalAmount", "modalCardAdd", false);
    CardAdd();
});
function CardAdd() {
    var existCreditCard = 0;
    paymentMethod = "T";

    $("#addCardForm").validate();
    var lastAmount = $("#CreditCardAmount").val();
    previousValidAmount('CreditCardAmount');

    if ($("#addCardForm").valid()) {
        $("#CreditCardAmount").val(lastAmount);
        var nameIsBlankChar = isBlankChar($.trim($("#CreditCardHolderName").val()));
        $("#modalCardAdd").find("#alertForm").hide();

        if (ValidateAddFormMain(paymentMethod) == true && CheckExpirationDate() && nameIsBlankChar) {
            // Se valida que no se ingrese el mismo registro
            var keyValid = $("#addCardForm").find("#CreditCardNumber").val() +
                $("#addCardForm").find("#CreditCardVoucherNumber").val();

            var cards = $("#modalCardAdd").find('#cardsListView').UifListView("getData");

            if (cards != null) {
                for (var j = 0; j < cards.length; j++) {
                    if (editCreditCard != j) {
                        var cardIndex = cards[j].CardNumber + cards[j].VoucherNumber;
                        if (cardIndex == keyValid) {
                            existCreditCard = 1;
                            break;
                        }
                    }
                }
            }

            if (editCreditCard > -1) {
                if (($("#addCardForm").find("#CreditCardNumber").val() == $("#addCardForm").find("#HiddenCreditCardNumber").val()) &&
                    ($("#addCardForm").find("#CreditCardVoucherNumber").val() == $("#addCardForm").find("#HiddenCreditCardVoucherNumber").val())) {
                    existCreditCard = 0;
                }
            }

            if (existCreditCard == 0) {
                var rowModel = new RowPaymentModel();
                var sortingCreditable = ($("#modalCardAdd").find('#checkCreditableCard').hasClass('glyphicon glyphicon-check')) ? 1 : 0;

                if (sortingCreditable == 1) {
                    rowModel.PaymentTypeId = $("#ViewBagParamPaymentMethodCreditableCreditCard").val();
                }
                else {
                    rowModel.PaymentTypeId = $("#ViewBagParamPaymentMethodUncreditableCreditCard").val();
                }

                rowModel.PaymentTypeDescription = "TARJETA";
                rowModel.CurrencyId = $("#addCardForm").find('#selectCreditCardCurrency').val();
                rowModel.Currency = $("#addCardForm").find('#selectCreditCardCurrency option:selected').text();
                rowModel.Amount = $("#addCardForm").find('#CreditCardAmount').val();
                rowModel.ExchangeRate = $("#addCardForm").find("#CreditCardExchangeRate").val();
                rowModel.LocalAmount = $("#addCardForm").find("#CreditCardLocalAmount").val();
                rowModel.DocumentNumber = "";
                rowModel.VoucherNumber = $("#addCardForm").find("#CreditCardVoucherNumber").val();
                rowModel.CardNumber = $("#addCardForm").find("#CreditCardNumber").val();
                rowModel.CardType = $("#addCardForm").find('#selectCreditCardType').val();
                rowModel.CardTypeName = $("#addCardForm").find('#selectCreditCardType option:selected').text();
                rowModel.AuthorizationNumber = $("#addCardForm").find("#CreditCardAuthorizationNumber").val();
                rowModel.CardsName = $("#addCardForm").find("#CreditCardHolderName").val();
                rowModel.ValidThruMonth = $("#addCardForm").find('#selectCreditCardValidThruMonth').val();
                rowModel.ValidThruYear = $("#addCardForm").find('#CreditCardValidThruYear').val();
                rowModel.IssuingBankId = issuingBankId;
                rowModel.IssuingBankName = $("#addCardForm").find("#inputCreditCardIssuingBank").val();
                rowModel.IssuingBankAccountNumber = "";
                rowModel.IssuerName = "";
                rowModel.RecievingBankId = "";
                rowModel.RecievingBankName = "";
                rowModel.RecievingBankAccountNumber = "";
                rowModel.SerialVoucher = "";
                rowModel.SerialNumber = "";
                rowModel.Date = "";
                rowModel.TaxBase = $("#addCardForm").find("#CreditCardTaxBase").val();
                rowModel.Information = "";

                $("#modalCardAdd").find('#TotalCard').text(rowModel.Amount);

                if (editCreditCard == -1) {
                    $("#modalCardAdd").find('#cardsListView').UifListView("addItem", rowModel);
                }
                else {
                    $("#modalCardAdd").find('#cardsListView').UifListView("editItem", editCreditCard, rowModel);
                    editCreditCard = -1;
                }

                $("#addCardForm").formReset();
                $("#CreditCardExchangeRate").val("");
                $("#CreditCardExchangeRate").attr('disabled', 'disabled');
                newRate = 0;
                currentRate = 0;
                SetTotalPayment("T");
                SetCardSummary();
            }
            else {
                $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.ValidateVoucherNumber, "warning");
            }
        }
        else {
            if (!nameIsBlankChar) {
                $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.btnSig_In + " " + Resources.PayCreditCardName, "warning");
            }
        }
        SetTotalApplicationPayment();
    }
    else {
        $("#CreditCardAmount").val(lastAmount);
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                            EFECTIVO                                                   ///
/////////////////////////////////////////////////////////////////////////////////////////////////////////////

var saveCashCallback = function (deferred, data) {
    $("#modalCashAdd").find("#cashsListView").UifListView("addItem", data);
    deferred.resolve();
};

var editCashCallback = function (deferred, data) {
    data.NombreEmpresa = "Sistran";
    deferred.resolve(data);
};

var deleteCashCallback = function (deferred, data, index) {
    if (editCash === index) {
        editCash = -1;
    } else if (editCash > index) {
        editCash -= 1;
    }
    deferred.resolve();
    setTimeout(function () {
        SetTotalPayment("E");
        SetTotalApplicationPayment();
        SetCashSummary();
    }, 1000);
};

$("#modalCashAdd").find("#cashsListView").UifListView({
    autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
    delete: true, displayTemplate: "#cash-display-template", editCallback: editCashCallback, deleteCallback: deleteCashCallback
});

//////////////////////////////////////
// Botón Efectivo                   //
//////////////////////////////////////
$('#OpenCash').click(function () {
    paymentMethod = "E";
    editCash = -1;
    currentRate = 0;
    if ($("#TotalApplication").val() == "") {
        $("#alert").UifAlert('show', Resources.EntryTotalApplication, "warning");
        $("#TotalApplication").removeAttr("disabled", false);
        return;
    }
    else {
        if (RemoveFormatMoney($("#TotalApplication").val()) === 0) {
            $("#alert").UifAlert('show', Resources.TotalApplicationGreaterZero, "warning");
            return;
        }
        $("#modalCashAdd").find("#alertForm").hide();
        $("#addCashForm").formReset();
        LoadDefaultCurrency($('#modalCashAdd').find('#selectCashCurrency'));
        $('#modalCashAdd').UifModal('showLocal', Resources.CashIncome);
    }
});

$("#CashExchangeRate").on('blur', function (event, selectedItem) {
    ValidateExchangeRate("CashExchangeRate", "selectCashCurrency", "CashAmount", "CashLocalAmount", "modalCashAdd");
});

/////////////////////////////////////
// ListView - efectivo             //
/////////////////////////////////////
$("#modalCashAdd").find('#cashsListView').on('rowEdit', function (event, data, index) {
    $("#modalCashAdd").find("#CashAmount").val(data.Amount);
    $("#modalCashAdd").find("#CashExchangeRate").val(data.ExchangeRate);
    $("#modalCashAdd").find("#CashLocalAmount").val(data.LocalAmount);
    $("#modalCashAdd").find("#selectCashCurrency").UifSelect("setSelected", data.CurrencyId);
    $("#modalCashAdd").find("#selectCashCurrency").trigger("change");
    $("#modalCashAdd").find("#hiddenCashCurrency").val(data.CurrencyId);
    editCash = index;
});

$("#modalCashAdd").find('#cashsListView').on('rowDelete', function (event, data, index) {
    SetTotalPayment("E");
    SetTotalApplicationPayment();
    if (editCash === index) {
        editCash = -1;
    }
});

///////////////////////////////////////////////////////
/// Combo moneda - efectivo                         ///
///////////////////////////////////////////////////////
$("#modalCashAdd").find("#selectCashCurrency").on('itemSelected', function (event, selectedItem) {
    $("#CashExchangeRate").removeAttr("disabled");
    SetCurrencyMainBilling("selectCashCurrency", "CashExchangeRate", "CashAmount", "CashLocalAmount", "modalCashAdd");
    if ($("#modalCashAdd").find("#CashAmount").val() != "") {
        SetCashLocalAmount();
    }
});

///////////////////////////////////////////////////////
/// Importe - efectivo                              ///
///////////////////////////////////////////////////////
$("#modalCashAdd").find("#CashAmount").blur(function () {
    if ($("#modalCashAdd").find("#CashAmount").val() != "") {
        var cashAmount = RemoveFormatMoney($("#modalCashAdd").find("#CashAmount").val());
        $("#modalCashAdd").find("#CashAmount").val(FormatMoneySymbol(cashAmount));
        SetCashLocalAmount();
    }
});

//////////////////////////
// Botón agregar modal ///
//////////////////////////
$("#modalCashAdd").find('#CashAdd').click(function () {
    ValidateExchangeRate("CashExchangeRate", "selectCashCurrency", "CashAmount", "CashLocalAmount", "modalCashAdd", false);
    CashAdd();
});
function CashAdd() {
    $("#modalCashAdd").find("#alertForm").hide();
    paymentMethod = "E";

    $("#addCashForm").validate();
    var lastAmount = $("#CashAmount").val();
    previousValidAmount('CashAmount');

    if ($("#addCashForm").valid()) {
        $("#CashAmount").val(lastAmount);
        var exist = 0;
        if (ValidateAddFormMain(paymentMethod) == true) {
            // Se valida que no ingrese más de un registro en efectivo
            var cashs = $("#modalCashAdd").find("#cashsListView").UifListView("getData");

            if (cashs != null) {
                for (var j = 0; j < cashs.length; j++) {
                    if (editCash != j) {
                        if (cashs[j].CurrencyId == $("#addCashForm").find('#selectCashCurrency').val()) {
                            exist = 1;
                            break;
                        }
                    }
                }
            }
            if (editCash > -1) {
                if ($("#addCashForm").find("#selectCashCurrency").val() == $("#addCashForm").find("#hiddenCashCurrency").val()) {
                    exist = 0;
                }
            }
            if (exist == 0) {
                var rowModel = new RowPaymentModel();
                rowModel.PaymentTypeId = $("#ViewBagParamPaymentMethodCash").val();
                rowModel.PaymentTypeDescription = "EFECTIVO";
                rowModel.CurrencyId = $("#addCashForm").find('#selectCashCurrency').val();
                rowModel.Currency = $("#addCashForm").find('#selectCashCurrency option:selected').text();
                rowModel.Amount = $("#addCashForm").find('#CashAmount').val();
                rowModel.ExchangeRate = $("#addCashForm").find("#CashExchangeRate").val();
                rowModel.LocalAmount = $("#addCashForm").find("#CashLocalAmount").val();
                rowModel.DocumentNumber = "";
                rowModel.VoucherNumber = "";
                rowModel.CardNumber = "";
                rowModel.CardType = "";
                rowModel.CardTypeName = "";
                rowModel.AuthorizationNumber = "";
                rowModel.CardsName = "";
                rowModel.ValidThruMonth = "";
                rowModel.ValidThruYear = "";
                rowModel.IssuingBankId = "";
                rowModel.IssuingBankName = "";
                rowModel.IssuingBankAccountNumber = "";
                rowModel.IssuerName = "";
                rowModel.RecievingBankId = "";
                rowModel.RecievingBankName = "";
                rowModel.RecievingBankAccountNumber = "";
                rowModel.SerialVoucher = "";
                rowModel.SerialNumber = "";
                rowModel.Date = "";
                rowModel.TaxBase = "";
                rowModel.Information = "";
                // TotalCash

                if (editCash == -1) {
                    $("#modalCashAdd").find('#cashsListView').UifListView("addItem", rowModel);
                }
                else {
                    $("#modalCashAdd").find('#cashsListView').UifListView("editItem", editCash, rowModel);
                    editCash = -1;
                }
                $("#addCashForm").formReset();
                $("#CashExchangeRate").val("");
                $("#CashExchangeRate").attr('disabled', 'disabled');
                newRate = 0;
                currentRate = 0;
                LoadDefaultCurrency($('#modalCashAdd').find('#selectCashCurrency'));
                SetTotalPayment("E");
                SetCashSummary();
            }
            else {
                $("#modalCashAdd").find("#alertForm").UifAlert('show', Resources.ValidateCashEntry, "warning");
            }
        }
        SetTotalApplicationPayment();
    }
    else {
        $("#CashAmount").val(lastAmount);
    }

}
//////////////////////////////////////
// CheckBox                         //
//////////////////////////////////////
$('span').click(function () {
    if ($("#ViewBagControlSpanMainBilling").val() == "true") {
        if ($(this).hasClass("glyphicon glyphicon-unchecked")) {
            $(this).removeClass("glyphicon glyphicon-unchecked");
            $(this).addClass("glyphicon glyphicon-check");
        }
        else if ($(this).hasClass("glyphicon glyphicon-check")) {
            $(this).removeClass("glyphicon glyphicon-check");
            $(this).addClass("glyphicon glyphicon-unchecked");
        }
    }
});

//////////////////////////////////////
// Botón Grabar                     //
//////////////////////////////////////
//$('#SaveBill').click(function () {

//    $("#formBilling").validate();

//    if ($("#formBilling").valid()) {

//        if ($("#Observation").val() == "") {
//            $("#alert").UifAlert('show', Resources.ObservationRequired, "warning");
//            return;
//        }
//        if ($("#selectIncomeConcept").val() == "") {
//            $("#alert").UifAlert('show', Resources.IncomeConceptRequired, "warning");
//            return;
//        }
//        if ($("#selectBranch").val() == "") {
//            $("#alert").UifAlert('show', Resources.SelectBranch, "warning");
//            return;
//        }

//        if ($("#TotalReceipt").text().replace("$", "").replace(/,/g, "").replace(" ", "") == "0.00") {
//            $("#alert").UifAlert('show', Resources.TotalReceiptGreaterZero, "warning");
//            return;
//        }

//        if ($("#TotalApplication").val() == "") {
//            $("#alert").UifAlert('show', Resources.EntryTotalApplication, "warning");
//            return
//        }
//        else {
//            if ($("#TotalApplication").val().replace("$", "").replace(/,/g, "").replace(" ", "") == "0.00") {
//                $("#alert").UifAlert('show', Resources.TotalApplicationGreaterZero, "warning");
//                return;
//            }
//        }
//        uiScreen = 1;
//        lockScreen();
//        // Validación de cierre de caja
//        if ($("#selectBranch").val() != -1 && $("#Difference").text().replace("$", "").replace(/,/g, "").replace(" ", "") == "0.00") {

//            //ClosureBillingMain();
//        }
//        else {
//            $.unblockUI();
//            $("#alert").UifAlert('show', Resources.RemainingDifference, "warning");
//        }
//    }
//});

//////////////////////////////////////
// Botón Aplicar                    //
//////////////////////////////////////
//$("#modalSaveBill").find("#ApplyReceipt").click(function () {

//    ApplyBill(applyBillId);
//    applyComments = "";
//    //debugger;

//    window.location.href = $("#ViewBagLoadApplicationReceiptLink").val() + "?receiptNumber=" + applyReceiptNumber +
//        "&depositer=" + applyDepositerBilling + "&amount=" + applyAmount + "&localAmount=" + applyLocalAmount + "&branch=" +
//        applyBranch + "&incomeConcept=" + applyIncomeConcept + "&postedValue=" + applyPostedValue + "&description=" +
//        applyDescription + "&accountingDate=" + applyAccountingDate +
//        "&tempImputationId=" + tempImputationId + "&comments=" + applyComments + "&technicalTransaction=" + applyTechnicalTransaction +
//        "&applyCollecId=" + applyBillId;


//    $('#modalSaveBill').UifModal('hide');
//});

///////////////////////////////////////////////////////
/// Combo moneda - efectivo                         ///
///////////////////////////////////////////////////////
$("#modalAdd").find("#selectCashCurrency").on('itemSelected', function (event, selectedItem) {
    $("#CashExchangeRate").removeAttr("disabled");
    SetCurrencyMainBilling("selectCashCurrency", "CashExchangeRate");
    if ($("#modalAdd").find("#CashAmount").val() != "") {
        SetCashLocalAmount();
    }
});

///////////////////////////////////////////////////////
/// Importe - efectivo                              ///
///////////////////////////////////////////////////////
$("#modalAdd").find("#CashAmount").blur(function () {
    var cashAmount = RemoveFormatMoney($("#modalAdd").find("#CashAmount").val());
    $("#modalAdd").find("#CashAmount").val(FormatMoneySymbol(cashAmount));
    SetCashLocalAmount();
});

//////////////////////////
// Botón agregar modal ///
//////////////////////////
$("#modalAdd").find('#btnAddPayment').click(function () {
    $("#addForm").validate();

    if ($("#addForm").valid()) {

        if (ValidateAddFormMain(paymentMethod) == true) {
            var rowPaymentModel = new RowPaymentModel();
            rowPaymentModel = SetPayment(paymentMethod);

            $("#modalAdd").find('#tablePayments').UifDataTable('addRow', rowPaymentModel);

            $("#addField").formReset();
        }
        else {
            setTimeout(function () {
                $("#alertForm").hide();
            }, 3000);
        }
    }
});

////////////////////////
// Botón grabar modal //
////////////////////////
$('#saveAdd').click(function () {
    $("#addForm").validate();

    if ($("#addForm").valid()) {

        if (ValidateAddFormMain(paymentMethod) == true) {
            var rowModel = new RowModel();

            rowModel.PaymentMethod = "Efectivo";
            rowModel.CurrencyId = $("#addForm").find('#selectCashCurrency').val();
            rowModel.CurrencyDescription = $("#addForm").find('#selectCashCurrency option:selected').html();
            rowModel.Amount = $("#addForm").find('#CashAmount').val();
            rowModel.ExchangeRate = $("#addForm").find("#CashExchangeRate").val();
            rowModel.LocalAmount = $("#addForm").find("#CashLocalAmount").val();

            $("#summariesListView").UifListView(
                {
                    height: '40%',
                    source: rowModel,
                    customDelete: false,
                    customAdd: false,
                    customEdit: false,
                    delete: true,
                    displayTemplate: "#summaries-template"
                });

            $("#addField").formReset();
            $('#modalAdd').UifModal('hide');
        }
        else {
            setTimeout(function () {
                $("#alertForm").hide();
            }, 3000);
        }
    }
});

/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                          TRANSFERENCIA                                                ///
/////////////////////////////////////////////////////////////////////////////////////////////////////////////

var saveTransferCallback = function (deferred, data) {
    $("#modalTransferAdd").find("#transferListView").UifListView("addItem", data);
    deferred.resolve();
};

var editTransferCallback = function (deferred, data) {
    data.NombreEmpresa = "Sistran";
    deferred.resolve(data);
};

var deleteTransferCallback = function (deferred, data) {
    deferred.resolve();
    setTimeout(function () {
        SetTotalPayment("TR");
        SetTotalApplicationPayment();
        SetTransferSummary();
    }, 1000);
};

$("#modalTransferAdd").find("#transferListView").UifListView({
    autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
    delete: true, displayTemplate: "#transfer-display-template", editCallback: editTransferCallback, deleteCallback: deleteTransferCallback
});

//////////////////////////////////////
// Botón Trasnferencia              //
//////////////////////////////////////
$('#OpenTransfer').click(function () {
    editTransfer = -1;
    paymentMethod = "TR";
    currentRate = 0;

    if ($("#TotalApplication").val() == "") {
        $("#alert").UifAlert('show', Resources.EntryTotalApplication, "warning");
        return;
    }
    else {
        if (RemoveFormatMoney($("#TotalApplication").val()) === 0) {
            $("#alert").UifAlert('show', Resources.TotalApplicationGreaterZero, "warning");
            return;
        }
        $("#addTransferForm").formReset();
        $('#modalTransferAdd').UifModal('showLocal', Resources.IncomeTransfer);
    }

});

$("#TransferExchangeRate").on('blur', function (event, selectedItem) {
    ValidateExchangeRate("TransferExchangeRate", "selectTransferCurrency", "TransferAmount", "TransferLocalAmount", "modalTransferAdd");
});


///////////////////////////////////////////////////////
/// Combo moneda - Transferencia                    ///
///////////////////////////////////////////////////////
$("#modalTransferAdd").find("#selectTransferCurrency").on('itemSelected', function (event, selectedItem) {
    $("#TransferExchangeRate").removeAttr("disabled");
    SetCurrencyMainBilling("selectTransferCurrency", "TransferExchangeRate");
    if ($("#modalTransferAdd").find("#TransferAmount").val() != "") {
        SetTransferLocalAmount();
    }
});

///////////////////////////////////////////////////////
/// Importe - Transferencia                         ///
///////////////////////////////////////////////////////
$("#modalTransferAdd").find("#TransferAmount").blur(function () {

    if ($("#modalTransferAdd").find("#TransferAmount").val() != "") {
        var transferAmount = RemoveFormatMoney($("#modalTransferAdd").find("#TransferAmount").val());
        $("#modalTransferAdd").find("#TransferAmount").val((FormatMoneySymbol(transferAmount)));
        SetTransferLocalAmount();
    }
});

/////////////////////////////////////////
// Combo banco receptor - transferencia //
/////////////////////////////////////////
$("#modalTransferAdd").find("#selectTransferReceivingBank").on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        var controller = ACC_ROOT + "Billing/GetAccountByBankId?bankId=" + selectedItem.Id;
        $("#modalTransferAdd").find("#selectTransferAccountNumber").UifSelect({ source: controller });
    }
});

$("#modalTransferAdd").find('#transferListView').on('rowDelete', function (event, data) {

});

////////////////////////////////////////////////////
// Autocomplete banco emisor - Transferencia      //
////////////////////////////////////////////////////
$("#modalTransferAdd").find('#inputTransferIssuingBank').on('itemSelected', function (event, selectedItem) {
    issuingBankId = selectedItem.Id;
});

//////////////////////////
// Botón agregar modal ///
//////////////////////////
$("#modalTransferAdd").find('#TransferAdd').click(function () {
    ValidateExchangeRate("TransferExchangeRate", "selectTransferCurrency", "TransferAmount", "TransferLocalAmount", "modalTransferAdd", false);
    TransferAdd();
});

function TransferAdd() {
    paymentMethod = "TR";
    var existTransfer = 0;

    $("#addTransferForm").validate();
    var lastAmount = $("#TransferAmount").val();
    previousValidAmount('TransferAmount');

    if ($("#addTransferForm").valid()) {
        $("#TransferAmount").val(lastAmount);
        var nameIsBlankChar = isBlankChar($.trim($("#TransferHolderName").val()));
        $("#modalTransferAdd").find("#alertForm").hide();

        if (ValidateAddFormMain(paymentMethod) == true && nameIsBlankChar) {
            // Se valida que en un nuevo registro no se duplique la información 
            if (editTransfer == -1) {
                var keyValid = $("#addTransferForm").find("#selectTransferReceivingBank option:selected").text() +
                    $("#addTransferForm").find("#TransferDocumentNumber").val() +
                    $("#addTransferForm").find("#selectTransferAccountNumber option:selected").text();

                var transfers = $("#modalTransferAdd").find('#transferListView').UifListView("getData");

                if (transfers != null) {
                    for (var j = 0; j < transfers.length; j++) {
                        var transferIndex = transfers[j].RecievingBankName +
                            transfers[j].DocumentNumber +
                            transfers[j].RecievingBankAccountNumber;
                        if (transferIndex == keyValid) {
                            existTransfer = 1;
                            break;
                        }
                    }
                }
            }

            if (editTransfer > -1) {
                if (($("#addTransferForm").find("#selectTransferReceivingBank option:selected").text() == $("#addTransferForm").find("#HiddenReceivingBankName").val()) &&
                    ($("#addTransferForm").find("#TransferDocumentNumber").val() == $("#addTransferForm").find("#HiddenTransferDocumentNumber").val()) &&
                    ($("#addTransferForm").find("#selectTransferAccountNumber option:selected").text() == $("#addTransferForm").find("#HiddenAccountNumber").val())) {
                    existTransfer = 0;
                }
            }

            if (existTransfer == 0) {
                var rowModel = new RowModel();

                rowModel.PaymentTypeId = $("#ViewBagParamPaymentMethodTransfer").val();
                rowModel.PaymentTypeDescription = "TRANSFERENCIA";
                rowModel.CurrencyId = $("#addTransferForm").find('#selectTransferCurrency').val();
                rowModel.Currency = $("#addTransferForm").find('#selectTransferCurrency option:selected').text();
                rowModel.Amount = $("#addTransferForm").find('#TransferAmount').val();
                rowModel.ExchangeRate = $("#addTransferForm").find("#TransferExchangeRate").val();
                rowModel.LocalAmount = $("#addTransferForm").find("#TransferLocalAmount").val();
                rowModel.DocumentNumber = $("#addTransferForm").find("#TransferDocumentNumber").val();
                rowModel.VoucherNumber = "";
                rowModel.CardNumber = "";
                rowModel.CardType = "";
                rowModel.CardTypeName = "";
                rowModel.AuthorizationNumber = "";
                rowModel.CardsName = "";
                rowModel.ValidThruMonth = "";
                rowModel.ValidThruYear = "";
                rowModel.IssuingBankId = issuingBankId;
                rowModel.IssuingBankName = $("#addTransferForm").find("#inputTransferIssuingBank").val();
                rowModel.IssuingBankAccountNumber = $("#addTransferForm").find("#TransferAccountNumber").val();
                rowModel.IssuerName = $("#addTransferForm").find("#TransferHolderName").val();
                rowModel.RecievingBankId = $("#addTransferForm").find("#selectTransferReceivingBank").val();
                rowModel.RecievingBankName = $("#addTransferForm").find("#selectTransferReceivingBank option:selected").text();
                rowModel.RecievingBankAccountNumber = $("#addTransferForm").find("#selectTransferAccountNumber option:selected").text();
                rowModel.SerialVoucher = $("#addTransferForm").find("#selectTransferAccountNumber").val(); // id account number
                rowModel.SerialNumber = "";
                rowModel.Date = $("#addTransferForm").find("#TransferDate").val();
                rowModel.TaxBase = "";
                rowModel.Information = "";

                $("#modalTransferAdd").find('#TotalTransfer').text(rowModel.Amount);
                if (editTransfer == -1) {
                    $("#modalTransferAdd").find('#transferListView').UifListView("addItem", rowModel);
                }
                else {
                    $("#modalTransferAdd").find('#transferListView').UifListView("editItem", editTransfer, rowModel);
                    editTransfer = -1;
                }

                $("#addTransferForm").formReset();
                $("#TransferExchangeRate").val("");
                $("#TransferExchangeRate").attr('disabled', 'disabled');
                newRate = 0;
                currentRate = 0;
                SetTotalPayment("TR");
                SetTransferSummary();
            }
            else {
                $("#modalTransferAdd").find("#alertForm").UifAlert('show', Resources.ValidateDuplicatePaymentMethods, "warning");
            }
        }
        else {
            if (!nameIsBlankChar) {
                $("#modalTransferAdd").find("#alertForm").UifAlert('show', Resources.EnterSurnamesAndNames, "warning");
            }
        }
        SetTotalApplicationPayment();
    }
    else {
        $("#TransferAmount").val(lastAmount);
    }
}
/////////////////////////////////////
// ListView - Transferencia        //
/////////////////////////////////////
$("#modalTransferAdd").find('#transferListView').on('rowEdit', function (event, data, index) {
    $("#modalTransferAdd").find("#selectTransferCurrency").val(data.CurrencyId);
    $("#modalTransferAdd").find("#TransferAmount").val(data.Amount);
    $("#modalTransferAdd").find("#TransferExchangeRate").val(data.ExchangeRate);
    $("#modalTransferAdd").find("#TransferLocalAmount").val(data.LocalAmount);
    $("#modalTransferAdd").find("#selectTransferReceivingBank").val(data.RecievingBankId);
    $("#modalTransferAdd").find("#HiddenReceivingBankName").val(data.RecievingBankName);
    $("#modalTransferAdd").find("#selectTransferAccountNumber").val(data.SerialVoucher);
    $("#modalTransferAdd").find("#HiddenAccountNumber").val(data.RecievingBankAccountNumber);
    $("#modalTransferAdd").find("#TransferDocumentNumber").val(data.DocumentNumber);
    $("#modalTransferAdd").find("#HiddenTransferDocumentNumber").val(data.DocumentNumber);
    $("#modalTransferAdd").find("#TransferDate").val(data.Date);
    $("#modalTransferAdd").find("#TransferHolderName").val(data.IssuerName);
    $("#modalTransferAdd").find("#inputTransferIssuingBank").val(data.IssuingBankName);
    $("#modalTransferAdd").find("#TransferAccountNumber").val(data.IssuingBankAccountNumber);

    editTransfer = index;
});

$("#modalTransferAdd").find('#transferListView').on('rowDelete', function (event, data) {

});

/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                          RETENCIONES                                                  ///
/////////////////////////////////////////////////////////////////////////////////////////////////////////////

var saveRetentionCallback = function (deferred, data) {
    $("#modalRetentionAdd").find("#retentionListView").UifListView("addItem", data);
    deferred.resolve();
};

var editRetentionCallback = function (deferred, data) {
    data.NombreEmpresa = "Sistran";
    deferred.resolve(data);
};

var deleteRetentionCallback = function (deferred, data) {
    deferred.resolve();
    setTimeout(function () {
        SetTotalPayment("R");
        SetTotalApplicationPayment();
        SetRetentionSummary();
    }, 1000);
};

$("#modalRetentionAdd").find("#retentionListView").UifListView({
    autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
    delete: true, displayTemplate: "#retention-display-template", editCallback: editRetentionCallback, deleteCallback: deleteRetentionCallback
});

//////////////////////////////////////
// Botón Retenciones                //
//////////////////////////////////////
$('#OpenRetention').click(function () {
    editRetention = -1;
    paymentMethod = "R";
    currentRate = 0;

    if ($("#TotalApplication").val() == "") {
        $("#alert").UifAlert('show', Resources.EntryTotalApplication, "warning");
        return
    }
    else {
        if (RemoveFormatMoney($("TotalApplication").val()) === 0) {
            $("#alert").UifAlert('show', Resources.TotalApplicationGreaterZero, "warning");
            return;
        }
        $("#addRetentionForm").formReset();
        $('#modalRetentionAdd').UifModal('showLocal', Resources.IncomeRetention);
    }

});

$("#RetentionExchangeRate").on('blur', function (event, selectedItem) {
    ValidateExchangeRate("RetentionExchangeRate", "selectRetentionCurrency", "RetentionAmount", "RetentionLocalAmount", "modalRetentionAdd");
});

///////////////////////////////////////////////////////
/// Combo moneda - Retenciones                      ///
///////////////////////////////////////////////////////
$("#modalRetentionAdd").find("#selectRetentionCurrency").on('itemSelected', function (event, selectedItem) {
    $("#RetentionExchangeRate").removeAttr("disabled");
    SetCurrencyMainBilling("selectRetentionCurrency", "RetentionExchangeRate");
    if ($("#modalRetentionAdd").find("#RetentionAmount").val() != "") {
        SetRetentionLocalAmount();
    }
});

///////////////////////////////////////////////////////
/// Importe - Retenciones                           ///
///////////////////////////////////////////////////////
$("#modalRetentionAdd").find("#RetentionAmount").blur(function () {
    if ($("#modalRetentionAdd").find("#RetentionAmount").val() != "") {
        var retentionAmount = RemoveFormatMoney($("#modalRetentionAdd").find("#RetentionAmount").val());
        $("#modalRetentionAdd").find("#RetentionAmount").val(FormatMoneySymbol(retentionAmount));
        SetRetentionLocalAmount();
    }
});

$("#modalRetentionAdd").find('#retentionListView').on('rowDelete', function (event, data) {

});

//////////////////////////
// Botón agregar modal ///
//////////////////////////
$("#modalRetentionAdd").find('#RetentionAdd').click(function () {
    ValidateExchangeRate("RetentionExchangeRate", "selectRetentionCurrency", "RetentionAmount", "RetentionLocalAmount", "modalRetentionAdd", false);
    RetentionAdd();
});
function RetentionAdd() {

    paymentMethod = "R";
    var existRetention = 0;

    $("#addRetentionForm").validate();
    var lastAmount = $("#RetentionAmount").val();
    previousValidAmount('RetentionAmount');

    if ($("#addRetentionForm").valid()) {
        $("#RetentionAmount").val(lastAmount);
        if (ValidateAddFormMain(paymentMethod) == true) {
            // Se valida que no se ingrese el mismo registro
            var keyValid = $("#addRetentionForm").find("#RetentionAutorizationNumber").val() +
                $("#addRetentionForm").find("#RetentionBillNumber").val() +
                $("#addRetentionForm").find("#RetentionVoucherNumber").val();

            var retention = $("#modalRetentionAdd").find('#retentionListView').UifListView("getData");

            if (retention != null) {
                for (var j = 0; j < retention.length; j++) {
                    var retentionIndex = retention[j].RetentionNumberSerie +
                        retention[j].RetentionBillNumber +
                        retention[j].RetentionVoucherNumber;
                    if (retentionIndex == keyValid) {
                        existRetention = 1;
                        break;
                    }
                }
            }

            if (editRetention > -1) {
                if (($("#addRetentionForm").find("#RetentionAutorizationNumber").val() == $("#addTransferForm").find("#HiddenRetentionAutorizationNumber").val()) &&
                    ($("#addRetentionForm").find("#RetentionBillNumber").val() == $("#addTransferForm").find("#HiddenRetentionBillNumber").val()) &&
                    ($("#addRetentionForm").find("#RetentionVoucherNumber").val() == $("#addTransferForm").find("#HiddenRetentionVoucherNumber").val())) {
                    existRetention = 0;
                }
            }

            if (existRetention == 0) {
                var rowModel = new RowModel();

                rowModel.PaymentTypeId = $("#ViewBagParamPaymentMethodRetentionReceipt").val();
                rowModel.PaymentTypeDescription = "RETENCION";
                rowModel.CurrencyId = $("#addRetentionForm").find('#selectRetentionCurrency').val();
                rowModel.Currency = $("#addRetentionForm").find('#selectRetentionCurrency option:selected').text();
                rowModel.Amount = $("#addRetentionForm").find('#RetentionAmount').val();
                rowModel.ExchangeRate = $("#addRetentionForm").find("#RetentionExchangeRate").val();
                rowModel.LocalAmount = $("#addRetentionForm").find("#RetentionLocalAmount").val();
                rowModel.DocumentNumber = $("#addRetentionForm").find("#RetentionBillNumber").val();
                rowModel.VoucherNumber = $("#addRetentionForm").find("#RetentionVoucherNumber").val();
                rowModel.CardNumber = "";
                rowModel.CardType = "";
                rowModel.CardTypeName = "";
                rowModel.AuthorizationNumber = $("#addRetentionForm").find("#RetentionAutorizationNumber").val();
                rowModel.CardsName = "";
                rowModel.ValidThruMonth = "";
                rowModel.ValidThruYear = "";
                rowModel.IssuingBankId = "";
                rowModel.IssuingBankName = "";
                rowModel.IssuingBankAccountNumber = "";
                rowModel.IssuerName = "";
                rowModel.RecievingBankId = "";
                rowModel.RecievingBankName = "";
                rowModel.RecievingBankAccountNumber = "";
                rowModel.SerialVoucher = $("#addRetentionForm").find("#SelectRetentionConcept").val();
                rowModel.SerialNumber = retentionPolicyId; //$("#addRetentionForm").find("#RetentionSerial").val()
                rowModel.Date = $("#addRetentionForm").find("#RetentionBillDate").val();
                rowModel.IssueDate = $("#addRetentionForm").find("#RetentionIssueDate").val();
                rowModel.ExpirationDate = $("#addRetentionForm").find("#RetentionExpirationDate").val();
                rowModel.TaxBase = "";
                rowModel.Information = "";
                rowModel.BranchId = $("#addRetentionForm").find('#selectRetentionBranch').val();
                rowModel.Branch = $("#addRetentionForm").find('#selectRetentionBranch option:selected').text();
                rowModel.PrefixId = $("#addRetentionForm").find('#selectRetentionPrefix').val();
                rowModel.Prefix = $("#addRetentionForm").find('#selectRetentionPrefix option:selected').text();
                rowModel.PolicyNumber = $("#addRetentionForm").find('#RetentionPolicy').val();
                rowModel.EndorsementNumber = $("#addRetentionForm").find('#RetentionEndorsement').val();
                rowModel.RetentionConceptId = $("#addRetentionForm").find('#SelectRetentionConcept').val();
                rowModel.RetentionConcept = $("#addRetentionForm").find('#SelectRetentionConcept option:selected').text();
                rowModel.InvoiceNumber = $("#addRetentionForm").find("#RetentionBillNumber").val();
                rowModel.InvoiceDate = $("#addRetentionForm").find("#RetentionBillDate").val();

                $("#modalRetentionAdd").find('#TotalRetention').text(rowModel.Amount);
                if (editRetention == -1) {
                    $("#modalRetentionAdd").find('#retentionListView').UifListView("addItem", rowModel);
                }
                else {
                    $("#modalRetentionAdd").find('#retentionListView').UifListView("editItem", editTransfer, rowModel);
                    editRetention = -1;
                }

                $("#addRetentionForm").formReset();
                $("#RetentionAmount").val("");
                $("#RetentionAmount").attr('disabled', 'disabled');
                newRate = 0;
                currentRate = 0;
                SetTotalPayment("R");
                SetRetentionSummary();
            }
            else {
                $("#modalRetentionAdd").find("#alertForm").UifAlert('show', Resources.ValidateDuplicatePaymentMethods, "warning");
            }
        }
        SetTotalApplicationPayment();

        setTimeout(function () {
            $("#modalRetentionAdd").find("#alertForm").hide();
        }, 3000);
    }
    else {
        $("#RetentionAmount").val(lastAmount);
    }

}
$("#modalRetentionAdd").find('#RetentionPolicy').blur(function () {
    GetPolicyId();
});

///////////////////////////////////////////////////////////////////
// Fecha expiración - Retenciones                               //
/////////////////////////////////////////////////////////////////
$("#modalRetentionAdd").find("#RetentionExpirationDate").blur(function () {
    if ($("#modalRetentionAdd").find("#RetentionExpirationDate").val() != "") {
        if (IsDate($("#RetentionExpirationDate").val()) == true) {
            ValidateExpirationDateRetention();
        }
        else {
            $("#modalRetentionAdd").find("#alertForm").UifAlert('show', Resources.BillingInvalidDate, "warning");
            $("#RetentionExpirationDate").val("");
        }
    }
});

/////////////////////////////////////
// ListView - Retenciones          //
/////////////////////////////////////
$("#modalRetentionAdd").find('#retentionListView').on('rowEdit', function (event, data, index) {
    $("#modalRetentionAdd").find("#selectRetentionCurrency").val(data.CurrencyId);
    $("#modalRetentionAdd").find("#RetentionAmount").val(data.Amount);
    $("#modalRetentionAdd").find("#RetentionExchangeRate").val(data.ExchangeRate);
    $("#modalRetentionAdd").find("#RetentionLocalAmount").val(data.LocalAmount);
    $("#modalRetentionAdd").find("#RetentionNumberSerie").val(data.SerialNumber);
    $("#modalRetentionAdd").find("#RetentionBillNumber").val(data.DocumentNumber);
    $("#modalRetentionAdd").find("#HiddenRetentionBillNumber").val(data.DocumentNumber);
    $("#modalRetentionAdd").find("#RetentionAutorizationNumber").val(data.AuthorizationNumber);
    $("#modalRetentionAdd").find("#HiddenRetentionAutorizationNumber").val(data.AuthorizationNumber);
    $("#modalRetentionAdd").find("#RetentionVoucherNumber").val(data.VoucherNumber);
    $("#modalRetentionAdd").find("#HiddenRetentionVoucherNumber").val(data.VoucherNumber);
    $("#modalRetentionAdd").find("#RetentionSerial").val(data.SerialVoucher);
    $("#modalRetentionAdd").find("#RetentionBillDate").val(data.Date);
    $("#modalRetentionAdd").find("#RetentionIssueDate").val(data.IssueDate);
    $("#modalRetentionAdd").find("#RetentionExpirationDate").val(data.ExpirationDate);
    $("#modalRetentionAdd").find("#selectRetentionBranch").val(data.BranchId);
    $("#modalRetentionAdd").find("#selectRetentionPrefix").val(data.PrefixId);
    $("#modalRetentionAdd").find("#RetentionPolicy").val(data.PolicyNumber);
    $("#modalRetentionAdd").find("#RetentionEndorsement").val(data.EndorsementNumber);
    $("#modalRetentionAdd").find("#SelectRetentionConcept").val(data.RetentionConceptId);

    editRetention = index;
});

$("#modalRetentionAdd").find('#retentionListView').on('rowDelete', function (event, data) {

});

/////////////////////////////////////////////////////////////////////////////////////////////////////////////
///                                            PRIMAS                                                     ///
/////////////////////////////////////////////////////////////////////////////////////////////////////////////

var savePremiumCheckCallback = function (deferred, data) {
    $("#modalPremiumAdd").find("#premiumsListView").UifListView("addItem", data);
    deferred.resolve();
};

var editPremiumCallback = function (deferred, data) {
    data.NombreEmpresa = "Sistran";
    deferred.resolve(data);
};

var deletePremiumCallback = function (deferred, data) {
    deferred.resolve();
    setTimeout(function () {
        SetTotalPayment("C");
        SetTotalApplicationPayment();
        SetMovementsSummary("C");
    }, 1000);
};

$("#modalPremiumAdd").find("#premiumsListView").UifListView({
    autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
    delete: true, displayTemplate: "#premiums-template", editCallback: editPremiumCallback, deleteCallback: deletePremiumCallback
});

///////////////////////////////////////////////////////
/// Combo buscar por - prima                        ///
///////////////////////////////////////////////////////
$("#modalPremiumAdd").find("#selectSearch").on('itemSelected', function (event, selectedItem) {

    //Limpia campos
    $("#premiumsListView").UifListView({ source: null });
    $("#modalPremiumAdd").find("#SearchBy").val("");
    $("#modalPremiumAdd").find("#selectBranch").val("");
    $("#modalPremiumAdd").find("#selectPrefix").val("");
    $("#modalPremiumAdd").find("#InsuredDocumentNumberBill").val("");

    if ($("#modalPremiumAdd").find("#selectSearch").val() != "") {
        if ($("#modalPremiumAdd").find("#selectSearch").val() == "1") {
            $("#DivSearchBy").show();
            $("#DivInsured").hide();
            $("#modalPremiumAdd").find("#TitleSearchBy").text(Resources.PolicyNumber);
            $("#modalPremiumAdd").find("#SearchBy").attr('onkeypress', "return JustNumbers(event, this)");
            $("#modalPremiumAdd").find("#SearchBy").attr('maxlength', "12");
        }
        if ($("#modalPremiumAdd").find("#selectSearch").val() == "2") {
            $("#DivSearchBy").hide();
            $("#DivInsured").show();
            $("#modalPremiumAdd").find("#TitleSearchBy").text(Resources.PolicyHolderDocumentNumber);
        }
        if ($("#modalPremiumAdd").find("#selectSearch").val() == "3") {
            $("#DivSearchBy").show();
            $("#DivInsured").hide();
            $("#modalPremiumAdd").find("#TitleSearchBy").text(Resources.PayerDocumentNumber);
            $("#modalPremiumAdd").find("#SearchBy").attr('onkeypress', "return JustNumbers(event, this)");
            $("#modalPremiumAdd").find("#SearchBy").attr('maxlength', "12");
        }
        if ($("#modalPremiumAdd").find("#selectSearch").val() == "4") {
            $("#DivSearchBy").show();
            $("#DivInsured").hide();
            $("#modalPremiumAdd").find("#TitleSearchBy").text(Resources.Payer);
            $("#modalPremiumAdd").find("#SearchBy").removeAttr('onkeypress');
            $("#modalPremiumAdd").find("#SearchBy").attr('maxlength', "70");
        }
        if ($("#modalPremiumAdd").find("#selectSearch").val() == "5") {
            $("#DivSearchBy").show();
            $("#DivInsured").hide();
            $("#modalPremiumAdd").find("#TitleSearchBy").text(Resources.PolicyHolder);
            $("#modalPremiumAdd").find("#SearchBy").removeAttr('onkeypress');
            $("#modalPremiumAdd").find("#SearchBy").attr('maxlength', "70");
        }
        $("#modalPremiumAdd").find("#SearchBy").val("");
    }
});

////////////////////////////////////////
/// ListView primas                  ///
////////////////////////////////////////
$("#modalPremiumAdd").find('#premiumsListView').on('rowEdit', function (event, data, index) {
    var existPremium = 0;
    var premiums = $("#policiesListView").UifListView("getData");

    if (TotalDebit() != 0 || TotalCredit() != 0) {
        $("#modalPremiumAdd").find("#alertForm").UifAlert('show', Resources.ItemsMessage, "warning");
    }
    else {
        var result = QuotaOrderControl();

        // Se valida que no se ingrese el mismo registro
        if (premiums != null) {
            for (var j = 0; j < premiums.length; j++) {
                if (premiums[j].BranchId == data.BranchId && premiums[j].CurrencyId == data.CurrencyId &&
                    premiums[j].EndorsementNumber == data.EndorsementNumber && premiums[j].PayerId == data.PayerId &&
                    premiums[j].PayerDocumentNumber == data.PayerDocumentNumber && premiums[j].PayerName == data.PayerName &&
                    premiums[j].PolicyId == data.PolicyId && premiums[j].PolicyNumber == data.PolicyNumber &&
                    premiums[j].QuotaNumber == data.QuotaNumber && premiums[j].QuotaValue == data.QuotaValue) {
                    existPremium = 1;
                    break;
                }
            }
        }

        if (existPremium == 0) {

            var rowPremiumModel = new RowPremiumModel();

            rowPremiumModel.BranchId = data.BranchId;
            rowPremiumModel.BranchName = data.BranchName;
            rowPremiumModel.CurrencyId = data.CurrencyId;
            rowPremiumModel.CurrencyName = data.CurrencyName;
            rowPremiumModel.EndorsementNumber = data.EndorsementNumber;
            rowPremiumModel.PayerDocumentNumber = data.PayerDocumentNumber;
            rowPremiumModel.PayerId = data.PayerId;
            rowPremiumModel.PayerName = data.PayerName;
            rowPremiumModel.PolicyId = data.PolicyId;
            rowPremiumModel.PolicyNumber = data.PolicyNumber;
            rowPremiumModel.QuotaNumber = data.QuotaNumber;
            rowPremiumModel.QuotaValue = data.QuotaValue;
            rowPremiumModel.ItemId = data.ItemId;
            rowPremiumModel.EndorsementId = data.EndorsementId;
            rowPremiumModel.DocumentType = data.DocumentType;
            rowPremiumModel.BillId = data.BillId;
            rowPremiumModel.ItemTypeId = data.ItemTypeId;
            rowPremiumModel.Reference = data.DocumentType + " No: " + data.PayerDocumentNumber + " Endoso No:" + data.EndorsementNumber + " Cuota No:" + data.QuotaNumber;
            rowPremiumModel.Amount = data.QuotaValue;
            rowPremiumModel.IndividualId = data.IndividualId;
            rowPremiumModel.ExchangeRate = data.ExchangeRate;
            rowPremiumModel.PrefixName = data.PrefixName;
            totalPremium += data.QuotaValue;

            $("#policiesListView").UifListView("addItem", rowPremiumModel);
            $("#modalPremiumAdd").find('#TotalPremium').text(FormatMoneySymbol(totalPremium));
            SetPremiumsSummary();
            editPremium = index;
        }
        else {
            $("#modalPremiumAdd").find("#alertForm").UifAlert('show', Resources.MessageValidatorQuotaAdded, "warning");
        }
    }

    setTimeout(function () {
        $("#modalPremiumAdd").find("#alertForm").hide();
        SetTotalApplicationPayment();
    }, 3000);
});

$("#modalPremiumAdd").find('#premiumsListView').on('rowDelete', function (event, data) {
    SetTotalPayment("C");
    SetTotalApplicationPayment();
});

////AUTOCOMPLETES ASEGURADO DOCUMENTO
$("#modalPremiumAdd").find('#InsuredDocumentNumberBill').on('itemSelected', function (event, selectedItem) {
    insuredId = selectedItem.Id;
    $("#modalPremiumAdd").find('#SearchBy').val($("#modalPremiumAdd").find("#InsuredDocumentNumberBill").val());
});

//////////////////////////////////
// Botón buscar modal - primas ///
//////////////////////////////////
$("#modalPremiumAdd").find('#Search').click(function () {
    $("#modalPremiumAdd").find("#alertForm").hide();

    $("#addPremiumForm").validate();

    if ($("#modalPremiumAdd").find('#SearchBy').val().trim() == "") {
        $("#modalPremiumAdd").find('#SearchBy').val("");
        $("#addPremiumForm").valid();

    } else {

        //Se valida que ingrese los criterios de búsqueda
        if ($("#addPremiumForm").valid()) {
            var policyNumber = "";
            var documentNumber = "";
            var payerName = "";
            var prefix = "";
            var branch = "";
            var holderDocumentNumber = "";
            var holderName = "";

            if ($("#modalPremiumAdd").find("#selectSearch").val() == "1") {
                policyNumber = $("#modalPremiumAdd").find("#SearchBy").val();
            }
            if ($("#modalPremiumAdd").find("#selectSearch").val() == "2") {

                holderDocumentNumber = insuredId;
            }
            if ($("#modalPremiumAdd").find("#selectSearch").val() == "3") {
                documentNumber = $("#modalPremiumAdd").find("#SearchBy").val();
            }
            if ($("#modalPremiumAdd").find("#selectSearch").val() == "4") {
                payerName = $("#modalPremiumAdd").find("#SearchBy").val();
            }
            if ($("#modalPremiumAdd").find("#selectSearch").val() == "5") {
                holderName = $("#modalPremiumAdd").find("#SearchBy").val();
            }
            if ($("#modalPremiumAdd").find("#selectBranch").val() != "") {
                branch = $("#modalPremiumAdd").find("#selectBranch").val();
            }
            if ($("#modalPremiumAdd").find("#selectPrefix").val() != "") {
                prefix = $("#modalPremiumAdd").find("#selectPrefix").val();
            }

            var controller = ACC_ROOT + "Billing/GetPolicyQuotaListView?policyNumber=" + policyNumber + "&documentNumber=" + documentNumber +
                "&payerName=" + payerName + "&prefix=" + prefix + "&branch=" + branch + "&holderDocumentNumber=" +
                holderDocumentNumber + "&holderName=" + holderName;


            $("#modalPremiumAdd").find("#premiumsListView").UifListView(
                {
                    autoHeight: true,
                    source: controller,
                    customDelete: false,
                    customAdd: false,
                    customEdit: true,
                    edit: true,
                    displayTemplate: "#premiums-template"
                });
            //$("#addPremiumForm").formReset();
            $("#modalPremiumAdd").find("#TitleSearchBy").text("");
        }
    }


});

//////////////////////////
// Botón agregar modal ///
//////////////////////////
$("#modalPremiumAdd").find('#PremiumAdd').click(function () {
    paymentMethod = "C";
    var existCheck = 0;
    $("#addPremiumForm").validate();
    if ($("#addPremiumForm").valid()) {

        if (ValidateAddFormMain(paymentMethod) == true) {
            // Se valida que no se ingrese el mismo registro
            var keyValid = $("#addPremiumForm").find("#CheckDocumentNumber").val() + issuingBankId +
                $("#addPremiumForm").find("#CheckAccountNumber").val();

            var checks = $("#modalPremiumAdd").find('#premiumsListView').UifListView("getData");

            if (checks != null) {
                for (var j = 0; j < checks.length; j++) {
                    var checkIndex = checks[j].DocumentNumber + checks[j].IssuingBankId + checks[j].IssuingBankAccountNumber;
                    if (checkIndex == keyValid) {
                        existCheck = 1;
                        break;
                    }
                }
            }

            if (editCheck > -1) {
                if (($("#addPremiumForm").find("#CheckDocumentNumber").val() == $("#addPremiumForm").find("#HiddenCheckDocumentNumber").val()) &&
                    ($("#addPremiumForm").find("#CheckAccountNumber").val() == $("#addPremiumForm").find("#HiddenCheckAccountNumber").val()) &&
                    (issuingBankId == $("#addPremiumForm").find("#HiddenIssuingBankId").val())) {
                    existCheck = 0;
                }
            }

            if (existCheck == 0) {
                var rowModel = new RowPaymentModel();

                rowModel.PaymentTypeId = $("#ViewBagParamPaymentMethodCurrentCheck").val();
                rowModel.PaymentTypeDescription = "CHEQUE";
                rowModel.CurrencyId = $("#addPremiumForm").find('#selectCheckCurrency').val();
                rowModel.Currency = $("#addPremiumForm").find('#selectCheckCurrency option:selected').text();
                rowModel.Amount = $("#addPremiumForm").find('#CheckAmount').val();
                rowModel.ExchangeRate = $("#addPremiumForm").find("#CheckExchangeRate").val();
                rowModel.LocalAmount = $("#addPremiumForm").find("#CheckLocalAmount").val();
                rowModel.DocumentNumber = $("#addPremiumForm").find("#CheckDocumentNumber").val();
                rowModel.VoucherNumber = "";
                rowModel.CardNumber = "";
                rowModel.CardType = "";
                rowModel.CardTypeName = "";
                rowModel.AuthorizationNumber = "";
                rowModel.CardsName = "";
                rowModel.ValidThruMonth = "";
                rowModel.ValidThruYear = "";
                rowModel.IssuingBankId = issuingBankId;
                rowModel.IssuingBankName = $("#addPremiumForm").find("#inputCheckIssuingBank").val();
                rowModel.IssuingBankAccountNumber = $("#addPremiumForm").find("#CheckAccountNumber").val();
                rowModel.IssuerName = $("#addPremiumForm").find("#CheckHolderName").val();
                rowModel.RecievingBankId = "";
                rowModel.RecievingBankName = "";
                rowModel.RecievingBankAccountNumber = "";
                rowModel.SerialVoucher = "";
                rowModel.SerialNumber = "";
                rowModel.Date = $("#addPremiumForm").find("#CheckDate").val();
                rowModel.TaxBase = "";
                rowModel.Information = "";

                $("#modalPremiumAdd").find('#TotalCheck').text(rowModel.Amount);

                if (editCheck == -1) {
                    $("#modalPremiumAdd").find('#premiumsListView').UifListView("addItem", rowModel);
                }
                else {
                    $("#modalPremiumAdd").find('#premiumsListView').UifListView("editItem", editCheck, rowModel);
                    editCheck = -1;
                }
                $("#addPremiumForm").formReset();
                SetTotalPayment("C");
            }
            else {
                $("#modalPremiumAdd").find("#alertForm").UifAlert('show', Resources.ValidateDuplicatePaymentMethods, "warning");
            }
        }
        SetTotalApplicationPayment();
        SetMovementsSummary("C");

        setTimeout(function () {
            $("#modalPremiumAdd").find("#alertForm").hide();
        }, 3000);
    }

});

//////////////////////////////////////
// Botón Otros                      //
//////////////////////////////////////
$('#OpenOthers').click(function () {
    $.UifNotify('show', { type: 'info', message: Resources.NotImplemented, autoclose: true });

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/RevertionJournalEntry",
        data: {
            "technicalTransaction": 400025,
            "technicalTransactionRevertion": 400030,
        },
        success: function (data) {
            if (data > 0) {
                isValid = false;
            } else {
                isValid = true;
            }
        }
    });
});

$("#modalCardAdd").find('#selectCreditCardValidThruMonth').on('itemSelected', function (event, selectedItem) {
    if (selectedItem.Id > 0) {
        CheckExpirationDate();
    }
});
/*--------------------------------------------------------------------------------------------------------------------------*/
/*                                                 DEFINICION DE FUNCIONES                                                  */
/*--------------------------------------------------------------------------------------------------------------------------*/
function CleanAllFields() {
    $('#inputDocumentNumber').val('');
    $('#inputName').val('');
}

//////////////////////////////////////////////////
// Da formato a un número para su visualización //
//////////////////////////////////////////////////
function NumberFormatMainBilling(number, decimals, decimalSeparator, thousandsSeparator) {
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

///////////////////////////////////////////
// Setear la tasa de cambio de la moneda //
///////////////////////////////////////////
function SetCurrencyMainBilling(source, destination, amountFieldId, localAmountFieldId, modalId) {

    var selectCurrency = $("#" + source).UifSelect("getSelected");
    var textExchangeRate = $("#" + destination);

    if (selectCurrency >= 0) {
        var response = GetCurrencyRateBilling($("#ViewBagDateAccounting").val(), selectCurrency);
        currentRate = RemoveFormatMoney(response[0], 6);
        ValidateExchangeRate(destination, source, amountFieldId, localAmountFieldId, modalId, false);

        if (response[1] == false) {
            $("#alert").UifAlert('show', Resources.ExchageRateNotUpToDate, "warning");
        }
    }
    else {
        textExchangeRate.val("");
    }
}

///////////////////////////////////////////////////////////
// Obtiene la tasa de cambio de la moneda                //
///////////////////////////////////////////////////////////
function GetCurrencyRateBilling(accountingDate, currencyId) {
    var alert = true;
    var rate;
    var response = new Array();

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/GetCurrencyExchangeRate",
        data: {
            "rateDate": accountingDate,
            "currencyId": currencyId
        },
        success: function (data) {
            if (data == 1 || data == 0) {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "Billing/GetLatestCurrencyExchangeRate",
                    data: {
                        "rateDate": accountingDate,
                        "currencyId": currencyId
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
            }
            else {
                rate = data;
                alert = true;
            }
        }
    });

    response[0] = rate;
    response[1] = alert;
    exchangeRate = rate;

    return response;
}

//////////////////////////////////////////////////
// Setea el valor del importe local en efectivo //
//////////////////////////////////////////////////
function SetCashLocalAmount() {
    var cashAmount = RemoveFormatMoney($('#modalCashAdd').find('#CashAmount').val());
    var exchangeRate = RemoveFormatMoney($('#modalCashAdd').find('#CashExchangeRate').val(), 6);

    var cashLocalAmount = decimalAdjust('round', (cashAmount * exchangeRate), -3);
    $("#modalCashAdd").find("#CashLocalAmount").val(FormatMoneySymbol(cashLocalAmount));
}

////////////////////////////////////////////////////////////
// Setea el valor del importe local en todos los modales  //
////////////////////////////////////////////////////////////
function SetLocalAmount(controlAmount, exchangecontrol, localAmountControl, modalControlName) {

    let checkAmount = RemoveFormatMoney($("#" + modalControlName).find("#" + controlAmount).val());
    let exchangeRate = RemoveFormatMoney($("#" + modalControlName).find("#" + exchangecontrol).val(), 6);

    let checkLocalAmount = decimalAdjust('round', (checkAmount * exchangeRate), -3);
    $("#" + modalControlName).find("#" + localAmountControl).val(FormatMoneySymbol(checkLocalAmount));

}

//////////////////////////////////////////////////
// Setea el valor del importe local en cheque   //
//////////////////////////////////////////////////
function SetCheckLocalAmount() {

    let checkAmount = RemoveFormatMoney($("#modalCheckAdd").find("#CheckAmount").val());
    let exchangeRate = RemoveFormatMoney($("#modalCheckAdd").find("#CheckExchangeRate").val(), 6);

    let checkLocalAmount = checkAmount * exchangeRate;
    $("#modalCheckAdd").find("#CheckLocalAmount").val(FormatMoneySymbol(checkLocalAmount));

}

////////////////////////////////////////////////////////////
// Setea el valor del importe local en tarjeta de crédito //
////////////////////////////////////////////////////////////
function SetCreditCardLocalAmount() {
    var creditCardAmount = RemoveFormatMoney($('#modalCardAdd').find('#CreditCardAmount').val());
    var exchangeRate = RemoveFormatMoney($('#modalCardAdd').find('#CreditCardExchangeRate').val(), 6);

    var creditCardLocalAmount = decimalAdjust('round', (creditCardAmount * exchangeRate), -3);
    $("#modalCardAdd").find("#CreditCardLocalAmount").val(FormatMoneySymbol(creditCardLocalAmount));
}

//////////////////////////////////////////////////////
// Setea el valor del importe local en consignación //
//////////////////////////////////////////////////////
function SetConsignmentLocalAmount() {
    var consignmentAmount = RemoveFormatMoney($('#modalConsignmentAdd').find('#ConsignmentAmount').val());
    var exchangeRate = RemoveFormatMoney($('#modalConsignmentAdd').find('#ConsignmentExchangeRate').val(), 6);

    var consignmentLocalAmount = decimalAdjust('round', (consignmentAmount * exchangeRate), -3);
    $("#modalConsignmentAdd").find("#ConsignmentLocalAmount").val(FormatMoneySymbol(consignmentLocalAmount));
}

/////////////////////////////////////////////////////////////
// Setea el valor del importe local en consignación cheques//
////////////////////////////////////////////////////////////
function SetConsignmentCheckLocalAmount() {
    var consignmentCheckAmount = RemoveFormatMoney($('#modalConsignmentCheckAdd').find('#ConsignmentCheckAmount').val());
    var exchangeRate = RemoveFormatMoney($('#modalConsignmentCheckAdd').find('#ConsignmentCheckExchangeRate').val(), 6);

    var consignmentCheckLocalAmount = decimalAdjust('round', (consignmentCheckAmount * exchangeRate), -3);
    $("#modalConsignmentCheckAdd").find("#ConsignmentCheckLocalAmount").val(FormatMoneySymbol(consignmentCheckLocalAmount));
}

//////////////////////////////////////////////////
// Setea el valor del importe local en datafono //
//////////////////////////////////////////////////
function SetDatafonoLocalAmount() {
    var datafanoAmount = RemoveFormatMoney($('#modalDatafonoAdd').find('#DatafonoAmount').val());
    var exchangeRate = RemoveFormatMoney($('#modalDatafonoAdd').find('#DatafonoExchangeRate').val(), 6);

    var datafanoLocalAmount = decimalAdjust('round', (datafanoAmount * exchangeRate), -3);
    $("#modalDatafonoAdd").find("#DatafonoLocalAmount").val(FormatMoneySymbol(datafanoLocalAmount));
}

/////////////////////////////////////////////////////////
// Setea el valor del importe local en transferencia   //
/////////////////////////////////////////////////////////
function SetTransferLocalAmount() {
    var transferAmount = RemoveFormatMoney($('#modalTransferAdd').find('#TransferAmount').val());
    var exchangeRate = RemoveFormatMoney($('#modalTransferAdd').find('#TransferExchangeRate').val(), 6);

    var transferLocalAmount = decimalAdjust('round', (transferAmount * exchangeRate), -3);
    $("#modalTransferAdd").find("#TransferLocalAmount").val(FormatMoneySymbol(transferLocalAmount));
}

/////////////////////////////////////////////////////////
// Setea el valor del importe local en retenciones     //
/////////////////////////////////////////////////////////
function SetRetentionLocalAmount() {
    var retentionAmount = RemoveFormatMoney($('#modalRetentionAdd').find('#RetentionAmount').val());
    var exchangeRate = RemoveFormatMoney($('#modalRetentionAdd').find('#RetentionExchangeRate').val(), 6);

    var retentionLocalAmount = decimalAdjust('round', (retentionAmount * exchangeRate), -3);
    $("#modalRetentionAdd").find("#RetentionLocalAmount").val(FormatMoneySymbol(retentionLocalAmount));
}

//////////////////////////////////////////////////
// Valida el ingreso de campos obligatorios     //
//////////////////////////////////////////////////
function ValidateAddFormMain(paymentMethod) {
    // Cash
    if (paymentMethod == "E") {
        if ($("#modalCashAdd").find('#selectCashCurrency').val() == "") {
            $("#modalCashAdd").find("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
            return false;
        }
        if ($("#modalCashAdd").find('#CashAmount').val() == "") {
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
        if ($("#modalCheckAdd").find('#CheckDate').val() == "") {
            $("#modalCheckAdd").find("#alertForm").UifAlert('show', Resources.DateRequired, "warning");
            return false;
        }
    }
    // CreditCard
    if (paymentMethod == "T") {
        if ($("#modalCardAdd").find('#selectCreditCardCurrency').val() == "") {
            $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
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

    // Consignment
    if (paymentMethod == "CH") {
        if ($("#modalConsignmentCheckAdd").find('#selectConsignmentCheckCurrency').val() == "") {
            $("#modalConsignmentCheckAdd").find("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
            return false;
        }
        if ($("#modalConsignmentCheckAdd").find('#ConsignmentCheckAmount').val() == "") {
            $("#modalConsignmentCheckAdd").find("#alertForm").UifAlert('show', Resources.AmountRequired, "warning");
            return false;
        }
        if ($("#modalConsignmentCheckAdd").find('#selectConsignmentCheckReceivingBank').val() == "") {
            $("#modalConsignmentCheckAdd").find("#alertForm").UifAlert('show', Resources.ReceivingBankRequired, "warning");
            return false;
        }
        if ($("#modalConsignmentCheckAdd").find('#selectConsignmentCheckAccountNumber').val() == "") {
            $("#modalConsignmentCheckAdd").find("#alertForm").UifAlert('show', Resources.AccountNumberRequired, "warning");
            return false;
        }
        if ($("#modalConsignmentCheckAdd").find('#ConsignmentCheckBallotNumber').val() == "") {
            $("#modalConsignmentCheckAdd").find("#alertForm").UifAlert('show', Resources.BallotNumberRequired, "warning");
            return false;
        }
        if ($("#modalConsignmentCheckAdd").find('#ConsignmentCheckDate').val() == "") {
            $("#modalConsignmentCheckAdd").find("#alertForm").UifAlert('show', Resources.DateRequired, "warning");
            return false;
        }
        if ($("#modalConsignmentCheckAdd").find('#ConsignmentCheckDepositorName').val() == "") {
            $("#modalConsignmentCheckAdd").find("#alertForm").UifAlert('show', Resources.DepositorNameRequired, "warning");
            return false;
        }
    }

    // Datafono
    if (paymentMethod == "D") {
        if ($("#modalDatafonoAdd").find('#selectDatafonoCurrency').val() == "") {
            $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
            return false;
        }
        if ($("#modalDatafonoAdd").find('#DatafonoAmount').val() == "") {
            $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.AmountRequired, "warning");
            return false;
        }
        if ($("#modalDatafonoAdd").find('#selectDatafonoCreditCardType').val() == "") {
            $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.ConduitRequired, "warning");
            return false;
        }
        if ($("#modalDatafonoAdd").find('#inputDatafonoIssuingBank').val() == "") {
            $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.IssuingBankRequired, "warning");
            return false;
        }
        if ($("#modalDatafonoAdd").find('#DatafonoCreditCardNumber').val() == "") {
            $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.CreditCardNumberRequired, "warning");
            return false;
        }
        if ($("#modalDatafonoAdd").find('#DatadfonoAuthorizationNumber').val() == "") {
            $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.AuthorizationNumberRequired, "warning");
            return false;
        }
        if ($("#modalDatafonoAdd").find('#DatafonoVoucherNumber').val() == "") {
            $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.VoucherNumberRequired, "warning");
            return false;
        }
        if ($("#modalDatafonoAdd").find('#DatafonoHolderName').val() == "") {
            $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.NameRequired, "warning");
            return false;
        }
        if ($("#modalDatafonoAdd").find('#selectDatafonoCreditCardValidThruMonth').val() == "") {
            $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.MonthRequired, "warning");
            return false;
        }
        if ($("#modalDatafonoAdd").find('#DatafonoCreditCardValidThruYear').val() == "") {
            $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.YearRequired, "warning");
            return false;
        }
        if ($("#modalDatafonoAdd").find('#DatafonoTaxBase').val() == "") {
            $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.TaxBaseRequired, "warning");
            return false;
        }
    }

    // Transfer
    if (paymentMethod == "TR") {
        if ($("#modalTransferAdd").find('#selectTransferCurrency').val() == "") {
            $("#modalTransferAdd").find("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
            return false;
        }
        if ($("#modalTransferAdd").find('#TransferAmount').val() == "") {
            $("#modalTransferAdd").find("#alertForm").UifAlert('show', Resources.AmountRequired, "warning");
            return false;
        }
        if ($("#modalTransferAdd").find('#selectTransferReceivingBank').val() == "") {
            $("#modalTransferAdd").find("#alertForm").UifAlert('show', Resources.ReceivingBankRequired, "warning");
            return false;
        }
        if ($("#modalTransferAdd").find('#selectTransferAccountNumber').val() == "") {
            $("#modalTransferAdd").find("#alertForm").UifAlert('show', Resources.AccountNumberRequired, "warning");
            return false;
        }
        if ($("#modalTransferAdd").find('#TransferDate').val() == "") {
            $("#modalTransferAdd").find("#alertForm").UifAlert('show', Resources.DateRequired, "warning");
            return false;
        }
        if ($("#modalTransferAdd").find('#TransferHolderName').val() == "") {
            $("#modalTransferAdd").find("#alertForm").UifAlert('show', Resources.DepositorNameRequired, "warning");
            return false;
        }
    }

    // Retention
    if (paymentMethod == "R") {
        if ($("#modalRetentionAdd").find('#selectRetentionCurrency').val() == "") {
            $("#modalRetentionAdd").find("#alertForm").UifAlert('show', Resources.SelectCurrency, "warning");
            return false;
        }
        if ($("#modalRetentionAdd").find('#RetentionAmount').val() == "") {
            $("#modalRetentionAdd").find("#alertForm").UifAlert('show', Resources.AmountRequired, "warning");
            return false;
        }

        if ($("#modalRetentionAdd").find('#RetentionBillNumber').val() == "") {
            $("#modalRetentionAdd").find("#alertForm").UifAlert('show', Resources.AccountNumberRequired, "warning");
            return false;
        }
        if ($("#modalRetentionAdd").find('#RetentionBillDate').val() == "") {
            $("#modalRetentionAdd").find("#alertForm").UifAlert('show', Resources.BillDateRequired, "warning");
            return false;
        }
        if ($("#modalRetentionAdd").find('#RetentionAutorizationNumber').val() == "") {
            $("#modalRetentionAdd").find("#alertForm").UifAlert('show', Resources.DateRequired, "warning");
            return false;
        }
        if ($("#modalRetentionAdd").find('#RetentionVoucherNumber').val() == "") {
            $("#modalRetentionAdd").find("#alertForm").UifAlert('show', Resources.DepositorNameRequired, "warning");
            return false;
        }

        if ($("#modalRetentionAdd").find('#RetentionIssueDate').val() == "") {
            $("#modalRetentionAdd").find("#alertForm").UifAlert('show', Resources.IssueDateRequired, "warning");
            return false;
        }
        if ($("#modalRetentionAdd").find('#RetentionExpirationDate').val() == "") {
            $("#modalRetentionAdd").find("#alertForm").UifAlert('show', Resources.ExpirationDateRequired, "warning");
            return false;
        }
    }

    return true;
}

//////////////////////////////////////////////////////////////////
// Valida que no se ingrese el mismo número de voucher Datafono //
/////////////////////////////////////////////////////////////////
function ValidDatafonoVoucherNumber() {
    var isValid = false;
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/ValidateVoucher",
        data: {
            "creditCardNumber": $("#modalDatafonoAdd").find("#DatafonoCreditCardNumber").val(),
            "voucherNumber": $("#modalDatafonoAdd").find("#DatafonoVoucherNumber").val(),
            "conduitType": $("#modalDatafonoAdd").find("#selectDatafonoCreditCardType").val()
        },
        success: function (data) {
            if (data > 0) {
                isValid = false;
            } else {
                isValid = true;
            }
        }
    });
    return isValid;
}

/////////////////////////////////////////////////////////
// Valida que no se ingrese el mismo número de voucher //
/////////////////////////////////////////////////////////
function ValidCreditCardVoucherNumber() {
    var isValid = false;
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/ValidateVoucher",
        data: {
            "creditCardNumber": $("#modalCardAdd").find("#CreditCardNumber").val(),
            "voucherNumber": $("#modalCardAdd").find("#CreditCardVoucherNumber").val(),
            "conduitType": $("#modalCardAdd").find("#selectCreditCardType").val()
        },
        success: function (data) {
            if (data > 0) {
                isValid = false;
            } else {
                isValid = true;
            }
        }
    });
    return isValid;
}

///////////////////////////////////////////////////////////////
// Valida que no se ingrese el mismo número de transferencia //
///////////////////////////////////////////////////////////////
function ValidTransferDocumentNumber() {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/ValidateCheckBankOrTransfer",
        data: {
            "bankId": TransferIssuingBankId,
            "documentNumber": $("#TransferDocumentNumber").val(),
            "accountNumber": $("#TransferIssuingAccountNumber").val()
        },
        success: function (data) {
            if (data > 0) {
                return false;
            } else {
                return true;
            }
        }
    });
}

///////////////////////////////////////////////////////////////
// Valida que no se ingrese el mismo número de documento     //
///////////////////////////////////////////////////////////////
function ValidDepositVoucherDocumentNumber() {
    var voucherFound = 0;

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/ValidateDepositVoucher",
        data: {
            "bankId": $("#modalConsignmentAdd").find("#selectConsignmentReceivingBank").val(),
            "documentNumber": $("#modalConsignmentAdd").find("#ConsignmentBallotNumber").val(),
            "accountNumber": $("#modalConsignmentAdd").find("#selectConsignmentAccountNumber option:selected").text()
        },
        success: function (data) {
            if (data > 0) {
                voucherFound = 1;
            } else {
                voucherFound = 0;
            }
        }
    });

    return voucherFound;
}

///////////////////////////////////////////////////////////////
// Valida que no se ingrese el mismo número de documento     //
///////////////////////////////////////////////////////////////
function ValidDepositCheckVoucherDocumentNumber() {
    return $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/ValidateDepositVoucher",
        data: {
            "bankId": $('#HiddenConsignmentIssuingBankId').val(),
            "documentNumber": $("#modalConsignmentCheckAdd").find("#ConsignmentCheckDocumentNumber").val(),
            "accountNumber": $("#modalConsignmentCheckAdd").find("#ConsignmentCheckAccountNumber").val()
        }
    });
}

///////////////////////////////////////////////////////////////
// Valida que no se ingrese el mismo número de cheque        //
///////////////////////////////////////////////////////////////
function ValidCheckDocumentNumber() {
    var checkFound = 0;

    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/ValidateCheckBankOrTransfer",
        data: {
            "bankId": issuingBankId,
            "numberDoc": $("#modalCheckAdd").find("#CheckDocumentNumber").val(),
            "accountNumber": $("#modalCheckAdd").find("#CheckAccountNumber").val()
        },
        success: function (data) {
            if (data > 0) {
                checkFound = 1;
            } else {
                checkFound = 0;
            }
        }
    });

    return checkFound;
}

///////////////////////////////////////////////////////
// Setear el total de la listview del método de pago //
///////////////////////////////////////////////////////
function SetTotalPayment(paymentMethod) {
    totalPolicy = 0;
    // Cash
    if (paymentMethod == "E") {
        var cashs = $("#modalCashAdd").find("#cashsListView").UifListView("getData");

        if (cashs != null) {

            for (var j = 0; j < cashs.length; j++) {
                var cashAmount = RemoveFormatMoney(cashs[j].LocalAmount);
                totalPolicy += parseFloat(cashAmount);
            }
        }
        else {
            $("#modalCashAdd").find$("#TotalCash").text("");
        }
        $("#modalCashAdd").find("#TotalCash").text(FormatMoneySymbol(totalPolicy));
    }

    // Check
    if (paymentMethod == "C") {
        var checks = $("#modalCheckAdd").find('#checksListView').UifListView("getData");

        if (checks != null) {

            for (var j = 0; j < checks.length; j++) {
                totalPolicy += RemoveFormatMoney(checks[j].LocalAmount);
            }
        }
        else {
            $("#modalCheckAdd").find$("#TotalCheck").text("");
        }
        $("#modalCheckAdd").find("#TotalCheck").text(FormatMoneySymbol(totalPolicy));
    }

    // Credit Card
    if (paymentMethod == "T") {
        var cards = $("#modalCardAdd").find('#cardsListView').UifListView("getData");

        if (cards != null) {

            for (var j = 0; j < cards.length; j++) {
                var cardAmount = RemoveFormatMoney(cards[j].LocalAmount);
                totalPolicy += parseFloat(cardAmount);
            }
        }
        else {
            $("#modalCardAdd").find$("#TotalCard").text("");
        }
        $("#modalCardAdd").find("#TotalCard").text(FormatMoneySymbol(totalPolicy));
    }

    // Consignment
    if (paymentMethod == "B") {
        var consignments = $("#modalConsignmentAdd").find('#consignmentsListView').UifListView("getData");

        if (consignments != null) {

            for (var j = 0; j < consignments.length; j++) {
                var consignmentAmount = RemoveFormatMoney(consignments[j].LocalAmount);
                totalPolicy += parseFloat(consignmentAmount);
            }
        }
        else {
            $("#modalConsignmentAdd").find$("#TotalConsignment").text("");
        }
        $("#modalConsignmentAdd").find("#TotalConsignment").text(FormatMoneySymbol(totalPolicy));
    }

    //ConsignmentCheck
    if (paymentMethod == "CH") {
        var consignmentsCheck = $("#modalConsignmentCheckAdd").find('#ConsignmentChecksListView').UifListView("getData");

        if (consignmentsCheck != null) {

            for (var j = 0; j < consignmentsCheck.length; j++) {
                var consignmentCheckAmount = RemoveFormatMoney(consignmentsCheck[j].LocalAmount);
                totalPolicy += parseFloat(consignmentCheckAmount);
            }
        }
        else {
            $("#modalConsignmentCheckAdd").find$("#TotalConsignmentCheck").text("");
        }
        $("#modalConsignmentCheckAdd").find("#TotalConsignmentCheck").text(FormatMoneySymbol(totalPolicy));
    }

    // Datafono
    if (paymentMethod == "D") {
        var datafono = $("#modalDatafonoAdd").find("#datafonoListView").UifListView("getData");

        if (datafono != null) {

            for (var j = 0; j < datafono.length; j++) {
                var datafonoAmount = RemoveFormatMoney(datafono[j].LocalAmount);
                totalPolicy += parseFloat(datafonoAmount);
            }
        }
        else {
            $("#modalDatafonoAdd").find$("#TotalDatafono").text("");
        }
        $("#modalDatafonoAdd").find("#TotalDatafono").text(FormatMoneySymbol(totalPolicy));
    }

    // Transfer
    if (paymentMethod == "TR") {
        var transfers = $("#modalTransferAdd").find('#transferListView').UifListView("getData");

        if (transfers != null) {

            for (var j = 0; j < transfers.length; j++) {
                var transferAmount = RemoveFormatMoney(transfers[j].LocalAmount);
                totalPolicy += parseFloat(transferAmount);
            }
        }
        else {
            $("#modalTransferAdd").find$("#TotalTransfer").text("");
        }
        $("#modalTransferAdd").find("#TotalTransfer").text(FormatMoneySymbol(totalPolicy));
    }

    // Retenciones
    if (paymentMethod == "R") {
        var retention = $("#modalRetentionAdd").find('#retentionListView').UifListView("getData");

        if (retention != null) {

            for (var j = 0; j < retention.length; j++) {
                var retentionAmount = RemoveFormatMoney(retention[j].LocalAmount);
                totalPolicy += parseFloat(retentionAmount);
            }
        }
        else {
            $("#modalRetentionAdd").find$("#TotalRetention").text("");
        }
        $("#modalRetentionAdd").find("#TotalRetention").text(FormatMoneySymbol(totalPolicy));
    }

}

/////////////////////////////////////////////////////////
// Setear el total de la aplicación del método de pago //
/////////////////////////////////////////////////////////
function SetTotalApplicationPayment() {
    var totalCash = 0;
    var totalCheck = 0;
    var totalCreditCard = 0;
    var totalConsignment = 0;
    var totalConsignmentCheck = 0;
    var totalDatafono = 0;
    var totalTransfer = 0;
    var totalRetention = 0;

    totalReceipt = 0;

    // Cash
    totalCash = RemoveFormatMoney($("#modalCashAdd").find("#TotalCash").text());

    // Check
    totalCheck = RemoveFormatMoney($("#modalCheckAdd").find("#TotalCheck").text());

    // Credit Card
    totalCreditCard = RemoveFormatMoney($("#modalCardAdd").find("#TotalCard").text());

    // Consignment
    totalConsignment = RemoveFormatMoney($("#modalConsignmentAdd").find("#TotalConsignment").text());

    // ConsignmentCheck
    totalConsignmentCheck = RemoveFormatMoney($("#modalConsignmentCheckAdd").find("#TotalConsignmentCheck").text());

    // Datafono
    totalDatafono = RemoveFormatMoney($("#modalDatafonoAdd").find("#TotalDatafono").text());

    // Transfer
    totalTransfer = RemoveFormatMoney($("#modalTransferAdd").find("#TotalTransfer").text());

    // Retenciones
    totalRetention = RemoveFormatMoney($("#modalRetentionAdd").find("#TotalRetention").text());

    totalReceipt = totalCash + totalCheck + totalCreditCard + totalConsignment + totalTransfer + totalDatafono + totalRetention + totalConsignmentCheck;
    difference = RemoveFormatMoney($("#TotalApplication").val()) - totalReceipt;

    difference = difference.toFixed(2);
    $("#Difference").text(FormatMoneySymbol(difference));
    $("#TotalReceipt").text(FormatMoneySymbol(totalReceipt));
}

////////////////////////////////////////////////////////
// Setear el método de pago y el total en la listview //
////////////////////////////////////////////////////////
function SetMovementsSummary(paymentMethod) {
    var rowSummaryModel = new RowModel();
    var currencyId = -1;
    var currency = "";
    var amount = 0;
    var exchangeRate = 0;
    var localAmount = 0;
    var j;

    $("#summariesListView").UifListView({
        height: '40%', theme: 'dark',
        customAdd: false, customEdit: false, customDelete: false, localMode: true, add: false, edit: false, delete: true,
        displayTemplate: "#summaries-template",
        deleteCallback: deleteSummaryCashCallback
    });


    // Cash
    var cashs = $("#modalCashAdd").find('#cashsListView').UifListView("getData");

    if (cashs != null) {
        for (var j = 0; j < cashs.length; j++) {
            var cashAmount = RemoveFormatMoney(cashs[j].Amount);
            amount += parseFloat(cashAmount);
            var cashLocalAmount = RemoveFormatMoney(cashs[j].LocalAmount);
            localAmount += parseFloat(cashLocalAmount);
            var cashExchangeRate = RemoveFormatMoney(cashs[j].ExchangeRate, 6);
            exchangeRate = parseFloat(cashExchangeRate);
            currencyId = cashs[j].CurrencyId;
            currency = cashs[j].Currency;
        }
        if (cashs.length > 0) {
            rowSummaryModel.PaymentMethod = "Efectivo";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            $("#summariesListView").UifListView("addItem", rowSummaryModel);
        }
    }

    // Check
    amount = 0;
    localAmount = 0;
    exchangeRate = 0;
    currencyId = -1;
    currency = "";

    var checks = $("#modalCheckAdd").find('#checksListView').UifListView("getData");

    if (checks != null) {
        for (var j = 0; j < checks.length; j++) {
            var checkAmount = RemoveFormatMoney(checks[j].Amount);
            amount += parseFloat(checkAmount);
            var checkLocalAmount = RemoveFormatMoney(checks[j].LocalAmount);
            localAmount += parseFloat(checkLocalAmount);
            var checkExchangeRate = RemoveFormatMoney(checks[j].ExchangeRate);
            exchangeRate = parseFloat(checkExchangeRate);
            currencyId = checks[j].CurrencyId;
            currency = checks[j].Currency;
        }
        if (checks.length > 0) {
            rowSummaryModel.PaymentMethod = "Cheque";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            $("#summariesListView").UifListView("addItem", rowSummaryModel);
        }
    }

    // Credit Card
    amount = 0;
    localAmount = 0;
    exchangeRate = 0;
    currencyId = -1;
    currency = "";
    var cards = $("#modalCardAdd").find('#cardsListView').UifListView("getData");

    if (cards != null) {
        for (var j = 0; j < cards.length; j++) {
            var cardAmount = RemoveFormatMoney(cards[j].Amount);
            amount += parseFloat(cardAmount);
            var cardLocalAmount = RemoveFormatMoney(cards[j].LocalAmount);
            localAmount += parseFloat(cardLocalAmount);
            var cardExchangeRate = RemoveFormatMoney(cards[j].ExchangeRate);
            exchangeRate = parseFloat(cardExchangeRate);
            currencyId = cards[j].CurrencyId;
            currency = cards[j].Currency;
        }
        if (cards.length > 0) {
            rowSummaryModel.PaymentMethod = "Tarjeta";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            $("#summariesListView").UifListView("addItem", rowSummaryModel);
        }
    }


    // Consignment
    amount = 0;
    localAmount = 0;
    exchangeRate = 0;
    currencyId = -1;
    currency = "";
    var consignments = $("#modalConsignmentAdd").find('#consignmentsListView').UifListView("getData");

    if (consignments != null) {
        for (var j = 0; j < consignments.length; j++) {
            var consignmentAmount = RemoveFormatMoney(consignments[j].Amount);
            amount += parseFloat(consignmentAmount);
            var consignmentLocalAmount = RemoveFormatMoney(consignments[j].LocalAmount);
            localAmount += parseFloat(consignmentLocalAmount);
            var consignmentExchangeRate = RemoveFormatMoney(consignments[j].ExchangeRate);
            exchangeRate = parseFloat(consignmentExchangeRate);
            currencyId = consignments[j].CurrencyId;
            currency = consignments[j].Currency;
        }
        if (consignments.length > 0) {
            rowSummaryModel.PaymentMethod = "Consignación";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            $("#summariesListView").UifListView("addItem", rowSummaryModel);
        }
    }


    // ConsignmentCheck
    amount = 0;
    localAmount = 0;
    exchangeRate = 0;
    currencyId = -1;
    currency = "";
    var consignmentsCheck = $("#modalConsignmentCheckAdd").find('#ConsignmentChecksListView').UifListView("getData");

    if (consignmentsCheck != null) {
        for (var j = 0; j < consignmentsCheck.length; j++) {
            var consignmentCheckAmount = RemoveFormatMoney(consignmentsCheck[j].Amount);
            amount += parseFloat(consignmentCheckAmount);
            var consignmentCheckLocalAmount = RemoveFormatMoney(consignmentsCheck[j].LocalAmount);
            localAmount += parseFloat(consignmentCheckLocalAmount);
            var consignmentCheckExchangeRate = RemoveFormatMoney(consignmentsCheck[j].ExchangeRate);
            exchangeRate = parseFloat(consignmentCheckExchangeRate);
            currencyId = consignmentsCheck[j].CurrencyId;
            currency = consignmentsCheck[j].Currency;
        }
        if (consignmentsCheck.length > 0) {
            rowSummaryModel.PaymentMethod = "Consignación de Cheques";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            $("#summariesListView").UifListView("addItem", rowSummaryModel);
        }
    }

    // Transfer
    amount = 0;
    localAmount = 0;
    exchangeRate = 0;
    currencyId = -1;
    currency = "";

    var transfers = $("#modalTransferAdd").find('#transferListView').UifListView("getData");

    if (transfers != null) {
        for (var j = 0; j < transfers.length; j++) {
            var transferAmount = RemoveFormatMoney(transfers[j].Amount);
            amount += parseFloat(transferAmount);
            var transferLocalAmount = RemoveFormatMoney(transfers[j].LocalAmount);
            localAmount += parseFloat(transferLocalAmount);
            var transferExchangeRate = RemoveFormatMoney(transfers[j].ExchangeRate);
            exchangeRate = parseFloat(transferExchangeRate);
            currencyId = transfers[j].CurrencyId;
            currency = transfers[j].Currency;
        }
        if (transfers.length > 0) {
            rowSummaryModel.PaymentMethod = "Transferencia";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            $("#summariesListView").UifListView("addItem", rowSummaryModel);
        }
    }

    // Datafono
    amount = 0;
    localAmount = 0;
    exchangeRate = 0;
    currencyId = -1;
    currency = "";
    var datafono = $("#modalDatafonoAdd").find('#datafonoListView').UifListView("getData");

    if (datafono != null) {
        for (var j = 0; j < datafono.length; j++) {
            var datafonoAmount = RemoveFormatMoney(datafono[j].Amount);
            amount += parseFloat(datafonoAmount);
            var datafonoLocalAmount = RemoveFormatMoney(datafono[j].LocalAmount);
            localAmount += parseFloat(datafonoLocalAmount);
            var datafonoExchangeRate = RemoveFormatMoney(datafono[j].ExchangeRate);
            exchangeRate = parseFloat(datafonoExchangeRate);
            currencyId = datafono[j].CurrencyId;
            currency = datafono[j].Currency;
        }
        if (datafono.length > 0) {
            rowSummaryModel.PaymentMethod = "Datafono";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            $("#summariesListView").UifListView("addItem", rowSummaryModel);
        }
    }

    // Retenciones
    amount = 0;
    localAmount = 0;
    exchangeRate = 0;
    currencyId = -1;
    currency = "";
    var retention = $("#modalRetentionAdd").find('#retentionfonoListView').UifListView("getData");

    if (retention != null) {
        for (var j = 0; j < retention.length; j++) {
            var retentionAmount = RemoveFormatMoney(retention[j].Amount);
            amount += parseFloat(retentionAmount);
            var retentionLocalAmount = RemoveFormatMoney(retention[j].LocalAmount);
            localAmount += parseFloat(retentionLocalAmount);
            var retentionExchangeRate = RemoveFormatMoney(retention[j].ExchangeRate);
            exchangeRate = parseFloat(retentionExchangeRate);
            currencyId = retention[j].CurrencyId;
            currency = retention[j].Currency;
        }
        if (retention.length > 0) {
            rowSummaryModel.PaymentMethod = "Retenciones";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            $("#summariesListView").UifListView("addItem", rowSummaryModel);
        }
    }
}

/////////////////////////////////////
// Validacion de cambio de importe //
/////////////////////////////////////
function validateExchangeRateTolerance(newRate, exchangeControl, currencyControl, amountControl, localAmountControl, modalName, showError = true) {

    $("#" + exchangeControl).val(FormatMoneySymbol(newRate, 6));
    var idCurrency = $("#" + currencyControl).val();
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Billing/CalculateExchangeRateTolerance",
        data: {
            "newRate": ReplaceDecimalPoint(newRate),
            "currencyId": idCurrency
        },
        success: function (data) {
            if (data != true) {
                if (showError === true) {
                    $.UifNotify('show', {
                        'type': 'info', 'message': Resources.ValueIsNotRangeTolerance, 'autoclose': true
                    });
                }
                $("#" + exchangeControl).val(FormatMoneySymbol(currentRate, 6));
                SetLocalAmount(amountControl, exchangeControl, localAmountControl, modalName);
                return false;
            }
            else {
                currentRate = newRate;
                $("#" + exchangeControl).val(FormatMoneySymbol(newRate, 6));
                SetLocalAmount(amountControl, exchangeControl, localAmountControl, modalName);
                return true;
            }
        }
    });
}

////////////////////////////////////////////////////////    
// Setear el método de pago y el total en la listview //
////////////////////////////////////////////////////////
function SetMovementsSummaryOriginal(paymentMethod) {
    var rowSummaryModel = new RowModel();
    var currencyId = -1;
    var currency = "";
    var amount = 0;
    var exchangeRate = 0;
    var localAmount = 0;
    var j;
    $("#summariesListView").UifListView({
        height: '40%', theme: 'dark',
        customAdd: false, customEdit: false, customDelete: false, localMode: true, add: false, edit: false, delete: true,
        displayTemplate: "#summaries-template",
        deleteCallback: deleteSummaryCashCallback
    });

    // Cash
    if (paymentMethod == "E") {
        var cashs = $("#modalCashAdd").find('#cashsListView').UifListView("getData");

        if (cashs != null) {
            for (var j = 0; j < cashs.length; j++) {
                var cashAmount = RemoveFormatMoney(cashs[j].Amount);
                amount += parseFloat(cashAmount);
                var cashLocalAmount = RemoveFormatMoney(cashs[j].LocalAmount);
                localAmount += parseFloat(cashLocalAmount);
                var cashExchangeRate = RemoveFormatMoney(cashs[j].ExchangeRate);
                exchangeRate = parseFloat(cashExchangeRate);
                currencyId = cashs[j].CurrencyId;
                currency = cashs[j].Currency;
            }
        }
        else {
            amount = 0;
            localAmount = 0;
            exchangeRate = 0;
            currencyId = -1;
            currency = "";
        }
        rowSummaryModel.PaymentMethod = "Efectivo";
        rowSummaryModel.CurrencyId = currencyId;
        rowSummaryModel.CurrencyDescription = currency;
        rowSummaryModel.Amount = FormatMoneySymbol(amount);
        rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
        rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
    }

    // Check
    if (paymentMethod == "C") {
        var checks = $("#modalCheckAdd").find('#checksListView').UifListView("getData");

        if (checks != null) {
            for (var j = 0; j < checks.length; j++) {
                var checkAmount = RemoveFormatMoney(checks[j].Amount);
                amount += parseFloat(checkAmount);
                var checkLocalAmount = RemoveFormatMoney(checks[j].LocalAmount);
                localAmount += parseFloat(checkLocalAmount);
                var checkExchangeRate = RemoveFormatMoney(checks[j].ExchangeRate);
                exchangeRate = parseFloat(checkExchangeRate);
                currencyId = checks[j].CurrencyId;
                currency = checks[j].Currency;
            }
        }
        else {
            amount = 0;
            localAmount = 0;
            exchangeRate = 0;
            currencyId = -1;
            currency = "";
        }
        rowSummaryModel.PaymentMethod = "Cheque";
        rowSummaryModel.CurrencyId = currencyId;
        rowSummaryModel.CurrencyDescription = currency;
        rowSummaryModel.Amount = FormatMoneySymbol(amount);
        rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
        rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
    }

    // Credit Card
    if (paymentMethod == "T") {
        var cards = $("#modalCardAdd").find('#cardsListView').UifListView("getData");

        if (cards != null) {
            for (var j = 0; j < cards.length; j++) {
                var cardAmount = RemoveFormatMoney(cards[j].Amount);
                amount += parseFloat(cardAmount);
                var cardLocalAmount = RemoveFormatMoney(cards[j].LocalAmount);
                localAmount += parseFloat(cardLocalAmount);
                var cardExchangeRate = RemoveFormatMoney(cards[j].ExchangeRate);
                exchangeRate = parseFloat(cardExchangeRate);
                currencyId = cards[j].CurrencyId;
                currency = cards[j].Currency;
            }
        }
        else {
            amount = 0;
            localAmount = 0;
            exchangeRate = 0;
            currencyId = -1;
            currency = "";
        }
        rowSummaryModel.PaymentMethod = "Tarjeta";
        rowSummaryModel.CurrencyId = currencyId;
        rowSummaryModel.CurrencyDescription = currency;
        rowSummaryModel.Amount = FormatMoneySymbol(amount);
        rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
        rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
    }

    // Consignment
    if (paymentMethod == "B") {
        var consignments = $("#modalConsignmentAdd").find('#consignmentsListView').UifListView("getData");

        if (consignments != null) {
            for (var j = 0; j < consignments.length; j++) {
                var consignmentAmount = RemoveFormatMoney(consignments[j].Amount);
                amount += parseFloat(consignmentAmount);
                var consignmentLocalAmount = RemoveFormatMoney(consignments[j].LocalAmount);
                localAmount += parseFloat(consignmentLocalAmount);
                var consignmentExchangeRate = RemoveFormatMoney(consignments[j].ExchangeRate);
                exchangeRate = parseFloat(consignmentExchangeRate);
                currencyId = consignments[j].CurrencyId;
                currency = consignments[j].Currency;
            }
        }
        else {
            amount = 0;
            localAmount = 0;
            exchangeRate = 0;
            currencyId = -1;
            currency = "";
        }
        rowSummaryModel.PaymentMethod = "Consignación";
        rowSummaryModel.CurrencyId = currencyId;
        rowSummaryModel.CurrencyDescription = currency;
        rowSummaryModel.Amount = FormatMoneySymbol(amount);
        rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
        rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
    }

    // ConsignmentCheck
    if (paymentMethod == "CH") {
        var consignmentsCheck = $("#modalConsignmentCheckAdd").find('#ConsignmentChecksListView').UifListView("getData");

        if (consignmentsCheck != null) {
            for (var j = 0; j < consignmentsCheck.length; j++) {
                var consignmentCheckAmount = RemoveFormatMoney(consignmentsCheck[j].Amount);
                amount += parseFloat(consignmentAmount);
                var consignmentCheckLocalAmount = RemoveFormatMoney(consignmentsCheck[j].LocalAmount);
                localAmount += parseFloat(consignmentCheckLocalAmount);
                var consignmentCheckExchangeRate = RemoveFormatMoney(consignmentsCheck[j].ExchangeRate);
                exchangeRate = parseFloat(consignmentCheckExchangeRate);
                currencyId = consignmentsCheck[j].CurrencyId;
                currency = consignmentsCheck[j].Currency;
            }
        }
        else {
            amount = 0;
            localAmount = 0;
            exchangeRate = 0;
            currencyId = -1;
            currency = "";
        }
        rowSummaryModel.PaymentMethod = "Consignación de Cheques";
        rowSummaryModel.CurrencyId = currencyId;
        rowSummaryModel.CurrencyDescription = currency;
        rowSummaryModel.Amount = FormatMoneySymbol(amount);
        rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
        rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
    }

    // Transfer
    if (paymentMethod == "TR") {
        var transfers = $("#modalTransferAdd").find('#transferListView').UifListView("getData");

        if (transfers != null) {
            for (var j = 0; j < transfers.length; j++) {
                var transferAmount = RemoveFormatMoney(transfers[j].Amount);
                amount += parseFloat(transferAmount);
                var transferLocalAmount = RemoveFormatMoney(transfers[j].LocalAmount);
                localAmount += parseFloat(transferLocalAmount);
                var transferExchangeRate = RemoveFormatMoney(transfers[j].ExchangeRate);
                exchangeRate = parseFloat(transferExchangeRate);
                currencyId = transfers[j].CurrencyId;
                currency = transfers[j].Currency;
            }
        }
        else {
            amount = 0;
            localAmount = 0;
            exchangeRate = 0;
            currencyId = -1;
            currency = "";
        }
        rowSummaryModel.PaymentMethod = "Transferencia";
        rowSummaryModel.CurrencyId = currencyId;
        rowSummaryModel.CurrencyDescription = currency;
        rowSummaryModel.Amount = FormatMoneySymbol(amount);
        rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
        rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
    }

    // Datafono
    if (paymentMethod == "D") {
        var datafono = $("#modalDatafonoAdd").find('#datafonoListView').UifListView("getData");

        if (datafono != null) {
            for (var j = 0; j < datafono.length; j++) {
                var datafonoAmount = RemoveFormatMoney(datafono[j].Amount);
                amount += parseFloat(datafonoAmount);
                var datafonoLocalAmount = RemoveFormatMoney(datafono[j].LocalAmount);
                localAmount += parseFloat(datafonoLocalAmount);
                var datafonoExchangeRate = RemoveFormatMoney(datafono[j].ExchangeRate);
                exchangeRate = parseFloat(datafonoExchangeRate);
                currencyId = datafono[j].CurrencyId;
                currency = datafono[j].Currency;
            }
        }
        else {
            amount = 0;
            localAmount = 0;
            exchangeRate = 0;
            currencyId = -1;
            currency = "";
        }
        rowSummaryModel.PaymentMethod = "Datafono";
        rowSummaryModel.CurrencyId = currencyId;
        rowSummaryModel.CurrencyDescription = currency;
        rowSummaryModel.Amount = FormatMoneySymbol(amount);
        rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
        rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
    }

    // Retention
    if (paymentMethod == "R") {
        var retention = $("#modalRetentionAdd").find('#retentionListView').UifListView("getData");

        if (retention != null) {
            for (var j = 0; j < retention.length; j++) {
                var retentionAmount = RemoveFormatMoney(retention[j].Amount);
                amount += parseFloat(retentionAmount);
                var retentionLocalAmount = RemoveFormatMoney(retention[j].LocalAmount);
                localAmount += parseFloat(retentionLocalAmount);
                var retentionExchangeRate = RemoveFormatMoney(retention[j].ExchangeRate);
                exchangeRate = parseFloat(retentionExchangeRate);
                currencyId = retention[j].CurrencyId;
                currency = retention[j].Currency;
                branchId = retention[j].BranchId;
                branch = retention[j].Branch;
                prefixId = retention[j].PrefixId;
                prefix = retention[j].Prefix;
                retentionConceptId = retention[j].RetentionConceptId;
                retentionConcept = retention[j].RetentionConcept;
            }
        }
        else {
            amount = 0;
            localAmount = 0;
            exchangeRate = 0;
            currencyId = -1;
            currency = "";
            branchId = -1;
            branch = "";
            prefixId = -1;
            prefix = "";
            retentionConceptId = -1;
            retentionConcept = "";
        }
        rowSummaryModel.PaymentMethod = "Retenciones";
        rowSummaryModel.CurrencyId = currencyId;
        rowSummaryModel.CurrencyDescription = currency;
        rowSummaryModel.Amount = FormatMoneySymbol(amount);
        rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate);
        rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
    }


    $("#summariesListView").UifListView("addItem", rowSummaryModel);
}

/////////////////////////////////////////////////////////////////
// Setear el método de pago efectivo y el total en la listview //
/////////////////////////////////////////////////////////////////
function SetCashSummary() {
    var rowSummaryModel = new RowModel();
    var currencyId = -1;
    var currency = "";
    var amount = 0;
    var exchangeRate = 0;
    var localAmount = 0;
    var index = -1;
    var j;


    var summaries = $("#summariesListView").UifListView("getData");
    if (summaries != null) {
        for (var j = 0; j < summaries.length; j++) {
            if (summaries[j].PaymentMethod == "Efectivo") {
                index = j;
            }
        }
    }

    // Cash
    var cashs = $("#modalCashAdd").find('#cashsListView').UifListView("getData");

    if (cashs != null) {
        for (var j = 0; j < cashs.length; j++) {
            var cashAmount = RemoveFormatMoney(cashs[j].Amount);
            amount += parseFloat(cashAmount);
            var cashLocalAmount = RemoveFormatMoney(cashs[j].LocalAmount);
            localAmount += parseFloat(cashLocalAmount);
            var cashExchangeRate = RemoveFormatMoney(cashs[j].ExchangeRate);
            exchangeRate = parseFloat(cashExchangeRate);
            currencyId = cashs[j].CurrencyId;
            currency = cashs[j].Currency;
        }
        if (cashs.length > 0) {
            rowSummaryModel.PaymentMethod = "Efectivo";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate, 6);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            if (index == -1) {
                $("#summariesListView").UifListView("addItem", rowSummaryModel);
            }
            else {
                $("#summariesListView").UifListView("editItem", index, rowSummaryModel);
            }
        }
        else {
            setSummary("Efectivo");
        }
    }
}

///////////////////////////////////////////////////////////////
// Setear el método de pago cheque y el total en la listview //
///////////////////////////////////////////////////////////////
function SetCheckSummary() {
    var rowSummaryModel = new RowModel();
    var currencyId = -1;
    var currency = "";
    var amount = 0;
    var exchangeRate = 0;
    var localAmount = 0;
    var index = -1;
    var j;
    var summaries = $("#summariesListView").UifListView("getData");
    if (summaries != null) {

        for (var j = 0; j < summaries.length; j++) {
            if (summaries[j].PaymentMethod == "Cheque") {
                index = j;
            }
        }
    }

    // Check
    var checks = $("#modalCheckAdd").find('#checksListView').UifListView("getData");

    if (checks != null) {
        for (var j = 0; j < checks.length; j++) {
            var checkAmount = RemoveFormatMoney(checks[j].Amount);
            amount += parseFloat(checkAmount);
            var checkLocalAmount = RemoveFormatMoney(checks[j].LocalAmount);
            localAmount += parseFloat(checkLocalAmount);
            var checkExchangeRate = RemoveFormatMoney(checks[j].ExchangeRate);
            exchangeRate = parseFloat(checkExchangeRate);
            currencyId = checks[j].CurrencyId;
            currency = checks[j].Currency;
        }
        if (checks.length > 0) {
            rowSummaryModel.PaymentMethod = "Cheque";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate, 6);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            if (index == -1) {
                $("#summariesListView").UifListView("addItem", rowSummaryModel);
            }
            else {
                $("#summariesListView").UifListView("editItem", index, rowSummaryModel);
            }
        }
        else {
            setSummary("Cheque");
        }
    }
}

function setSummary(paymentMethod) {
    var summariesList = $('#summariesListView').UifListView("getData");

    $("#summariesListView").UifListView({
        height: '40%', theme: 'dark',
        customAdd: false, customEdit: false, customDelete: false, localMode: true, add: false, edit: false, delete: true,
        displayTemplate: "#summaries-template", deleteCallback: deleteSummaryCashCallback
    });

    if (summariesList.length > 1) {
        for (var i in summariesList) {
            if (summariesList[i].PaymentMethod != paymentMethod) {
                var rowSummaryModel = new RowModel();
                rowSummaryModel.PaymentMethod = summariesList[i].PaymentMethod;
                rowSummaryModel.CurrencyId = summariesList[i].CurrencyId;
                rowSummaryModel.CurrencyDescription = summariesList[i].CurrencyDescription;
                rowSummaryModel.Amount = FormatMoneySymbol(summariesList[i].Amount);
                rowSummaryModel.ExchangeRate = FormatMoneySymbol(summariesList[i].ExchangeRate, 6);
                rowSummaryModel.LocalAmount = FormatMoneySymbol(summariesList[i].LocalAmount);
                $("#summariesListView").UifListView("addItem", rowSummaryModel);
            }
        }
    }
}
/////////////////////////////////////////////////////////////////////
// Setear el método de pago consignación y el total en la listview //
/////////////////////////////////////////////////////////////////////
function SetConsignmentSummary() {
    var rowSummaryModel = new RowModel();
    var currencyId = -1;
    var currency = "";
    var amount = 0;
    var exchangeRate = 0;
    var localAmount = 0;
    var index = -1;
    var j;


    var summaries = $("#summariesListView").UifListView("getData");
    if (summaries != null) {
        for (var j = 0; j < summaries.length; j++) {
            if (summaries[j].PaymentMethod == "Consignación") {
                index = j;
            }
        }
    }


    // Consignment
    var consignments = $("#modalConsignmentAdd").find('#consignmentsListView').UifListView("getData");

    if (consignments != null) {
        for (var j = 0; j < consignments.length; j++) {
            var consignmentAmount = RemoveFormatMoney(consignments[j].Amount);
            amount += parseFloat(consignmentAmount);
            var consignmentLocalAmount = RemoveFormatMoney(consignments[j].LocalAmount);
            localAmount += parseFloat(consignmentLocalAmount);
            var consignmentExchangeRate = RemoveFormatMoney(consignments[j].ExchangeRate);
            exchangeRate = parseFloat(consignmentExchangeRate);
            currencyId = consignments[j].CurrencyId;
            currency = consignments[j].Currency;
        }
        if (consignments.length) {
            rowSummaryModel.PaymentMethod = "Consignación";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate, 6);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            if (index == -1) {
                $("#summariesListView").UifListView("addItem", rowSummaryModel);
            }
            else {
                $("#summariesListView").UifListView("editItem", index, rowSummaryModel);
            }
        }
        else {
            setSummary("Consignación");
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
// Setear el método de pago consignación de cheques y el total en la listview //
///////////////////////////////////////////////////////////////////////////////
function SetConsignmentCheckSummary() {
    var rowSummaryModel = new RowModel();
    var currencyId = -1;
    var currency = "";
    var amount = 0;
    var exchangeRate = 0;
    var localAmount = 0;
    var index = -1;
    var j;


    var summaries = $("#summariesListView").UifListView("getData");
    if (summaries != null) {
        for (var j = 0; j < summaries.length; j++) {
            if (summaries[j].PaymentMethod == "Consignación de Cheques") {
                index = j;
            }
        }
    }


    // Consignment
    var consignmentsCheck = $("#modalConsignmentCheckAdd").find('#ConsignmentChecksListView').UifListView("getData");

    if (consignmentsCheck != null) {
        for (var j = 0; j < consignmentsCheck.length; j++) {
            var consignmentCheckAmount = RemoveFormatMoney(consignmentsCheck[j].Amount);
            amount += parseFloat(consignmentCheckAmount);
            var consignmentCheckLocalAmount = RemoveFormatMoney(consignmentsCheck[j].LocalAmount);
            localAmount += parseFloat(consignmentCheckLocalAmount);
            var consignmentCheckExchangeRate = RemoveFormatMoney(consignmentsCheck[j].ExchangeRate);
            exchangeRate = parseFloat(consignmentCheckExchangeRate);
            currencyId = consignmentsCheck[j].CurrencyId;
            currency = consignmentsCheck[j].Currency;
        }
        if (consignmentsCheck.length) {
            rowSummaryModel.PaymentMethod = "Consignación de Cheques";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate, 6);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            if (index == -1) {
                $("#summariesListView").UifListView("addItem", rowSummaryModel);
            }
            else {
                $("#summariesListView").UifListView("editItem", index, rowSummaryModel);
            }
        }
        else {
            setSummary("Consignación de Cheques");
        }
    }
}

////////////////////////////////////////////////////////////////
// Setear el método de pago tarjeta y el total en la listview //
////////////////////////////////////////////////////////////////
function SetCardSummary() {
    var rowSummaryModel = new RowModel();
    var currencyId = -1;
    var currency = "";
    var amount = 0;
    var exchangeRate = 0;
    var localAmount = 0;
    var index = -1;
    var j;

    var summaries = $("#summariesListView").UifListView("getData");
    if (summaries != null) {
        for (var j = 0; j < summaries.length; j++) {
            if (summaries[j].PaymentMethod == "Tarjeta") {
                index = j;
            }
        }
    }

    // Credit Card
    var cards = $("#modalCardAdd").find('#cardsListView').UifListView("getData");

    if (cards != null) {
        for (var j = 0; j < cards.length; j++) {
            var cardAmount = RemoveFormatMoney(cards[j].Amount);
            amount += parseFloat(cardAmount);
            var cardLocalAmount = RemoveFormatMoney(cards[j].LocalAmount);
            localAmount += parseFloat(cardLocalAmount);
            var cardExchangeRate = RemoveFormatMoney(cards[j].ExchangeRate);
            exchangeRate = parseFloat(cardExchangeRate);
            currencyId = cards[j].CurrencyId;
            currency = cards[j].Currency;
        }
        if (cards.length > 0) {
            rowSummaryModel.PaymentMethod = "Tarjeta";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate, 6);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            if (index == -1) {
                $("#summariesListView").UifListView("addItem", rowSummaryModel);
            }
            else {
                $("#summariesListView").UifListView("editItem", index, rowSummaryModel);
            }
        }
        else {
            setSummary("Tarjeta");
        }
    }
}

////////////////////////////////////////////////////////
// Setear el total de primas en el listview           //
////////////////////////////////////////////////////////
function SetPremiumsSummary() {
    var amount = 0;
    var premiums = $("#policiesListView").UifListView("getData");

    if (premiums != null) {
        for (var j = 0; j < premiums.length; j++) {
            var quotaValue = premiums[j].QuotaValue;
            amount += parseFloat(quotaValue);
        }
    }
    $("#TotalPolicy").text(FormatMoneySymbol(amount));
    $("#TotalApplication").val(FormatMoneySymbol(amount));
    if (amount == 0) {
        $("#TotalApplication").removeAttr("disabled");
        $("#TotalApplication").val("");
    }
    else {
        $("#TotalApplication").attr('disabled', 'disabled');
    }
}

//////////////////////////////////////////////////////////////////////
// Setear el método de pago transferencia y el total en la listview //
/////////////////////////////////////////////////////////////////////
function SetTransferSummary() {
    var rowSummaryModel = new RowModel();
    var currencyId = -1;
    var currency = "";
    var amount = 0;
    var exchangeRate = 0;
    var localAmount = 0;
    var index = -1;
    var j;

    editTransfer = -1;//Se inicializa de nuevo la variable luego de una eliminación

    var summaries = $("#summariesListView").UifListView("getData");
    if (summaries != null) {
        for (var j = 0; j < summaries.length; j++) {
            if (summaries[j].PaymentMethod == "Transferencia") {
                index = j;
            }
        }
    }

    // Transfer
    var transfers = $("#modalTransferAdd").find('#transferListView').UifListView("getData");

    if (transfers != null) {
        for (var j = 0; j < transfers.length; j++) {
            var transferAmount = RemoveFormatMoney(transfers[j].Amount);
            amount += parseFloat(transferAmount);
            var transferLocalAmount = RemoveFormatMoney(transfers[j].LocalAmount);
            localAmount += parseFloat(transferLocalAmount);
            var transferExchangeRate = RemoveFormatMoney(transfers[j].ExchangeRate);
            exchangeRate = parseFloat(transferExchangeRate);
            currencyId = transfers[j].CurrencyId;
            currency = transfers[j].Currency;
        }
        if (transfers.length > 0) {
            rowSummaryModel.PaymentMethod = "Transferencia";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate, 2);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            if (index == -1) {
                $("#summariesListView").UifListView("addItem", rowSummaryModel);
            }
            else {
                $("#summariesListView").UifListView("editItem", index, rowSummaryModel);
            }
        }
        else {
            setSummary("Transferencia");
        }
    }
}

////////////////////////////////////////////////////////////////
// Setear el método de pago datafono y el total en la listview //
////////////////////////////////////////////////////////////////
function SetDatafonoSummary() {
    var rowSummaryModel = new RowModel();
    var currencyId = -1;
    var currency = "";
    var amount = 0;
    var exchangeRate = 0;
    var localAmount = 0;
    var index = -1;
    var j;

    var summaries = $("#summariesListView").UifListView("getData");
    if (summaries != null) {

        for (var j = 0; j < summaries.length; j++) {
            if (summaries[j].PaymentMethod == "Datafono") {
                index = j;
            }
        }
    }

    // Datafono
    var datafono = $("#modalDatafonoAdd").find('#datafonoListView').UifListView("getData");

    if (datafono != null) {
        for (var j = 0; j < datafono.length; j++) {
            var datafonoAmount = RemoveFormatMoney(datafono[j].Amount);
            amount += parseFloat(datafonoAmount);
            var datafonoLocalAmount = RemoveFormatMoney(datafono[j].LocalAmount);
            localAmount += parseFloat(datafonoLocalAmount);
            var datafonoExchangeRate = RemoveFormatMoney(datafono[j].ExchangeRate);
            exchangeRate = parseFloat(datafonoExchangeRate);
            currencyId = datafono[j].CurrencyId;
            currency = datafono[j].Currency;
        }
        if (datafono.length > 0) {
            rowSummaryModel.PaymentMethod = "Datafono";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate, 6);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            if (index == -1) {
                $("#summariesListView").UifListView("addItem", rowSummaryModel);
            }
            else {
                $("#summariesListView").UifListView("editItem", index, rowSummaryModel);
            }
        }
        else {
            setSummary("Datafono");
        }
    }
}

////////////////////////////////////////////////////////////////////
// Setear el método de pago retenciones y el total en la listview //
///////////////////////////////////////////////////////////////////
function SetRetentionSummary() {
    var rowSummaryModel = new RowModel();
    var currencyId = -1;
    var currency = "";
    var amount = 0;
    var exchangeRate = 0;
    var localAmount = 0;
    var index = -1;
    var j;

    editRetention = -1 // Se inicializa variable, y permitir el ingreso de datos seleccionados previo a su eliminación.

    var summaries = $("#summariesListView").UifListView("getData");
    if (summaries != null) {

        for (var j = 0; j < summaries.length; j++) {
            if (summaries[j].PaymentMethod == "Retenciones") {
                index = j;
            }
        }
    }

    // Retention
    var retention = $("#modalRetentionAdd").find('#retentionListView').UifListView("getData");

    if (retention != null) {
        for (var j = 0; j < retention.length; j++) {
            var retentionAmount = RemoveFormatMoney(retention[j].Amount);
            amount += parseFloat(retentionAmount);
            var retentionLocalAmount = RemoveFormatMoney(retention[j].LocalAmount);
            localAmount += parseFloat(retentionLocalAmount);
            var retentionExchangeRate = RemoveFormatMoney(retention[j].ExchangeRate);
            exchangeRate = parseFloat(retentionExchangeRate);
            currencyId = retention[j].CurrencyId;
            currency = retention[j].Currency;
            //prefixId = retention[j].PrefixId;
            //prefix = retention[j].Prefix;
        }
        if (retention.length > 0) {
            rowSummaryModel.PaymentMethod = "Retenciones";
            rowSummaryModel.CurrencyId = currencyId;
            rowSummaryModel.CurrencyDescription = currency;
            rowSummaryModel.Amount = FormatMoneySymbol(amount);
            rowSummaryModel.ExchangeRate = FormatMoneySymbol(exchangeRate, 6);
            rowSummaryModel.LocalAmount = FormatMoneySymbol(localAmount);
            if (index == -1) {
                $("#summariesListView").UifListView("addItem", rowSummaryModel);
            }
            else {
                $("#summariesListView").UifListView("editItem", index, rowSummaryModel);
            }
        }
        else {
            setSummary("Retenciones");
        }
    }
}

////////////////////////////////////
// Valida si la caja esta cerrada //
////////////////////////////////////
function ClosureBillingDialogMain() {
    $('#modalBillingClosure').UifModal('showLocal', Resources.Closing);
}

//////////////////////////////////////////
// Despliega la ventana para abrir caja //
//////////////////////////////////////////
function OpenBillingDialogMain() {

    $('#modalOpeningBilling').find("#Branch").val($("#selectBranch option:selected").text());
    $('#modalOpeningBilling').find("#AccountingDate").val($("#ViewBagAccountingDate").val());
    $('#modalOpeningBilling').find("#UserId").val($("#ViewBagUserNick").val());
    $('#modalOpeningBilling').UifModal('showLocal', Resources.OpeningBilling);
}

function GetUserNick() {
    $.ajax({
        async: false,
        type: "GET",
        url: ACC_ROOT + "Common/GetUserNick",
        success: function (data) {
            $("#user").val(data);
            $('#modalOpeningBilling').find("#UserId").val(data);
            $("#UserIdClosure").val(data);
            $('#modalSaveBill').find("#ReceiptUser").val(data);
        }
    });
}

///////////////////////////////////////////////////////////////
// Valida la apertura de caja al momento de grabar el recibo //
///////////////////////////////////////////////////////////////
var resultClosureBillingMain = false;

function ClosureBillingMain() {
    var allowBranchId = '-1';
    if ($("#selectBranch").val() != "") {
        allowBranchId = $("#selectBranch").val();
    }
    validateOpenBill(allowBranchId, true);
}

function processSave() {
    if (resultClosureBillingMain == true) {

        if (isOpen == true) {

            // Validación de imputación
            if (TotalDebit() == 0 && TotalCredit() == 0) { //no se realizó una imputación
                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "Billing/SaveBillRequest",
                    data: {
                        "frmBill": SetDataBillMain(), "itemsToPayGridModel": SetDataItemToPaySummary(),
                        "branchId": $("#selectBranch").val(), "preliquidationBranch": preliquidationBranch
                    },
                    success: function (data) {

                        //EXCEPCION ROLLBACK
                        if (data.success == false && data.result == -2) {

                            $("#alert").UifAlert('show', Resources.PolicyComponentsValidationMessage, "danger");
                            $.unblockUI();

                        }
                        else if (data.success == false) {

                            $("#alert").UifAlert('show', data.result, "danger");
                            $.unblockUI();
                        }
                        else {

                            $("#modalSaveBill").find("#ReceiptDescription").text($("#Observation").val());
                            $("#modalSaveBill").find("#ReceiptTotalAmount").text($("#TotalReceipt").text());
                            $("#modalSaveBill").find("#BillingId").text("00000" + data.BillId);
                            $("#modalSaveBill").find("#TransactionNumber").text("00000" + data.TechnicalTransaction);
                            applyTechnicalTransaction = data.TechnicalTransaction;
                            GetAccountingDateMainBilling();
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

                            branchId = $("#selectBranch").val();
                            applyBillId = data.BillId;
                            if (data.StatusId == 1) {
                                $("#modalSaveBill").find("#ApplyReceipt").show();
                                applyReceiptNumber = data.BillId;
                                applyTechnicalTransaction = data.TechnicalTransaction;

                                applyDepositerBilling = personDocumentNumber + " - " + personName;
                                applyAmount = $("#TotalReceipt").text();
                                applyLocalAmount = $("#TotalReceipt").text();
                                applyBranch = $("#selectBranch option:selected").text();
                                applyIncomeConcept = $("#selectIncomeConcept option:selected").text();
                                applyPostedValue = "$0.00";
                                applyDescription = $("#Observation").val();
                                applyAccountingDate = $("#ViewBagAccountingDate").val();
                            }
                            else {
                                $("#modalSaveBill").find("#ApplyReceipt").hide();
                            }
                            if (data.StatusId == 3)//se aplicó el recibo
                            {
                                applyReceiptNumber = data.BillNumber;
                                applyTechnicalTransaction = data.TechnicalTransaction;
                                $("#modalSaveBill").find("#ApplyReceipt").hide();
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
                            $.unblockUI();
                        }

                        setTimeout(function () {
                            ClearFieldsMain();
                            ClearFields();

                        }, 2500);

                        SetDataBillEmptyMainBilling();
                        ClearImputationFields();
                    }
                });//fin ajax


                if (tempImputationId > 0) {

                    //hacer el delete del TempImputation en el caso de que el usuario abrio la ventana de Imputaciones
                    //pero no añadió items en ella
                    deleteTempImputation(tempImputationId);
                }

                isOpen = false;
            }
            else if (TotalDebit() != 0 || TotalCredit() != 0) { //se realizó la imputación

                imputationTotalControl = SetMainTotalControl(totalAmount, TotalDebit(), TotalCredit(), 2);

                if (imputationTotalControl != 0) { //controla que el total sea igual a cero
                    $("#alert").UifAlert('show', Resources.WarningImputationTotalControlValidation, "warning");
                    setTimeout(function () {
                        $("#alert").UifAlert('hide');
                    }, 3000);
                    $.unblockUI();
                    return true;
                }
                else {

                    $.ajax({
                        async: false,
                        type: "POST",
                        url: ACC_ROOT + "Billing/SaveBillRequest",
                        data: {
                            "frmBill": SetDataBillMain(), "itemsToPayGridModel": SetDataItemToPaySummary(), "branchId": $("#selectBranch").val(),
                            "preliquidationBranch": preliquidationBranch
                        },
                        success: function (data) {

                            if (data.success == false && data.result != -2) {

                                $("#alert").UifAlert('show', data.result, "danger");

                            }
                            else if (data.success == false && data.result == -2) {

                                $("#alert").UifAlert('show', Resources.PolicyComponentsValidationMessage, "danger");
                            }
                            else {
                                $("#modalSaveBill").find("#ReceiptDescription").text($("#Observation").val());
                                $("#modalSaveBill").find("#ReceiptTotalAmount").text($("#TotalReceipt").val());
                                $("#modalSaveBill").find("#BillingId").text("00000" + data.BillId);
                                $("#modalSaveBill").find("#TransactionNumber").text("00000" + data.TechnicalTransaction);
                                applyTechnicalTransaction = data.TechnicalTransaction;
                                GetAccountingDateMainBilling();
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

                                branchId = $("#selectBranch").val();
                                applyBillId = data.BillId;

                                // Oculto el botón de aplicación de recibo en el mensaje de grabación
                                $("#modalSaveBill").find("#ApplyReceipt").hide();

                                // Actualizo el sourceCode en el temporal de imputación.
                                $.ajax({
                                    async: false,
                                    type: "POST",
                                    url: ACC_ROOT + "Billing/UpdateTempImputationSourceCode",
                                    data: { "tempImputationId": tempImputationId, "sourceId": applyBillId },
                                    success: function (data) {
                                        // Graba imputación a reales, elimina de temporales, actualiza estado de bill
                                        $.ajax({
                                            async: false,
                                            type: "POST",
                                            url: ACC_ROOT + "ReceiptApplication/SaveReceiptApplication",
                                            data: {
                                                "sourceCode": BillId, "tempImputationId": tempImputationId,
                                                "imputationTypeId": $("#ViewBagImputationTypeBill").val(),
                                                "comments": "Imputación generada antes que el bill", "statusId": 3
                                            },
                                            success: function () {
                                                $("#alert").UifAlert('show', Resources.SaveRealSuccessMessage, "succes");
                                                setTimeout(function () {
                                                    $("#alert").UifAlert('hide');
                                                }, 3000);

                                                ClearImputationFields();
                                            }
                                        });
                                    }
                                });

                                $('#modalSaveBill').UifModal('showLocal', Resources.ReceiptSaveSuccess);

                            }

                            setTimeout(function () {
                                ClearFieldsMain();
                                ClearFields();
                                $.unblockUI();
                            }, 2500);
                            SetDataBillEmptyMainBilling();
                        }
                    });//fin ajax

                    isOpen = false;
                }
            }
        }
        else {
            $.unblockUI();
        }
    }
    else {
        $.unblockUI();
    }

}

//////////////////////////////////////////
// Setea el objeto que grabará el recbo //
//////////////////////////////////////////
function SetDataBillMain() {
    var paymentsTotal = 0;
    var j;
    var localAmount;
    var exchangeRate;

    oBillModel.BillId = 0; //autonumerico
    oBillModel.BillingConceptId = $("#selectIncomeConcept").val();
    oBillModel.BillControlId = billControlId;
    oBillModel.RegisterDate = $("#BillingDate").val();
    oBillModel.Description = $("#Observation").val();
    paymentsTotal = RemoveFormatMoney($("#TotalReceipt").text());
    oBillModel.PaymentsTotal = ReplaceDecimalPoint(paymentsTotal);
    oBillModel.PayerId = payerId;
    oBillModel.SourcePaymentId = 0;
    var payerTypeId = $("#CollectionTo").val();


    if ($("#CollectionTo").val() == $("#ViewBagOthersCode").val() || $("#CollectionTo").val() == $("#ViewBagCollectorCode").val()) {
        personDocumentNumber = $("#inputOtherDocumentNumber").val();
        personName = $("#inputOtherName").val();
    }

    oBillModel.PayerTypeId = payerTypeId;
    oBillModel.PayerDocumentTypeId = $("#DocumentType").val() == "" ? -1 : $("#DocumentType").val();
    oBillModel.PayerDocumentNumber = personDocumentNumber;
    oBillModel.PayerName = personName;
    if (!IsNewMainBilling) {
        oBillModel.SourcePaymentId = parseInt($("#ViewBagPreliquidationId").val());
    }
    //Cash
    var cashs = $("#modalCashAdd").find("#cashsListView").UifListView("getData");
    if (cashs != null) {
        for (var j = 0; j < cashs.length; j++) {
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

            oPaymentSummaryModel.PaymentId = 0; //autonumerico
            oPaymentSummaryModel.BillId = 0;    //se lo asigna a nivel de controlador
            oPaymentSummaryModel.PaymentMethodId = cashs[j].PaymentTypeId;
            var cashAmount = RemoveFormatMoney(cashs[j].Amount);
            oPaymentSummaryModel.Amount = ReplaceDecimalPoint(cashAmount);
            oPaymentSummaryModel.CurrencyId = cashs[j].CurrencyId;
            localAmount = RemoveFormatMoney(cashs[j].LocalAmount);
            oPaymentSummaryModel.LocalAmount = ReplaceDecimalPoint(localAmount);
            exchangeRate = RemoveFormatMoney(cashs[j].ExchangeRate);
            oPaymentSummaryModel.ExchangeRate = ReplaceDecimalPoint(exchangeRate);

            oBillModel.PaymentSummary.push(oPaymentSummaryModel);
        }
    }

    // Check
    var checks = $("#modalCheckAdd").find("#checksListView").UifListView("getData");
    if (checks != null) {
        for (var j = 0; j < checks.length; j++) {
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

            oPaymentSummaryModel.PaymentId = 0; //autonumerico
            oPaymentSummaryModel.BillId = 0;    //se lo asigna a nivel de controlador
            oPaymentSummaryModel.PaymentMethodId = checks[j].PaymentTypeId;
            var checkAmount = RemoveFormatMoney(checks[j].Amount);
            oPaymentSummaryModel.Amount = ReplaceDecimalPoint(checkAmount);
            oPaymentSummaryModel.CurrencyId = checks[j].CurrencyId;
            localAmount = RemoveFormatMoney(checks[j].LocalAmount);
            oPaymentSummaryModel.LocalAmount = ReplaceDecimalPoint(localAmount);
            exchangeRate = RemoveFormatMoney(checks[j].ExchangeRate);
            oPaymentSummaryModel.ExchangeRate = ReplaceDecimalPoint(exchangeRate);

            oCheckModel = {
                DocumentNumber: null,
                IssuingBankId: -1,
                IssuingAccountNumber: null,
                IssuerName: null,
                Date: null
            };

            oCheckModel.DocumentNumber = checks[j].DocumentNumber;
            oCheckModel.IssuingBankId = checks[j].IssuingBankId;
            oCheckModel.IssuingAccountNumber = checks[j].IssuingBankAccountNumber;
            oCheckModel.IssuerName = checks[j].IssuerName;
            oCheckModel.Date = checks[j].Date;

            oPaymentSummaryModel.CheckPayments.push(oCheckModel);

            oBillModel.PaymentSummary.push(oPaymentSummaryModel);
        }
    }

    // Credit Card
    var cards = $("#modalCardAdd").find("#cardsListView").UifListView("getData");
    if (cards != null) {
        for (var j = 0; j < cards.length; j++) {
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

            oPaymentSummaryModel.PaymentId = 0; //autonumerico
            oPaymentSummaryModel.BillId = 0;    //se lo asigna a nivel de controlador
            oPaymentSummaryModel.PaymentMethodId = cards[j].PaymentTypeId;
            var cardAmount = RemoveFormatMoney(cards[j].Amount);
            oPaymentSummaryModel.Amount = ReplaceDecimalPoint(cardAmount);
            oPaymentSummaryModel.CurrencyId = cards[j].CurrencyId;
            localAmount = RemoveFormatMoney(cards[j].LocalAmount);
            oPaymentSummaryModel.LocalAmount = ReplaceDecimalPoint(localAmount);
            exchangeRate = RemoveFormatMoney(cards[j].ExchangeRate);
            oPaymentSummaryModel.ExchangeRate = ReplaceDecimalPoint(exchangeRate);

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

            oCreditModel.CardNumber = cards[j].CardNumber;
            oCreditModel.Voucher = cards[j].VoucherNumber;
            oCreditModel.AuthorizationNumber = cards[j].AuthorizationNumber;
            oCreditModel.CreditCardTypeId = cards[j].CardType;
            oCreditModel.IssuingBankId = cards[j].IssuingBankId;
            oCreditModel.Holder = cards[j].CardsName;
            oCreditModel.ValidThruYear = cards[j].ValidThruYear;
            oCreditModel.ValidThruMonth = cards[j].ValidThruMonth;
            var taxBase = RemoveFormatMoney(cards[j].TaxBase);
            oCreditModel.TaxBase = ReplaceDecimalPoint(taxBase);

            oPaymentSummaryModel.CreditPayments.push(oCreditModel);

            oBillModel.PaymentSummary.push(oPaymentSummaryModel);
        }
    }

    // Consignment
    var consignments = $("#modalConsignmentAdd").find("#consignmentsListView").UifListView("getData");
    if (consignments != null) {
        for (var j = 0; j < consignments.length; j++) {
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

            oPaymentSummaryModel.PaymentId = 0; //autonumerico
            oPaymentSummaryModel.BillId = 0;    //se lo asigna a nivel de controlador
            oPaymentSummaryModel.PaymentMethodId = consignments[j].PaymentTypeId;
            var consignmentAmount = RemoveFormatMoney(consignments[j].Amount);
            oPaymentSummaryModel.Amount = ReplaceDecimalPoint(consignmentAmount);
            oPaymentSummaryModel.CurrencyId = consignments[j].CurrencyId;
            localAmount = RemoveFormatMoney(consignments[j].LocalAmount);
            oPaymentSummaryModel.LocalAmount = ReplaceDecimalPoint(localAmount);
            exchangeRate = RemoveFormatMoney(consignments[j].ExchangeRate);
            oPaymentSummaryModel.ExchangeRate = ReplaceDecimalPoint(exchangeRate);

            oDepositVoucherModel = {
                VoucherNumber: null,
                ReceivingAccountBankId: -1,
                ReceivingAccountNumber: null,
                Date: null,
                DepositorName: null
            };

            oDepositVoucherModel.VoucherNumber = consignments[j].DocumentNumber;
            oDepositVoucherModel.ReceivingAccountBankId = consignments[j].RecievingBankId;
            oDepositVoucherModel.ReceivingAccountNumber = consignments[j].RecievingBankAccountNumber;
            oDepositVoucherModel.Date = consignments[j].Date;
            oDepositVoucherModel.DepositorName = consignments[j].IssuerName;

            oPaymentSummaryModel.DepositVouchers.push(oDepositVoucherModel);

            oBillModel.PaymentSummary.push(oPaymentSummaryModel);
        }
    }

    // Transfer
    var transfers = $("#modalTransferAdd").find("#transferListView").UifListView("getData");
    if (transfers != null) {
        for (var j = 0; j < transfers.length; j++) {
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

            oPaymentSummaryModel.PaymentId = 0; //autonumerico
            oPaymentSummaryModel.BillId = 0;    //se lo asigna a nivel de controlador
            oPaymentSummaryModel.PaymentMethodId = transfers[j].PaymentTypeId;
            var transferAmount = RemoveFormatMoney(transfers[j].Amount);
            oPaymentSummaryModel.Amount = ReplaceDecimalPoint(transferAmount);
            oPaymentSummaryModel.CurrencyId = transfers[j].CurrencyId;
            localAmount = RemoveFormatMoney(transfers[j].LocalAmount);
            oPaymentSummaryModel.LocalAmount = ReplaceDecimalPoint(localAmount);
            exchangeRate = RemoveFormatMoney(transfers[j].ExchangeRate);
            oPaymentSummaryModel.ExchangeRate = ReplaceDecimalPoint(exchangeRate);

            oTransferModel = {
                DocumentNumber: null,
                IssuingBankId: -1,
                IssuingAccountNumber: null,
                IssuerName: null,
                ReceivingBankId: -1,
                ReceivingAccountNumber: null,
                Date: null
            };

            oTransferModel.DocumentNumber = transfers[j].DocumentNumber;
            oTransferModel.IssuingBankId = transfers[j].IssuingBankId;
            oTransferModel.IssuingAccountNumber = transfers[j].IssuingBankAccountNumber;
            oTransferModel.ReceivingAccountBankId = transfers[j].RecievingBankId;
            oTransferModel.ReceivingAccountNumber = transfers[j].RecievingBankAccountNumber;
            oTransferModel.Date = transfers[j].Date;
            oTransferModel.IssuerName = transfers[j].IssuerName;


            oPaymentSummaryModel.TransferPayments.push(oTransferModel);

            oBillModel.PaymentSummary.push(oPaymentSummaryModel);
        }
    }

    // Datafomo
    var datafono = $("#modalDatafonoAdd").find("#datafonoListView").UifListView("getData");
    if (datafono != null) {
        for (var j = 0; j < datafono.length; j++) {
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

            oPaymentSummaryModel.PaymentId = 0; //autonumerico
            oPaymentSummaryModel.BillId = 0;    //se lo asigna a nivel de controlador
            oPaymentSummaryModel.PaymentMethodId = datafono[j].PaymentTypeId;
            var datafonoAmount = RemoveFormatMoney(datafono[j].Amount);
            oPaymentSummaryModel.Amount = ReplaceDecimalPoint(datafonoAmount);
            oPaymentSummaryModel.CurrencyId = datafono[j].CurrencyId;
            localAmount = RemoveFormatMoney(datafono[j].LocalAmount);
            oPaymentSummaryModel.LocalAmount = ReplaceDecimalPoint(localAmount);
            exchangeRate = RemoveFormatMoney(datafono[j].ExchangeRate);
            oPaymentSummaryModel.ExchangeRate = ReplaceDecimalPoint(exchangeRate);

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

            oCreditModel.CardNumber = datafono[j].CardNumber;
            oCreditModel.Voucher = datafono[j].VoucherNumber;
            oCreditModel.AuthorizationNumber = datafono[j].AuthorizationNumber;
            oCreditModel.CreditCardTypeId = datafono[j].CardType;
            oCreditModel.IssuingBankId = datafono[j].IssuingBankId;
            oCreditModel.Holder = datafono[j].CardsName;
            oCreditModel.ValidThruYear = datafono[j].ValidThruYear;
            oCreditModel.ValidThruMonth = datafono[j].ValidThruMonth;
            var taxBase = RemoveFormatMoney(datafono[j].TaxBase);
            oCreditModel.TaxBase = ReplaceDecimalPoint(taxBase);

            oPaymentSummaryModel.CreditPayments.push(oCreditModel);

            oBillModel.PaymentSummary.push(oPaymentSummaryModel);
        }

    }

    // Retention
    var retention = $("#modalRetentionAdd").find("#retentionListView").UifListView("getData");
    if (retention != null) {
        for (var j = 0; j < retention.length; j++) {
            oPaymentSummaryModel = {
                PaymentId: 0,
                BillId: 0,
                PaymentMethodId: 0,
                Amount: 0,
                CurrencyId: 0,
                BranchId: 0,
                PrefixId: 0,
                LocalAmount: 0,
                ExchangeRate: 0,
                CheckPayments: [],
                CreditPayments: [],
                TransferPayments: [],
                DepositVouchers: [],
                RetentionReceipts: []
            };

            oPaymentSummaryModel.PaymentId = 0; //autonumerico
            oPaymentSummaryModel.BillId = 0;    //se lo asigna a nivel de controlador
            oPaymentSummaryModel.PaymentMethodId = retention[j].PaymentTypeId;
            var retentionAmount = RemoveFormatMoney(retention[j].Amount);
            oPaymentSummaryModel.Amount = ReplaceDecimalPoint(retentionAmount);
            oPaymentSummaryModel.CurrencyId = retention[j].CurrencyId;
            oPaymentSummaryModel.BranchId = retention[j].BranchId;
            oPaymentSummaryModel.PrefixId = retention[j].PrefixId;
            localAmount = RemoveFormatMoney(retention[j].LocalAmount);
            oPaymentSummaryModel.LocalAmount = ReplaceDecimalPoint(localAmount);
            exchangeRate = RemoveFormatMoney(retention[j].ExchangeRate);
            oPaymentSummaryModel.ExchangeRate = ReplaceDecimalPoint(exchangeRate);

            oRetentionReceiptModel = {
                AuthorizationNumber: null,
                BillNumber: null,
                SerialNumber: null,
                SerialVoucherNumber: null,
                VoucherNumber: null,
                BranchId: 0,
                PrefixId: 0,
                Date: null,
                IssueDate: null,
                ExpirationDate: null,
                PolicyNumber: null,
                EndorsementNumber: null,
                InvoiceNumber: null,
                InvoiceDate: null
            };

            oRetentionReceiptModel.AuthorizationNumber = retention[j].AuthorizationNumber;
            oRetentionReceiptModel.BillNumber = retention[j].DocumentNumber;
            oRetentionReceiptModel.SerialNumber = retention[j].SerialNumber;
            oRetentionReceiptModel.SerialVoucherNumber = retention[j].SerialVoucher;
            oRetentionReceiptModel.VoucherNumber = retention[j].VoucherNumber;
            oRetentionReceiptModel.Date = retention[j].Date;
            oRetentionReceiptModel.IssueDate = retention[j].IssueDate;
            oRetentionReceiptModel.ExpirationDate = retention[j].ExpirationDate;
            oRetentionReceiptModel.RetentionConceptId = retention[j].RetentionConceptId;
            oRetentionReceiptModel.PolicyNumber = retention[j].PolicyNumber;
            oRetentionReceiptModel.EndorsementNumber = retention[j].EndorsementNumber;
            oRetentionReceiptModel.InvoiceNumber = retention[j].DocumentNumber;
            oRetentionReceiptModel.InvoiceDate = retention[j].Date;

            oPaymentSummaryModel.RetentionReceipts.push(oRetentionReceiptModel);

            oBillModel.PaymentSummary.push(oPaymentSummaryModel);
        }
    }

    return oBillModel;
}

/////////////////////////////////////////////////////
// Setea los valores de los items a pagar (primas) //
/////////////////////////////////////////////////////
function SetDataItemToPaySummary() {
    oItemsToPayGrid.billID = 1;

    var premiums = $("#policiesListView").UifListView("getData");

    if (premiums != null) {
        for (var j = 0; j < premiums.length; j++) {
            oBillItem = {
                billItemId: 0,
                billID: 0,
                itemTypeId: 0,
                description: "",
                amount: 0,
                currencyId: 0,
                exchangeRate: 0,
                paidAmount: 0,
                column1: "",
                column2: "",
                column3: "",
                column4: "",
                column5: "",
                column6: "",
                column7: ""
            };

            oBillItem.billItemId = premiums[j].ItemId;
            oBillItem.billID = premiums[j].BillId;
            oBillItem.itemTypeId = premiums[j].ItemTypeId;
            oBillItem.description = premiums[j].Reference;
            oBillItem.amount = ReplaceDecimalPoint(String(premiums[j].QuotaValue));
            oBillItem.paidAmount = ReplaceDecimalPoint(String(premiums[j].Amount));
            oBillItem.column1 = premiums[j].PolicyId;
            oBillItem.column2 = premiums[j].EndorsementId;
            oBillItem.column3 = premiums[j].QuotaNumber;
            oBillItem.column4 = premiums[j].PayerId;
            oBillItem.currencyId = premiums[j].CurrencyId;
            oBillItem.exchangeRate = ReplaceDecimalPoint(String(premiums[j].ExchangeRate));

            oItemsToPayGrid.billItem.push(oBillItem);
        }
    }

    return oItemsToPayGrid;
}

////////////////////////////////////////////////
// Obtiene el total de débito para aplicación //
////////////////////////////////////////////////
function TotalDebit() {
    var total = 0;
    return total;
}

/////////////////////////////////////////////////
// Obtiene el total de crédito para aplicación //
/////////////////////////////////////////////////
function TotalCredit() {
    var total = 0;
    return total;
}

///////////////////////////////
// Setea los objetos a nulos //
///////////////////////////////
function SetDataBillEmptyMainBilling() {

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

        BranchId: 0,
        Branch: null,
        PrefixId: 0,
        Prefix: null,
        PolicyNumber: 0,
        EndorsementNumber: 0,
        IssueDate: "",
        ExpirationDate: "",
        RetentionConceptId: 0,
        RetentionConcept: null,
    };

    oItemsToPayGrid = {
        billItem: []
    };

    oBillItem = {
        billItemId: 0,
        billID: 0,
        itemTypeId: 0,
        description: "",
        amount: 0,
        currencyId: 0,
        exchangeRate: 0,
        paidAmount: 0,
        column1: "",
        column2: "",
        column3: "",
        column4: "",
        column5: "",
        column6: "",
        column7: ""
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
        TransferPayments: [],
        RetentionReceipts: [],
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
        Date: null,
        DateIssue: null,
        DateExpiration: null
    };
}

////////////////////////////////////////////
// Limpia los campos después de grabación //
////////////////////////////////////////////
function clearFields() {
    $("#selectBranch").val($("#ViewBagBranchDefault").val());
    $("#selectBranch").trigger('change');
    $("#Observation").val("");
    $("#inputName").UifAutoComplete('setValue', "");
    $("#inputDocumentNumber").UifAutoComplete('setValue', "");
    $("#SearchCode").val("");
    $("#DocumentType").val("");
    $("#inputAgentDocumentNumber").val("");
    $("#inputAgentName").val("");
    $("#inputCompanyDocumentNumber").val("");
    $("#inputCompanyName").val("");
    $("#inputOtherDocumentNumber").UifAutoComplete('setValue', "");
    $("#inputOtherName").UifAutoComplete('setValue', "");
    $("#inputInsuredDocumentNumber").UifAutoComplete('setValue', "");
    $("#inputInsuredName").UifAutoComplete('setValue', "");
    $("#inputSupplierDocumentNumber").UifAutoComplete('setValue', "");
    $("#inputSupplierName").UifAutoComplete('setValue', "");
    $("#inputReinsuranceName").UifAutoComplete('setValue', "");
    $("#inputReinsuranceDocumentNumber").UifAutoComplete('setValue', "");
    $("#inputContractorDocumentNumber").UifAutoComplete('setValue', "");
    $("#inputContractorName").UifAutoComplete('setValue', "");
    $("#inputThirdName").UifAutoComplete('setValue', "");
    $("#inputThirdDocumentNumber").UifAutoComplete('setValue', "");
    $("#ViewBagTempImputationId").val("");
}
function ClearFieldsMain() {

    selectedInsured = undefined;
    IsNewMainBilling = true;
    preliquidationBranch = 0;
    selectedInsured = undefined;
    selectedCompany = undefined;
    selectedEmployee = undefined;
    selectedAgent = undefined;
    selectedSupplier = undefined;
    uiScreen = 0;
    selectedThird = undefined;
    totalReceipt = 0;
    difference = 0;


    $("#policiesListView").UifListView({
        height: '50%', customDelete: false, customAdd: false, customEdit: false, edit: false,
        delete: true, deleteCallback: deleteSelectedPremiumCallback, displayTemplate: "#policies-template"
    });

    $("#summariesListView").UifListView({
        height: '40%', theme: 'dark',
        customAdd: false, customEdit: false, customDelete: false, localMode: true, add: false, edit: false, delete: true,
        displayTemplate: "#summaries-template", deleteCallback: deleteSummaryCashCallback
    });

    $("#modalCashAdd").find("#cashsListView").UifListView({
        autoHeight: true, customAdd: false, customEdit: true, localMode: true, customDelete: false, add: false, edit: true,
        delete: true, displayTemplate: "#cash-display-template", editCallback: editCashCallback, deleteCallback: deleteCashCallback
    });

    $("#modalCheckAdd").find("#checksListView").UifListView({
        autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
        delete: true, displayTemplate: "#check-display-template", editCallback: editCheckCallback, deleteCallback: deleteCheckCallback
    });

    LoadcardsListView();
    LoadconsigListView();
    LoadConsignmentCheckListView();

    $("#modalTransferAdd").find("#transferListView").UifListView({
        autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
        delete: true, displayTemplate: "#transfer-display-template", editCallback: editTransferCallback, deleteCallback: deleteTransferCallback
    });

    $("#modalDatafonoAdd").find("#datafonoListView").UifListView({
        autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
        delete: true, displayTemplate: "#datafono-display-template", editCallback: editDatafonoCallback, deleteCallback: deleteDatafonoCallback
    });

    $("#modalRetentionAdd").find("#retentionListView").UifListView({
        autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
        delete: true, displayTemplate: "#retention-display-template", editCallback: editRetentionCallback, deleteCallback: deleteRetentionCallback
    });

    $("#Difference").text(FormatMoneySymbol(0));
    $("#TotalReceipt").text(FormatMoneySymbol(0));
    $("#TotalApplication").text(FormatMoneySymbol(0));
    $("#TotalApplication").val('');
    $("#TotalApplicationCash").val('');
    $("#TotalPolicy").text(FormatMoneySymbol(0));
    payerId = -1;
    issuingBankId = -1;

    $("#modalCashAdd").find("#TotalCash").text(FormatMoneySymbol(0));
    $("#modalCheckAdd").find("#TotalCheck").text(FormatMoneySymbol(0));
    $("#modalCardAdd").find("#TotalCard").text(FormatMoneySymbol(0));
    $("#modalConsignmentAdd").find("#TotalConsignment").text(FormatMoneySymbol(0));
    $("#modalConsignmentCheckAdd").find("#TotalConsignmentCheck").text(FormatMoneySymbol(0));
    $("#modalTransferAdd").find("#TotalTransfer").text(FormatMoneySymbol(0));
    $("#modalDatafonoAdd").find("#TotalDatafono").text(FormatMoneySymbol(0));
    $("#modalRetentionAdd").find("#TotalRetention").text(FormatMoneySymbol(0));
}

///////////////////////////////////////////////////////
// Limpia los campos después de grabación imputación //
///////////////////////////////////////////////////////
function ClearImputationFields() {
    imputationTotalControl = 0;
    tempImputationId = 0;
}

///////////////////////////////
// Obtiene la fecha contable //
///////////////////////////////
function GetAccountingDateMainBilling() {
    $("#modalSaveBill").find("#BillingDate").val($("#ViewBagDateAccounting").val());
    $("#modalSaveBill").find("#Dateaccounting").val($("#ViewBagDateAccounting").val());
}


///////////////////////////////////////////
// Carga reporte de caja                 //
///////////////////////////////////////////
function LoadBillingReport(branchId, billCode) {
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "Report/LoadBillingReport",
        data: { "branchId": branchId, "billCode": billCode, "reportId": 3 },
        success: function () {

        }
    });
}

//////////////////////////////////////////////
// Visualiza el reporte de caja en pantalla //
//////////////////////////////////////////////
function ShowBillingReport() {
    window.open(ACC_ROOT + "Report/ShowBillingClousureReport", 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

//////////////////////////////////////////////
// Visualiza el reporte de caja en pantalla //
//////////////////////////////////////////////
function ShowReceiptReportMainBilling(branchId, billCode, otherPayerName) {
    var controller = $("#ViewBagShowReceiptReportLink").val() + "?branchId=" + branchId + "&billCode=" +
        billCode + "&reportId=3" + "&otherPayerName=" + otherPayerName;
    window.open(controller, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

/////////////////////////////////////////////////////////////
// Valida el mes y año expiración de la tarjeta de crédito //
/////////////////////////////////////////////////////////////
function CheckExpirationDate() {
    var date;
    var validateExpirationDate = true;
    $("#modalCardAdd").find("#alertForm").hide();

    var systemDate = new Date();
    var month = systemDate.getMonth() + 1;
    var year = systemDate.getFullYear();

    //Tarjeta de Crédito
    if (parseInt($("#modalCardAdd").find("#CreditCardValidThruYear").val()) < parseInt(year)) {
        $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.ExpiredCreditCard, "warning");
        validateExpirationDate = false;
    }
    else if (parseInt($("#modalCardAdd").find("#CreditCardValidThruYear").val()) == parseInt(year) &&
        parseInt($("#modalCardAdd").find("#selectCreditCardValidThruMonth").val()) < parseInt(month)) {
        $("#modalCardAdd").find("#alertForm").UifAlert('show', Resources.ExpiredCreditCard, "warning");
        validateExpirationDate = false;
    }

    return validateExpirationDate;
}

//////////////////////////////////////////////////////////////////////
// Valida el mes y año expiración de la tarjeta de crédito Datafono //
/////////////////////////////////////////////////////////////////////
function CheckExpirationDateDatafono() {
    var date;
    var validateDatafono = true;

    $("#modalDatafonoAdd").find("#alertForm").hide();

    var systemDate = new Date();
    var month = systemDate.getMonth() + 1;
    var year = systemDate.getFullYear();

    //Datafono
    if (parseInt($("#modalDatafonoAdd").find("#DatafonoCreditCardValidThruYear").val()) < parseInt(year)) {
        $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.ExpiredCreditCard, "warning");
        validateDatafono = false;
    }
    else if (parseInt($("#modalDatafonoAdd").find("#DatafonoCreditCardValidThruYear").val()) == parseInt(year) &&
        parseInt($("#modalDatafonoAdd").find("#selectDatafonoCreditCardValidThruMonth").val()) < parseInt(month)) {
        $("#modalDatafonoAdd").find("#alertForm").UifAlert('show', Resources.ExpiredCreditCard, "warning");
        validateDatafono = false;

    }
    return validateDatafono;
}

/////////////////////////////////////////////////////////////////////////////////////////
// Valida fecha contable con la fecha expiración de retenciones                       //
///////////////////////////////////////////////////////////////////////////////////////
function ValidateExpirationDateRetention() {
    var date;

    $.ajax({
        type: "GET",
        url: ACC_ROOT + "Common/GetAccountingDate",
        async: false,
        success: function (data) {
            date = data;
        }
    });

    //Retention
    if (date != '') {
        if (!CompareDates($("#RetentionExpirationDate").val(), date)) {
            $("#RetentionExpirationDate").val(date);
        }
    }
}

///////////////////////////////////////////////////
// Controla el orden en que se añaden las cuotas //
///////////////////////////////////////////////////
function QuotaOrderControl() {
    var result = false;
    var index = 0;

    var premiums = $("#modalPremiumAdd").find("#premiumsListView").UifListView("getData");

    if (premiums != null) {
        for (var j = 0; j < premiums.length; j++) {
            //Establecer control para evitar desbordamiento
            if (j < premiums.length - 1) {
                var k = parseInt(j) + 1;
                var rowDataNext = premiums[k];
            }
            else {
                break;
            }
            var top = premiums[j].EndorsementId + premiums[j].PolicyId + premiums[j].PayerDocumentNumber;
            var lower = rowDataNext.EndorsementId + rowDataNext.PolicyId + rowDataNext.PayerDocumentNumber;

            if (top == lower) {
                if (rowDataNext.Select == "1" && premiums[j].Select == "0") {
                    index += 1;
                }
            }
        }
    }

    if (index > 0) {
        result = false;
    }
    else {
        result = true;
    }

    return result;
}

////////////////////////////////////////////////
/// Realiza la aplicación del recibo de caja ///
////////////////////////////////////////////////
function ApplyBill(billId) {
    // Consultar si existe temporal
    $.ajax({
        async: false,
        type: "POST",
        url: ACC_ROOT + "ReceiptApplication/GetTempImputationBySourceCode",
        data: { "imputationTypeId": Resources.ImputationTypeBill, "sourceCode": billId },
        success: function (data) {
            if (data.Id == 0) {
                // Graba a temporales
                $.ajax({
                    async: false,
                    type: "POST",
                    url: ACC_ROOT + "ReceiptApplication/SaveTempImputation",
                    data: { "imputationTypeId": Resources.ImputationTypeBill, "sourceCode": billId },
                    success: function (dataImputation) {
                        tempImputationId = dataImputation.Id;
                    }
                });
            }
        }
    });
}

///////////////////////////////////////////////
// Obtiene la descripción de la moneda local //
///////////////////////////////////////////////
function GetCurrencyDescription(currencyId) {
    var description = "";
    $.ajax({
        async: false,
        type: "GET",
        url: ACC_ROOT + "Common/GetCurrencyDescriptionByCurrencyId",
        data: { "currencyId": currencyId },
        success: function (data) {
            description = data;
        }
    });

    return description;
}

///////////////////////////////////////////////
// Añade efectivo al listview de movimientos //
///////////////////////////////////////////////
function AddApplicationCash(currencyId) {
    var index = 0;
    var description = GetCurrencyDescription(currencyId);
    var response = GetCurrencyRateBilling($("#ViewBagDateAccounting").val(), currencyId);
    currentRate = RemoveFormatMoney(response[0], 6);

    var length = 0;
    var j;
    var isEditing = false;

    // Se valida que no ingrese más de un registro en efectivo por tipo de moneda
    var cashs = $("#modalCashAdd").find("#cashsListView").UifListView("getData");

    if (cashs != null) {
        length = cashs.length;
        for (var j = 0; j < cashs.length; j++) {
            if (cashs[j].CurrencyId == currencyId) {
                isEditing = true;
                index = j;
                break;
            }
        }
    }

    var rowModel = new RowPaymentModel();

    rowModel.PaymentTypeId = $("#ViewBagParamPaymentMethodCash").val();
    rowModel.PaymentTypeDescription = "EFECTIVO";
    rowModel.CurrencyId = currencyId;
    rowModel.Currency = description;
    rowModel.Amount = $('#TotalApplicationCash').val();
    rowModel.ExchangeRate = FormatMoneySymbol(currentRate, 6);
    rowModel.LocalAmount = FormatMoneySymbol(RemoveFormatMoney($('#TotalApplicationCash').val()) * currentRate);
    rowModel.DocumentNumber = "";
    rowModel.VoucherNumber = "";
    rowModel.CardNumber = "";
    rowModel.CardType = "";
    rowModel.CardTypeName = "";
    rowModel.AuthorizationNumber = "";
    rowModel.CardsName = "";
    rowModel.ValidThruMonth = "";
    rowModel.ValidThruYear = "";
    rowModel.IssuingBankId = "";
    rowModel.IssuingBankName = "";
    rowModel.IssuingBankAccountNumber = "";
    rowModel.IssuerName = "";
    rowModel.RecievingBankId = "";
    rowModel.RecievingBankName = "";
    rowModel.RecievingBankAccountNumber = "";
    rowModel.SerialVoucher = "";
    rowModel.SerialNumber = "";
    rowModel.Date = "";
    rowModel.TaxBase = "";
    rowModel.Information = "";

    if (isEditing === false) {
        $("#modalCashAdd").find('#cashsListView').UifListView("addItem", rowModel);
    }
    else {
        $("#modalCashAdd").find('#cashsListView').UifListView("editItem", index, rowModel);
    }

    SetTotalPayment("E");
    SetCashSummary();
    SetTotalApplicationPayment();
    currentRate = 0;
}

//////////////////////////////////////////////////
// Elimina el registro de efectivo del listview //
//////////////////////////////////////////////////
function DeleteSummaryCash(currencyId) {
    var index = -1;
    var cashs = $("#summariesListView").UifListView("getData");
    var cashAmount = 0;

    if (cashs != null) {
        for (var j = 0; j < cashs.length; j++) {
            if (cashs[j].CurrencyId == currencyId) {
                index = j;
                cashAmount = cashs[j].LocalAmount;
            }
        }
    }
    if (index > -1) {
        $("#alert").UifAlert('show', Resources.DeleteSummaryCash, "warning");
        $("#TotalApplicationCash").val(FormatMoneySymbol(cashAmount));
    }
}

var deleteSummaryCashCallback1 = function (deferred, data) {
    deferred.resolve();
    if (data.PaymentMethod == "Efectivo") {
        $("#modalCashAdd").find("#cashsListView").UifListView({
            autoHeight: true, customAdd: false, customEdit: true, localMode: true, customDelete: false, add: false, edit: true,
            delete: true, displayTemplate: "#cash-display-template", editCallback: editCashCallback, deleteCallback: deleteCashCallback
        });
        $("#TotalApplicationCash").val("");
        SetTotalPayment("E");
        SetMovementsSummary("E");

    }
    if (data.PaymentMethod == "Cheque") {
        $("#modalCheckAdd").find("#checksListView").UifListView({
            autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
            delete: true, displayTemplate: "#check-display-template", editCallback: editCheckCallback, deleteCallback: deleteCheckCallback
        });
        SetTotalPayment("C");
        SetMovementsSummary("C");
    }
    if (data.PaymentMethod == "Consignación") {
        LoadconsigListView();
        SetTotalPayment("B");
        SetMovementsSummary("B");
    }
    if (data.PaymentMethod == "Tarjeta") {
        LoadcardsListView();
        SetTotalPayment("T");
        SetMovementsSummary("T");
    }
    if (data.PaymentMethod == "Transferencia") {
        $("#modalTransferAdd").find("#transferListView").UifListView({
            autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
            delete: true, displayTemplate: "#transfer-display-template", editCallback: editTransferCallback, deleteCallback: deleteTransferCallback
        });
        SetTotalPayment("TR");
        SetMovementsSummary("TR");
    }
    if (data.PaymentMethod == "Datafono") {
        $("#modalDatafonoAdd").find("#datafonoListView").UifListView({
            autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
            delete: true, displayTemplate: "#datafono-display-template", editCallback: editDatafonoCallback, deleteCallback: deleteDatafonoCallback
        });
        SetTotalPayment("D");
        SetMovementsSummary("D");
    }

    if (data.PaymentMethod == "Retenciones") {
        $("#modalRetentionAdd").find("#retentionListView").UifListView({
            autoHeight: true, customAdd: false, customEdit: true, customDelete: false, localMode: true, add: false, edit: true,
            delete: true, displayTemplate: "#retention-display-template", editCallback: editRetentionCallback, deleteCallback: deleteRetentionCallback
        });
        SetTotalPayment("D");
        SetMovementsSummary("D");
    }
    SetTotalApplicationPayment();
};

var switchPerson = true;
var documentTypeId = 0;

//funcion para cargar datos para aplicar
function loadBilling() {

    $("#inputDocumentNumber").val($("#ViewBagDocumentNumber").val());
    $("#inputName").val($("#ViewBagName").val());
    $("#Observation").val(Resources.ObservationByPreliquidation + $("#ViewBagPreliquidationId").val());

    preliquidationBranch = parseInt($("#ViewBagPreliquidationBranch").val());
    $("#selectBranch").val(preliquidationBranch);

    var totalPreliquidation = Math.abs(RemoveFormatMoney($("#ViewBagTotalAmount").val()));
    $("#TotalApplication").val(FormatMoneySymbol(totalPreliquidation));

    payerId = $("#ViewBagIndividualId").val();
    $("#CollectionTo").val($("#ViewBagPreliquidationPersonTypeId").val());

    if ($("#DocumentType").val() == "" && switchPerson) {
        switchPerson = false;
        MainBillingRequest.GetPersonsByIndividualId(payerId)
            .done(function (data) {
                if (data.length > 0) {

                    documentTypeId = data[0].DocumentTypeId;

                    //recupera las pólizas
                    if ($("#policiesListView").UifListView("getData").length == 0) {

                        loadPremiumsReceivable($("#ViewBagTempImputationId").val());
                    }
                    $("#DocumentType").val(documentTypeId);
                }
            })
            .always(function () {
                loadAllFieldsBilling();
            });
    }
}

function OpenBranchDefault() {
    $("#alert").UifAlert('hide');
    validateOpenBill($('#selectBranch').UifSelect('getSelected'));
}

function isEmpty(text) {
    return text == undefined || text == null || (typeof (text) === 'string' && text === "");
}

function validateOpenBill(branchId, isClosure = false) {
    var _accountingDate = $("#ViewBagDateAccounting").val();
    if (!isEmpty(branchId) && !isEmpty(_accountingDate)) {
        MainBillingRequest.NeedCloseBill(branchId, _accountingDate)
            .done(function (userData) {
                if (userData[0].success == false) {
                    $("#alert").UifAlert('show', userData.result, "danger");
                    if (isClosure === true) {
                        resultClosureBillingMain = false;
                        $.unblockUI();
                    }
                } else if (userData[0].resp == true) {
                    $.UifNotify('show', {
                        'type': 'info', 'message': Resources.IncomeMadeAccountingDate + userData[0].accountingDate, 'autoclose': false
                    });
                    isOpen = true;
                    if (isClosure === true) {
                        resultClosureBillingMain = false;
                        $.unblockUI();
                    }
                } else {
                    MainBillingRequest.AllowOpenBill(branchId, _accountingDate)
                        .done(function (data) {
                            if (data[0].resp == true) {
                                OpenBillingDialogMain();
                            }
                            else {
                                isOpen = true;
                                if (isClosure === true) {
                                    resultClosureBillingMain = false;
                                    $.unblockUI();
                                }
                            }
                        });
                }
                billControlId = userData[0].Id;
            });
    }
}

function validateExpirationDate() {
    if ($("#RetentionExpirationDate").val() < $("#ViewBagDateAccounting").val()) {
        setTimeout(function () {
            $("#modalRetentionAdd").find("#alertForm").UifAlert('show', Resources.validateExpirationDate, "warning");
        }, 300);

        $("#RetentionExpirationDate").val('');
    }
}

//////////////////////////////////////////////////
// Da formato a un número para su visualización //
//////////////////////////////////////////////////
function NumberFormat(number, decimals, decimalSeparator, thousandsSeparator) {
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

//////////////////////////////////////////////////
// Poner foco en control                        //
//////////////////////////////////////////////////
function setFocusControl(idElemento) {
    document.getElementById(idElemento).focus();
}

//////////////////////////////////////////////////
// Poner valores autocomplete                  //
//////////////////////////////////////////////////

function setValueAutoComplete(identifier, selectedItem) {
    $('#' + identifier + 'DocumentNumber').UifAutoComplete('setValue', selectedItem.DocumentNumber);
    $('#' + identifier + 'Name').UifAutoComplete('setValue', selectedItem.Name);
    personDocumentNumber = selectedItem.DocumentNumber;
    personName = selectedItem.Name;
    $("#DocumentType").val(selectedItem.DocumentTypeId);
}

function setDocumentTypeId(individualId) {
    $.ajax({
        async: false,
        type: "GET",
        url: ACC_ROOT + "Billing/GetPersonsByIndividualId",
        data: { "individualId": parseInt(individualId) },
        success: function (data) {
            // Ajuste Jira SMT-1544 Inicio
            if (data.length > 0) {
                setTimeout(function () {
                    $("#DocumentType").val(data[0].DocumentTypeId);
                }, 600);
            }
            // Ajuste Jira SMT-1544 FIn
        }
    });
}

$(document).ready(function () {
    LoadcardsListView();
    LoadconsigListView();
    LoadConsignmentCheckListView();
    GetAvaibleCurrencies();
});

function GetAvaibleCurrencies() {
    MainBillingRequest.GetAvaibleCurrencies().done(function (response) {
        if (!isNull(response) && !isEmptyArray(response.data)) {
            $('#modalCheckAdd').find('#selectCheckCurrency').UifSelect({ sourceData: response.data });
            $('#modalCashAdd').find('#selectCashCurrency').UifSelect({ sourceData: response.data });
            $('#modalConsignmentAdd').find('#selectConsignmentCurrency').UifSelect({ sourceData: response.data });
            $('#modalConsignmentCheckAdd').find('#selectConsignmentCheckCurrency').UifSelect({ sourceData: response.data });
        }
    });
}

function LoadDefaultCurrency(control) {
    control.UifSelect('setSelected', $('#ViewBagDefaultCurrency').val());
    control.trigger('change');
}

function LoadConsignmentCheckListView() {
    $("#addConsignmentCheckForm").find("#ConsignmentChecksListView").UifListView({
        autoHeight: true,
        customAdd: false,
        customEdit: true,
        customDelete: false,
        localMode: true,
        add: false,
        edit: true,
        delete: true, displayTemplate: "#ConsignmentCheck-display-template", editCallback: editConsignmentCheckCallback, deleteCallback: deleteConsignmentCheckCallback
    });
}


function previousValidAmount(identifier) {
    var clearAmount = ClearFormatCurrency($('#' + identifier).val());
    var validAmount = clearAmount.split(".");

    if (validAmount[1] == 0) {
        clearAmount = validAmount[0];
    }
    $('#' + identifier).val(clearAmount.trim());
}

function LoadcardsListView() {
    $("#modalCardAdd").find("#cardsListView").UifListView({
        autoHeight: true,
        customAdd: false, customEdit: true, customDelete: false,
        localMode: true,
        add: false, edit: true, delete: true,
        displayTemplate: "#card-display-template", editCallback: editCreditCardCallback, deleteCallback: deleteCreditCardCallback
    });
}

function LoadconsigListView() {
    $("#modalConsignmentAdd").find("#consignmentsListView").UifListView({
        autoHeight: true,
        customAdd: false,
        customEdit: true,
        customDelete: false,
        localMode: true,
        add: false,
        edit: true,
        delete: true, displayTemplate: "#consignment-display-template", editCallback: editConsignmentCallback, deleteCallback: deleteConsignmentCallback
    });
}

function GetPolicyId() {
    $("#addRetentionForm").find("#alertForm").UifAlert('hide');

    if ($("#addRetentionForm").find('#RetentionPolicy').val() != "") {
        $.ajax({
            url: ACC_ROOT + 'Billing/GetPolicyIdByBranchPrefixPolicyNumber',
            data: {
                'branchId': $("#addRetentionForm").find('#selectRetentionBranch').val(),
                'prefixId': $("#addRetentionForm").find('#selectRetentionPrefix').val(),
                'policyNumber': $("#addRetentionForm").find('#RetentionPolicy').val()
            },
        }).done(function (data) {
            if (data[0].PolicyId > 0) {
                retentionPolicyId = data[0].PolicyId;
                $("#addRetentionForm").find('#RetentionEndorsement').val(data[0].EndorsementNumber);
                $("#addRetentionForm").find('#RetentionExpirationDate').val(data[0].ExpirationDate);
            }
            else {
                $("#addRetentionForm").find("#alertForm").UifAlert('show', Resources.PolicyNotExists, "warning");
                $("#addRetentionForm").find('#RetentionPolicy').val("");
                $("#addRetentionForm").find('#RetentionEndorsement').val("");
                $("#addRetentionForm").find('#RetentionExpirationDate').val("");
            }
        });
    }
}


setTimeout(function () {
    $("#uif-spinner").css("display", "none");
}, 5000);


function ValidateExchangeRate(fieldId, currencyFieldId, amountFieldId, localAmountFieldId, modalId, showMessage = true) {
    newRate = RemoveFormatMoney($('#' + fieldId).val());
    if (newRate === currentRate || isNaN(newRate) || newRate === 0) {
        $('#' + fieldId).val(FormatMoneySymbol(currentRate, 6));
    } else {
        validateExchangeRateTolerance(newRate, fieldId, currencyFieldId, amountFieldId, localAmountFieldId, modalId, showMessage);
    }
}

function loadBanks(modalId, bankElementId, currencyId, accountingBankElementId, defaultValue) {
    var modalItem = $('#' + modalId);
    if (modalItem.length == 0) {
        return;
    } 
    var bankItem = $('#' + modalId).find('#' + bankElementId);
    if (bankItem.length == 0) {
        return;
    }
    MainBillingRequest.GetAvaibleBanksByCurrencyId(currencyId).done(function (response) {
        if (!isNull(response) && !isEmptyArray(response.data)) {
            $('#' + modalId).find('#' + bankElementId).UifSelect({ sourceData: response.data });
            var accountingBankElement = $('#' + modalId).find('#' + accountingBankElementId);
            if (accountingBankElement.length > 0) {
                $('#' + modalId).find('#' + accountingBankElementId).UifSelect({ sourceData: [] });
            }
            if (isEmptyorZero(defaultValue) && response.data.length == 1) {
                defaultValue = response.data[0].Id;
            }
            if (!isEmptyorZero(defaultValue)) {
                $('#' + modalId).find('#' + bankElementId).UifSelect('setSelected', defaultValue);
                $('#' + modalId).find('#' + bankElementId).trigger('change');
            }
        }
    });
}

function loadAccountingBanks(modalId, accountingBankElementId, currencyId, bankId, defaultValue) {
    var modalItem = $('#' + modalId);
    if (modalItem.length == 0) {
        return;
    }
    var accountingBankElement = $('#' + modalId).find('#' + accountingBankElementId);
    if (accountingBankElement.length == 0) {
        return;
    }
    MainBillingRequest.GetAvaibleAccoutingBanksByCurrencyIdBankId(currencyId, bankId).done(function (response) {
        if (!isNull(response) && !isEmptyArray(response.data)) {
            $('#' + modalId).find('#' + accountingBankElementId).UifSelect({ sourceData: response.data });
            
            if (isEmptyorZero(defaultValue) && response.data.length == 1) {
                defaultValue = response.data[0].Id;
            }
            if (!isEmptyorZero(defaultValue)) {
                $('#' + modalId).find('#' + accountingBankElementId).UifSelect('setSelected', defaultValue);
                $('#' + modalId).find('#' + accountingBankElementId).trigger('change');
            }
        }
    });
}

function validateRiksListPerson() {
    validateInformation('inputDocumentNumber', 'inputName');
}

function validateRiksListAgent() {
    validateInformation('inputAgentDocumentNumber', 'inputAgentName');
}

function validateRiksListCompany() {
    validateInformation('inputCompanyDocumentNumber', 'inputCompanyName');
}

function validateRiksListOther() {
    validateInformation('inputOtherDocumentNumber', 'inputOtherName');
}

function validateRiksListThird() {
    validateInformation('inputThirdDocumentNumber', 'inputThirdName');
}

function validateRiksListInsured() {
    validateInformation('inputInsuredDocumentNumber', 'inputInsuredName');
}

function validateRiksListContractor() {
    validateInformation('inputContractorDocumentNumber', 'inputContractorName');
}

function validateRiksListSuplier() {
    validateInformation('inputSupplierDocumentNumber', 'inputSupplierName');
}

function validateRiksListReinsurance() {
    validateInformation('inputReinsuranceDocumentNumber', 'inputReinsuranceName');
}

function validateInformation(documentNumberInputId, fullNameInputId) {
    var documentNumber = $('#' + documentNumberInputId).val();
    var fullName = $('#' + fullNameInputId).val(    );

    if (!isEmpty(documentNumber) || !isEmpty(fullName)) {
        validateRisksList(documentNumber, fullName);
    } else {
        cleanRiskList();
    }
}

function validateRisksList(documentNumber, fullName) {
    MainBillingRequest.ValidateListRiskPerson(documentNumber, fullName).done(function (data) {
        if (!isNull(data) && data.success && !isNull(data.result)) {
            $("#alert-risk-list").UifAlert('show', data.result, "danger");
        } else {
            cleanRiskList();
        }
    }).fail(function () {
        $("#alert-risk-list").UifAlert('show', Resources.ErrorCouldNotCheckRisksList, "danger");
        setTimeout(function () {
            cleanRiskList();
        }, 9000);
    });
}

function cleanRiskList() {
    $("#alert-risk-list").UifAlert('hide');
}

function confirmModal() {
    cleanRiskList();
    if (!IsNewMainBilling) {
        setTimeout(function () {
            location.href = $("#ViewBagMainPreLiquidationsSearchLink").val();
        }, 1000);
    }
}

function getNextTicketId(control) {
    MainBillingRequest.GetNextSequence().done(function (result) {
        if (result.success === true) {
            $(control).val(result.result);
        } else {
            $("#alert").UifAlert('show', result.result, "warning");
        }
    });
}