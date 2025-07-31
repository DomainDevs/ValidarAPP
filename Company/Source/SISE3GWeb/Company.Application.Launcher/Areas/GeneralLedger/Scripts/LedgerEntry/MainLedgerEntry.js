/**
    * @file   MainLedgerEntry.js
    * @author Desarrollador
    * @version 0.1
    */

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                            DEFINICIÓN DE OBJETOS / VARIABLES / ARRAYS                                           */
/*---------------------------------------------------------------------------------------------------------------------------------*/
var timeLedgerEntry = 0;
var currentEditIndex = 0;
var rowSelected;
var postDatedExchangeRate = 0;
var tablePosition = -1;
var tableTypeId = 0;
var entryTypeId = 0;
var lastCostCenterPercentageValue = 0;
var entryEdit;
var entryCostCenterEdit;
var entryAnalysisEdit;
var entryPostDatedEdit;
var modalDelete;
var printDataEntryNumber = 0;
var dataTable = null;
var copyModal = null;
var chooseModal = null;
var documentNumberEntry = "";
var accounts = [];
var isEntryType = 0;
var keyLength = 0;
var analysisCurrentEditIndex = 0;
var postdatedExchangeRate = 1;
var postdatedCurrentEditIndex = 0;
var costCenterCurrentEditIndex = 0;
var thirdAccountingUsed = parseInt($("#ViewBagThirdAccountingUsed").val());



//Definición de modelos para la tabla de Movimientos
function RowModel() {
    this.BranchId;
    this.SalePointId;
    this.CompanyId;
    this.DestinationId;
    this.AccountingMovementTypeId;
    this.Date;
    this.AccountingAccountId;
    this.AccountingAccountNumber;
    this.AccountingAccountDescription;
    this.AccountingNatureId;
    this.AccountingNatureDescription;
    this.CurrencyId;
    this.CurrencyDescription;
    this.ExchangeRate;
    this.Amount;
    this.IndividualId;
    this.DocumentNumber;
    this.Name;
    this.ConceptId;
    this.ConceptDescription;
    this.BankReconciliationId;
    this.BankReconciliationDescription;
    this.ReceiptNumber;
    this.ReceiptDate;
    this.Description;
    this.CostCenters = [];
    this.Analysis = [];
    this.Postdated = [];
}

//Definición de modelos para la tabla de Centro de Costos
function CostCenterRowModel() {
    this.CostCenterId;
    this.Description;
    this.PercentageAmount;
}

//Definición de modelos para la tabla de Análisis
function AnalysisRowModel() {
    this.AnalysisId;
    this.AnalysisDescription;
    this.AnalysisConceptId;
    this.AnalysisConceptDescription;
    this.Key;
    this.Description;
}

//Definición de modelos para la tabla de Posfechados
function PostDatedRowModel() {
    this.PostDateTypeId;
    this.DocumentNumber;
    this.CurrencyId;
    this.ExchangeRate;
    this.IssueAmount;
    this.LocalAmount;
}

var oLedgerEntryModel = {
    LedgerEntryId: 0,
    AccountingCompanyId: 0,
    AccountingMovementTypeId: 0,
    AccountingModuleId: 0,
    BranchId: 0,
    SalePointId: 0,
    EntryDestinationId: 0,
    Description: null,
    Date: null,
    LedgerEntryItems: []
};

var oLedgerEntryItemModel = {
    LedgerEntryItemId: 0,
    CurrencyId: 0,
    AccountingAccountId: 0,
    AccountingAccountNumber: null,
    BankReconciliationId: 0,
    ReceiptDate: null,
    ReceiptNumber: 0,
    AccountingNatureId: 0,
    Description: null,
    ExchangeRate: 0,
    Amount: 0,
    LocalAmount: 0,
    IndividualId: 0,
    DocumentNumber: null,
    EntryTypeId: 0,
    CostCenters: [],
    Analysis: [],
    Postdated: []
};

var oCostCenterModel = {
    CostCenterId: 0,
    Description: null,
    PercentageAmount: 0
};

var oAnalysisModel = {
    AnalysisId: 0,
    AnalysisDescription: null,
    AnalysisConceptId: 0,
    AnalysisConceptDescription: null,
    Key: null,
    Description: null
};

var oPostDatedModel = {
    PostDateTypeId: 0,
    PostDateTypeDescription: null,
    DocumentNumber: 0,
    CurrencyId: 0,
    CurrencyDescription: null,
    ExchangeRate: 0,
    IssueAmount: 0,
    LocalAmount: 0
};

/*---------------------------------------------------------------------------------------------------------------------------------*/
/*                                                      DEFINICIÓN DE CLASE                                                        */
/*---------------------------------------------------------------------------------------------------------------------------------*/


$(() => {
    new MainLedgerEntry();
});

class MainLedgerEntry extends Uif2.Page {

    /**
        * Inicializa los componentes por default.
        *
        */
    getInitialState() {
        $("#ddlAccountingCompanies").attr("disabled", "disabled");
        $("#ddlAccountingCompanies").prop("disabled", "disabled");

        //MainLedgerEntry.GetDate();
        MainLedgerEntry.LoadEntryTypes();
        setTimeout(function () {
            try {
                MainLedgerEntry.GetAccountingDate(Resources.AccountingModuleDateId);
                MainLedgerEntry.SetSalePointEntry();
            } catch (e) { }
        }, 400);

        setTimeout(function () {
            $("#ddlAccountingCompanies").attr("disabled", "disabled");
        }, 1000);
    }

    /**
        * Enlaza los eventos de los componentes de la pantalla.
        *
        */
    bindEvents() {
        $("#btnAddMovement").on("click", this.ButtonAddMovements);
        $("#entryTable").on("rowSelected", this.RowSelectedLedgerEntry);
        $("#entryTypes").on("click", "li", this.ShowEntryTypes);
        $("#modalChoose").on("click", "#btnChoose", this.ChooseEntryType);
        $("#entryTable").on("rowDelete", this.RowDeleteLedgerEntry);
        $("#entryTable").on("rowEdit", this.RowEditLedgerEntry);
        $("#btnEntrySaveAdd").on("click", this.SaveAddLedgerEntry);
        $("#btnEntrySaveEdit").on("click", this.SaveEditLedgerEntry);
        $("#btnEntryCancelAdd").on("click", this.CancelAddLedgerEntry);
        $("#btnEntryCancelEdit").on("click", this.CancelEditLedgerEntry);
        $("#tblCostCenter").on("rowAdd", this.RowAddCostCenter);
        $("#tblCostCenter").on("rowDelete", this.RowDeleteCostCenter);
        $("#tblCostCenter").on("rowEdit", this.RowEditCostCenter);
        $("#btnSaveAddCostCenter").on("click", this.SaveAddCostCenter);
        $("#btnSaveEditCostCenter").on("click", this.SaveEditCostCenter);
        $("#btnCancelAddCostCenter").on("click", this.CancelAddCostCenter);
        $("#btnCancelEditCostCenter").on("click", this.CancelEditCostCenter);
        $("#tblAnalysis").on("rowAdd", this.RowAddAnalysis);
        $("#tblAnalysis").on("rowDelete", this.RowDeleteAnalysis);
        $("#tblAnalysis").on("rowEdit", this.RowEditAnalysis);
        $("#btnCancelSaveAddAnalysis").on("click", this.SaveAddAnalysis);
        $("#btnEntrySaveEditAnalysis").on("click", this.SaveEditAnalysis);
        $("#btnEntryCancelAddAnalysis").on("click", this.CancelAddAnalysis);
        $("#btnEntryCancelEditAnalysis").on("click", this.CancelEditAnalysis);
        $("#tblPostdated").on("rowAdd", this.RowAddPostDated);
        $("#tblPostdated").on("rowDelete", this.RowDetelePostDated);
        $("#tblPostdated").on("rowEdit", this.RowEditPostDated);
        $("#btnEntrySaveAddFrmPostdated").on("click", this.SaveAddPostDated);
        $("#btnEntrySaveEditFrmPostdated").on("click", this.SaveEditPostDated);
        $("#btnEntryCancelAddFrmPostdated").on("click", this.CancelAddPostDated);
        $("#btnEntryCancelEditFrmPostdated").on("click", this.CancelEditPostDated);
        $("#btnSaveAllEntry").on("click", this.SaveLedgerEntry);
        $("#btnChoose").on("click", this.ChoosePrint);
        $("#printEntryConfirmationAccept").on("click", this.ConfirmPrintLedgerEntry);
        $("#entryModalAdd").on("keypress", "#Amount", this.KeyPressAddAmount);
        $("#entryModalEdit").on("keypress", "#Amount", this.KeyPressEditAmount);
        $("#entryAddFrmCostCenter").on("keypress", "#PercentageAmount", this.KeyPressAddPercentageAmount)
        $("#entryEditFrmCostCenter").on("keypress", "#PercentageAmount", this.KeyPressEditPercentageAmount)
        $("#entryAddFrmPostdated").on("keypress", "#IssueAmount", this.KeyPressAddIssueAmount);
        $("#entryEditFrmPostdated").on("keypress", "#DocumentNumber", this.KeyPressEditDocumentNumber);
        $("#entryAddFrmPostdated").on("keypress", "#DocumentNumber", this.KeyPressAddDocumentNumber);
        $("#entryEditFrmPostdated").on("keypress", "#IssueAmount", this.KeyPressEditIssueAmount);
        $("#entryModalAdd").on("itemSelected", "#entryPayerDocumentNumber", this.AutocompleteAddPayerDocumentNumber);
        $("#entryModalAdd").on("blur", "#entryPayerDocumentNumber", this.BlurAddPayerDocumentNumber);
        $("#entryModalEdit").on("itemSelected", "#entryPayerDocumentNumber", this.AutocompleteEditPayerDocumentNumber);
        $("#entryModalEdit").on("blur", "#entryPayerDocumentNumber", this.BlurEditPayerDocumentNumber);
        $("#entryModalAdd").on("itemSelected", "#conceptId", this.AutocompleteAddAccountingConcept);
        $("#entryModalEdit").on("itemSelected", "#conceptId", this.AutocompleteEditAccountingConcept);
        $("#entryModalAdd").on("itemSelected", "#conceptDescription", this.AutocompleteAddAccountingConceptDescription);
        $("#entryModalEdit").on("itemSelected", "#conceptDescription", this.AutocompleteEditAccountingConceptDescription);
        $("#entryModalAdd").on("itemSelected", "#accountingAccountNumber", this.AutocompleteAddAccountingAccount);
        $("#entryModalEdit").on("itemSelected", "#accountingAccountNumber", this.AutocompleteEditAccountingAccount);
        $("#ddlBranches").on("itemSelected", this.ItemSelectedBranch);
        $("#entryModalAdd").find("#ddlCurrencies").on("itemSelected", this.ItemSelectedCurrency);
        $("#entryModalEdit").find("#ddlCurrencies").on("itemSelected", this.ItemSelectedCurrency);
        $("#entryAddFrmPostdated").on("itemSelected", "#CurrencyId", this.ItemSelectedAddCurrency);
        $("#entryAddFrmPostdated").on("blur", "#IssueAmount", this.BlurAddIssueAmount);
        $("#entryEditFrmPostdated").on("itemSelected", "#CurrencyId", this.ItemSelectedEditCurrency);
        $("#entryEditFrmPostdated").on("blur", "#IssueAmount", this.BlurEditIssueAmount);
        $("#entryAddForm").on("itemSelected", "#accountingAccountNumber", this.AutocompleteAddAccountingAccountNumber);
        $("#entryEditForm").on("itemSelected", "#accountingAccountNumber", this.AutocompleteEditAccountingAccountNumber);
        $("#entryAddFrmCostCenter").on("itemSelected", "#CostCenterId", this.AutocompleteAddCostCenter);
        $("#entryEditFrmCostCenter").on("itemSelected", "#CostCenterId", this.AutocompleteEditCostCenter);
        $("#entryAddFrmCostCenter").on("itemSelected", "#Description", this.AutocompleteAddCostCenterDescription);
        $("#entryEditFrmCostCenter").on("itemSelected", "#Description", this.AutocompleteEditCostCenterDescription);
        $("#entryAddFrmAnalysis").on("itemSelected", "#AnalysisId", this.ItemSelectedAddAnalysis);
        $("#entryEditFrmAnalysis").on("itemSelected", "#AnalysisId", this.ItemSelectedEditAnalysis);
        $("#entryDeleteModalWarningAccept").on("click", this.DeleteWarningAccept);
        $("#EntryTypeModalWarning").find("#EntryTypeModalWarningAccept").on("click", this.AcceptEntryTypewarning);
        $("#ddlAccountingCompanies").on("binded", this.BindedAccountingCompany);
        $("#entryAddFrmAnalysis").find('#AnalysisConceptId').on("itemSelected", this.ItemSelectedAddAnalysisConcept);
        $("#entryEditFrmAnalysis").find('#AnalysisConceptId').on("itemSelected", this.ItemSelectedEditAnalysisConcept);
        $("#ddlSalePoints").on("binded", this.BindedSalePoint);
        $('#entryModalAdd').on('closed.modal', this.closedModal);
    }

     /**
        * Setea  el uso del tercero en contabilidad Habilitado o desabilitado
        *
        */
     static LockDocumentNumberAdd() {
      
         if (thirdAccountingUsed == 0) {
            $("#entryAddForm").find('#entryPayerDocumentNumber').attr("disabled", true);
        } else {
            $("#entryAddForm").find('#entryPayerDocumentNumber').attr("disabled", false);
        }
        // DANC 2018-06-12
    }

    closedModal() {
        $('#conceptId').UifAutoComplete('clean');
        $('#conceptDescription').UifAutoComplete('clean');
        $('#accountingAccountNumber').UifAutoComplete('clean');
        $('#entryPayerDocumentNumber').UifAutoComplete('clean');
    }

    static LockDocumentNumberEdit() {

        if (thirdAccountingUsed == 0) {
            $("#entryEditForm").find('#entryPayerDocumentNumber').attr("disabled", true);
        } else {
            $("#entryEditForm").find('#entryPayerDocumentNumber').attr("disabled", false);
        }

    }

    


    /**
        * Setea la compañía contable por default una vez que esta cargado.
        *
        */
    BindedAccountingCompany() {
        if (parseInt($("#ViewBagParameterMulticompany").val()) == 0) {

            $("#ddlAccountingCompanies").attr("disabled", true);
        } else {
            $("#ddlAccountingCompanies").attr("disabled", false);
        }
    }

    /**
        * Setea el punto de venta por default una vez que esta cargado.
        *
        */
    BindedSalePoint() {
        //$("#ddlSalePoints").val($("#ViewBagSalePointBranchUserDefault").val());
        $("#ddlSalePoints").val("");
    }

    /**
        * Permite agregar movimientos contables.
        *
        */
    ButtonAddMovements() {
        isEntryType = 0;
        $("#mainForm").validate();
        if ($("#mainForm").valid()) {
            if (entryTypeId != 0) {
                $("#EntryTypeModalWarning").UifModal('showLocal', Resources.Warning);
            } else {
                $('#entryModalAdd').UifModal('showLocal', Resources.AddMovement);
                MainLedgerEntry.LockFields();
                MainLedgerEntry.LockDocumentNumberAdd();
                entryTypeId = 0;
            }
        }
    }

    /**
        * Permite seleccionar un movimiento para su edición.
        *
        * @param {String} event - Seleccionar.
        * @param {Object} data  - Objeto con valores del movimiento seleccionado.
        */
    RowSelectedLedgerEntry(event, data) {
        rowSelected = data;

        // Se obtiene los centros de costos asociados
        if (rowSelected.CostCenters.length > 0) {

            $('#tblCostCenter').dataTable().fnClearTable();

            for (var i = 0; i < rowSelected.CostCenters.length; i++) {
                $('#tblCostCenter').UifDataTable('addRow', rowSelected.CostCenters[i]);
            }

            // Se valido que los centros de costos lleguen al 100%
            if (MainLedgerEntry.CostCenterPercentageTotal() < 100) {

            }
        } else {
            $('#tblCostCenter').dataTable().fnClearTable();
        }

        // Se obtiene los análisis asociados
        if (rowSelected.Analysis.length > 0) {
            $('#tblAnalysis').dataTable().fnClearTable();
            for (var j = 0; j < rowSelected.Analysis.length; j++) {
                $('#tblAnalysis').UifDataTable('addRow', rowSelected.Analysis[j]);
            }
        } else {
            $('#tblAnalysis').dataTable().fnClearTable();
        }

        // Se obtiene los postfechados asociados
        if (rowSelected.Postdated.length > 0) {
            $('#tblPostdated').dataTable().fnClearTable();
            for (var k = 0; k < rowSelected.Postdated.length; k++) {
                $('#tblPostdated').UifDataTable('addRow', rowSelected.Postdated[k]);
            }
        } else {
            $('#tblPostdated').dataTable().fnClearTable();
        }
    }


    /**
        * Permite visaulizar los asientos tipo.
        *
        * @param {String} event    - Seleccionar.
        * @param {Object} data     - Objeto con valores del asiento tipo seleccionado.
        * @param {Number} position - Número de posición del asiento tipo seleccionado.
        */
    ShowEntryTypes(event, data, position) {
        isEntryType = 1;
        entryTypeId = $(this).attr('value');
        $("#mainForm").validate();
        if ($("#mainForm").valid()){
            if ($('#entryTable').UifDataTable('getData').length != 0) {
                $("#EntryTypeModalWarning").UifModal("showLocal", Resources.Warning);
            } else {
                MainLedgerEntry.AddEntryType(entryTypeId);
            }
        }    
    }

    /**
        * Permite escoger el asiento tipo parametrizado.
        *
        * @param {String} event    - Seleccionar.
        * @param {Object} data     - Objeto con valores del asiento tipo seleccionado.
        * @param {Number} position - Número de posición del asiento tipo seleccionado.
        */
    ChooseEntryType(event, data, position) {
        $('#entryTable').dataTable().fnClearTable();
        $('#tblCostCenter').dataTable().fnClearTable();
        $('#tblAnalysis').dataTable().fnClearTable();
        if (entryTypeId != 0) {
            mainLedgerEntry.AddEntryType(entryTypeId);
        }
    }

    /**
        * Permite eliminar una asiento de mayor temporal.
        *
        * @param {String} event    - Eliminar.
        * @param {Object} data     - Objeto con valores del asiento seleccionado.
        * @param {Number} position - Número de posición del asiento seleccionado.
        */
    RowDeleteLedgerEntry(event, data, position) {

        $("#entryDeleteModalWarning").UifModal('showLocal', Resources.Warning);

        tablePosition = position;
        tableTypeId = 4;
    }

    /**
        * Permite editar un asiento de mayor.
        *
        * @param {String} event    - Editar.
        * @param {Object} data     - Objeto con valores del asiento seleccionado.
        * @param {Number} position - Indice del asiento seleccionado.
        */
    RowEditLedgerEntry(event, data, position) {

        entryEdit = $("#entryModalEdit").detach();
        currentEditIndex = position;

        entryEdit.appendTo("body").UifModal('showLocal', Resources.Edit); //anexa el modal al cuerpo de la aplicación, evita el bloqueo de la pantalla
        $("#entryEditForm").appendTo("body"); //anexa el formulario dentro del modal al cuerpo de la aplicación
        $("#entryModalEdit").appendTo("#entryEditForm"); //anexa el modal al formulario para que no se pierdan los elementos de validación del formulario.

        $("#entryEditForm").find("#AccountingAccountId").val(data.AccountingAccountId);
        $("#entryEditForm").find("#conceptId").val(data.ConceptId);
        $("#entryEditForm").find("#conceptDescription").val(data.ConceptDescription);
        $("#entryEditForm").find("#accountingAccountNumber").val(data.AccountingAccountNumber);
        $("#entryEditForm").find("#accountingAccountDescription").val(data.AccountingAccountDescription);
        $("#entryEditForm").find("#AccountingNatureId").val(data.AccountingNatureId);
        $("#entryEditForm").find("#ddlCurrencies").val(data.CurrencyId);
        $("#entryEditForm").find("#exchange").val(data.ExchangeRate);
        $("#entryEditForm").find("#Amount").val(parseFloat(ClearFormatCurrency(data.AmountFormated)));
        $("#entryEditForm").find("#IndividualId").val(data.IndividualId);
        $("#entryEditForm").find("#entryPayerDocumentNumber").val(data.DocumentNumber);
        $("#entryEditForm").find("#entryPayerName").val(data.Name);
        $("#entryEditForm").find("#Description").val(data.Description);
        $("#entryEditForm").find("#BankReconciliationId").val(data.BankReconciliationId);
        $("#entryEditForm").find("#ReceiptNumber").val(data.ReceiptNumber);
        $("#entryEditForm").find("#receiptDate").val(data.ReceiptDate);

        MainLedgerEntry.LockDocumentNumberEdit();

        rowSelected = data;

        // Se obtiene los centros de costos asociados
        if (rowSelected.CostCenters.length > 0) {
            $('#tblCostCenter').dataTable().fnClearTable();

            for (var i = 0; i < rowSelected.CostCenters.length; i++) {
                $('#tblCostCenter').UifDataTable('addRow', rowSelected.CostCenters[i]);
            }
        } else {
            $('#tblCostCenter').dataTable().fnClearTable();
        }


        // Se obtiene los análisis asociados
        if (rowSelected.Analysis.length > 0) {
            $('#tblAnalysis').dataTable().fnClearTable();
            for (var j = 0; j < rowSelected.Analysis.length; j++) {
                $('#tblAnalysis').UifDataTable('addRow', rowSelected.Analysis[j]);

            }
        } else {
            $('#tblAnalysis').dataTable().fnClearTable();
        }

        // Se obtiene los postfechados asociados
        if (rowSelected.Postdated.length > 0) {
            $('#tblPostdated').dataTable().fnClearTable();
            for (var k = 0; k < rowSelected.Postdated.length; k++) {
                $('#tblPostdated').UifDataTable('addRow', rowSelected.Postdated[k]);
            }
        } else {
            $('#tblPostdated').dataTable().fnClearTable();
        }
    }

    /**
        * Permite agregar un asiento a la tabla de  movimientos contables.
        *
        */
    SaveAddLedgerEntry() {
        if ($("#entryAddForm").find("#AccountingAccountId").val() > 0) {
            if (thirdAccountingUsed == 0 || $("#entryAddForm").find("#IndividualId").val() > 0 || $("#entryAddForm").find("#IndividualId").val() != "") {
                $("#entryAddForm").validate();
                if ($("#entryAddForm").valid()) {

                    var exchangeRate = 0;
                    var rowModel = new RowModel();

                    rowModel.BranchId = $("#mainForm").find("#ddlBranches").val();
                    rowModel.SalePointId = $("#mainForm").find("#ddlSalePoints").val();
                    rowModel.CompanyId = $("#mainForm").find("#ddlAccountingCompanies").val();
                    rowModel.DestinationId = $("#mainForm").find("#ddlDestinations").val();
                    rowModel.AccountingMovementTypeId = $("#mainForm").find("#ddlAccountingMovementTypes").val();
                    rowModel.Date = $("#mainForm").find("#Date").val();
                    rowModel.AccountingAccountId = $("#entryAddForm").find("#AccountingAccountId").val();
                    rowModel.AccountingAccountNumber = $("#entryAddForm").find("#accountingAccountNumber").val();
                    rowModel.AccountingAccountDescription = $("#entryAddForm").find("#accountingAccountDescription").val();
                    rowModel.AccountingNatureId = $("#entryAddForm").find("#AccountingNatureId").val();
                    rowModel.AccountingNatureDescription = $("#entryAddForm").find("#AccountingNatureId option:selected").text();
                    rowModel.CurrencyId = $("#entryAddForm").find("#ddlCurrencies").val();
                    rowModel.CurrencyDescription = $("#entryAddForm").find("#ddlCurrencies option:selected").text();
                    exchangeRate = $("#entryAddForm").find("#exchange").val();
                    rowModel.ExchangeRate = ReplaceDecimalPoint(exchangeRate); //exchangeRate.replace(",", ".");
                    rowModel.ExchangeRateFormated = exchangeRate; //.replace(",", ".");
                    rowModel.Amount = ReplaceDecimalPoint($("#entryAddForm").find("#Amount").val()); //$("#entryAddForm").find("#Amount").val().replace(",", ".");
                    rowModel.AmountFormated = FormatCurrency(FormatDecimal($("#entryAddForm").find("#Amount").val()));
                    rowModel.IndividualId = $("#entryAddForm").find("#IndividualId").val();
                    rowModel.DocumentNumber = $("#entryAddForm").find("#entryPayerDocumentNumber").val();
                    rowModel.Name = $("#entryAddForm").find("#entryPayerName").val();
                    rowModel.ConceptId = $("#entryAddForm").find("#conceptId").val();
                    rowModel.ConceptDescription = $("#entryAddForm").find("#conceptDescription").val();
                    rowModel.BankReconciliationId = $("#entryAddForm").find("#BankReconciliationId").val();
                    rowModel.BankReconciliationDescription = $("#entryAddForm").find("#BankReconciliationId option:selected").text();
                    rowModel.ReceiptNumber = $("#entryAddForm").find("#ReceiptNumber").val();
                    rowModel.ReceiptDate = $("#entryAddForm").find("#receiptDate").val();
                    rowModel.Description = $("#entryAddForm").find("#Description").val();

                    $('#entryTable').UifDataTable('addRow', rowModel);
                    $("#entryAddForm").formReset();
                    $('#entryModalAdd').UifModal('hide');
                    $('#tblCostCenter').dataTable().fnClearTable();
                    $('#tblAnalysis').dataTable().fnClearTable();
                    $('#tblPostdated').dataTable().fnClearTable();
                    rowSelected = null;
                }
            } else {
                $("#entryModalAdd").find("#entryModalAlert").UifAlert('show', Resources.NoRegisteredPerson, "info");
            }
        } else {
            $("#entryModalAdd").find("#entryModalAlert").UifAlert('show', Resources.NoEnteredAccountingAccount, "info");
        }
    }

    /**
        * Permite agregar un asiento a la tabla de  movimientos contables.
        *
        */
    SaveEditLedgerEntry() {
        if ($("#entryEditForm").find("#AccountingAccountId").val() > 0) {
            if (thirdAccountingUsed == 0 || $("#entryEditForm").find("#IndividualId").val() > 0 || $("#entryEditForm").find("#IndividualId").val() != "") {
                $("#entryEditForm").validate();

                if ($("#entryEditForm").valid()) {

                    var exchangeRate = 0;
                    var rowModel = new RowModel();

                    rowModel.BranchId = $("#mainForm").find("#ddlBranches").val();
                    rowModel.SalePointId = $("#mainForm").find("#ddlSalePoints").val();
                    rowModel.CompanyId = $("#mainForm").find("#ddlAccountingCompanies").val();
                    rowModel.DestinationId = $("#mainForm").find("#ddlDestinations").val();
                    rowModel.AccountingMovementTypeId = $("#mainForm").find("#ddlAccountingMovementTypes").val();
                    rowModel.Date = $("#mainForm").find("#Date").val();
                    rowModel.AccountingAccountId = $("#entryEditForm").find("#AccountingAccountId").val();
                    rowModel.AccountingAccountNumber = $("#entryEditForm").find("#accountingAccountNumber").val();
                    rowModel.AccountingAccountDescription = $("#entryEditForm").find("#accountingAccountDescription").val();
                    rowModel.AccountingNatureId = $("#entryEditForm").find("#AccountingNatureId").val();
                    rowModel.AccountingNatureDescription = $("#entryEditForm").find("#AccountingNatureId option:selected").text();
                    rowModel.CurrencyId = $("#entryEditForm").find("#ddlCurrencies").val();
                    rowModel.CurrencyDescription = $("#entryEditForm").find("#ddlCurrencies option:selected").text();
                    exchangeRate = $("#entryEditForm").find("#exchange").val();
                    rowModel.ExchangeRate = ReplaceDecimalPoint(exchangeRate); //exchangeRate.replace(",", ".");
                    rowModel.ExchangeRateFormated = exchangeRate; //.replace(",", ".");
                    rowModel.Amount = ReplaceDecimalPoint($("#entryEditForm").find("#Amount").val()); //$("#entryEditForm").find("#Amount").val().replace(",", ".");
                    rowModel.AmountFormated = FormatCurrency(FormatDecimal($("#entryEditForm").find("#Amount").val()));
                    rowModel.IndividualId = $("#entryEditForm").find("#IndividualId").val();
                    rowModel.DocumentNumber = $("#entryEditForm").find("#entryPayerDocumentNumber").val();
                    rowModel.Name = $("#entryEditForm").find("#entryPayerName").val();
                    rowModel.ConceptId = $("#entryEditForm").find("#conceptId").val();
                    rowModel.ConceptDescription = $("#entryEditForm").find("#conceptDescription").val();
                    rowModel.BankReconciliationId = $("#entryEditForm").find("#BankReconciliationId").val();
                    rowModel.BankReconciliationDescription = $("#entryEditForm").find("#BankReconciliationId option:selected").text();
                    rowModel.ReceiptNumber = $("#entryEditForm").find("#ReceiptNumber").val();
                    rowModel.ReceiptDate = $("#entryEditForm").find("#receiptDate").val();
                    rowModel.Description = $("#entryEditForm").find("#Description").val();
                    rowModel.CostCenters = $('#tblCostCenter').UifDataTable('getData');
                    rowModel.Analysis = $('#tblAnalysis').UifDataTable('getData');

                    $('#entryTable').UifDataTable('editRow', rowModel, currentEditIndex);
                    $("#entryEditForm").formReset();
                    $('#entryModalEdit').UifModal('hide');
                }
            } else {
                $("#entryModalEdit").find("#entryModalAlert").UifAlert('show', Resources.NoRegisteredPerson, "info");
            }
        } else {
            $("#entryModalEdit").find("#entryModalAlert").UifAlert('show', Resources.NoEnteredAccountingAccount, "info");
        }
    }

    /**
        * Permite cerrar el modal de grabación de asiento.
        *
        */
    CancelAddLedgerEntry() {
        MainLedgerEntry.ClearModalFields();
        $("#entryAddForm").formReset();
        if ($('#entryTable').UifDataTable('getData').length == 0) {
            MainLedgerEntry.UnlockFields();
        }
    }

    /**
        * Permite cerrar el modal de edición de asiento.
        *
        */
    CancelEditLedgerEntry() {
        MainLedgerEntry.ClearModalFields();
        $("#entryEditForm").formReset();
        if ($('#entryTable').UifDataTable('getData').length == 0) {
            MainLedgerEntry.UnlockFields();
        }
    }

    /**
        * Permite visualizar el modal de ingreso de centro de costo.
        *
        */
    RowAddCostCenter() {

        if (rowSelected == undefined) {
            $("#recordAlert").UifAlert('show', Resources.SelectedRecordBeforeContinuing, "warning");
        } else {
            $("#recordAlert").UifAlert('hide');
            $('#entryModalAddFrmCostCenter').appendTo("body").UifModal('showLocal', Resources.AddCostCenter);
            $("#entryAddFrmCostCenter").appendTo("body");
            $("#entryModalAddFrmCostCenter").appendTo("#entryAddFrmCostCenter");
        }
    }

    /**
        * Permite eliminar un centro de costo.
        *
        * @param {String} event    - Eliminar.
        * @param {Object} data     - Objeto con valores del centro de costo seleccionado.
        * @param {Number} position - Indice del centro de costo seleccionado.
        */
    RowDeleteCostCenter(event, data, position) {

        $("#entryDeleteModalWarning").UifModal('showLocal', Resources.Warning);

        tablePosition = position;
        tableTypeId = 1;
    }

    /**
        * Permite editar un centro de costo.
        *
        * @param {String} event    - Editar.
        * @param {Object} data     - Objeto con valores del centro de costo seleccionado.
        * @param {Number} position - Indice del centro de costo seleccionado.
        */
    RowEditCostCenter(event, data, position) {

        entryCostCenterEdit = $("#entryModalEditFrmCostCenter").detach();
        costCenterCurrentEditIndex = position;

        entryCostCenterEdit.appendTo("body").UifModal('showLocal', Resources.Edit); //anexa el modal al cuerpo de la aplicación, evita el bloqueo de la pantalla
        $("#entryEditFrmCostCenter").appendTo("body"); //anexa el formulario dentro del modal al cuerpo de la aplicación
        $("#entryModalEditFrmCostCenter").appendTo("#entryEditFrmCostCenter"); //anexa el modal al formulario para que no se pierdan los elementos de validación del formulario.

        $("#entryEditFrmCostCenter").find("#CostCenterId").val(data.CostCenterId);
        $("#entryEditFrmCostCenter").find("#Description").val(data.Description);
        $("#entryEditFrmCostCenter").find("#PercentageAmount").val(data.PercentageAmountFormated);
    }

    /**
        * Permite agregar en la tabla de centros de costo.
        *
        */
    SaveAddCostCenter() {

        $("#entryAddFrmCostCenter").validate();
        if ($("#entryAddFrmCostCenter").valid()) {

            var costCenterRowModel = new CostCenterRowModel();

            costCenterRowModel.CostCenterId = $("#entryAddFrmCostCenter").find("#CostCenterId").val();
            costCenterRowModel.Description = $("#entryAddFrmCostCenter").find("#Description").val();
            costCenterRowModel.PercentageAmount = $("#entryAddFrmCostCenter").find("#PercentageAmount").val().replace(",", ".");
            costCenterRowModel.PercentageAmountFormated = $("#entryAddFrmCostCenter").find("#PercentageAmount").val();

            if (!MainLedgerEntry.IsDuplicatedCostCenters(costCenterRowModel.CostCenterId)) {
                if ((((MainLedgerEntry.CostCenterPercentageTotal() + parseFloat(parseFloat(costCenterRowModel.PercentageAmount.replace(",", ".")).toFixed(2))) <= 100))) {

                    rowSelected.CostCenters.push(costCenterRowModel);

                    $('#tblCostCenter').UifDataTable('addRow', costCenterRowModel);
                    $("#entryAddFrmCostCenter").formReset();
                    $('#entryModalAddFrmCostCenter').UifModal('hide');
                    $("#entryModalAddFrmCostCenter").find("#costCenterAlert").UifAlert('hide');

                } else {
                    $("#entryModalAddFrmCostCenter").find("#costCenterAlert").UifAlert('show', Resources.ValidateCostCenterPercentage, "danger");
                }
            } else {
                $("#entryModalAddFrmCostCenter").find("#costCenterAlert").UifAlert('show', Resources.DuplicatedRecord, "danger");
            }
        }
    }

    /**
        * Permite editar en la tabla de centros de costo.
        *
        */
    SaveEditCostCenter() {
        $("#entryEditFrmCostCenter").validate();
        if ($("#entryEditFrmCostCenter").valid()) {

            var costCenterRowModel = new CostCenterRowModel();

            costCenterRowModel.CostCenterId = $("#entryEditFrmCostCenter").find("#CostCenterId").val();
            costCenterRowModel.Description = $("#entryEditFrmCostCenter").find("#Description").val();
            costCenterRowModel.PercentageAmount = $("#entryEditFrmCostCenter").find("#PercentageAmount").val().replace(",", ".");
            costCenterRowModel.PercentageAmountFormated = $("#entryEditFrmCostCenter").find("#PercentageAmount").val();
            $('#tblCostCenter').UifDataTable('editRow', costCenterRowModel, costCenterCurrentEditIndex);
            rowSelected.CostCenters[costCenterCurrentEditIndex] = costCenterRowModel; //actualizo el registro en la variable rowSelected

            if (MainLedgerEntry.CostCenterPercentageTotal() > 100) {
                $("#entryModalEditFrmCostCenter").find("#costCenterAlert").UifAlert('show', Resources.SumPercentagesCostCentersDifferent, "danger");
            } else {
                $("#entryEditFrmCostCenter").formReset();
                $('#entryModalEditFrmCostCenter').UifModal('hide');
                $("#entryModalEditFrmCostCenter").find("#costCenterAlert").UifAlert('hide');
            }
        }
    }

    /**
        * Permite cerrar el modal de agregación de centro de costo.
        *
        */
    CancelAddCostCenter() {
        $("#entryAddFrmCostCenter").find("#CostCenterId").val("");
        $("#entryAddFrmCostCenter").find("#Description").val("");
        $("#entryAddFrmCostCenter").find("#PercentageAmount").val("");
        $("#entryAddFrmCostCenter").formReset();
    }

    /**
        * Permite cerrar el modal de edición de un centro de costo.
        *
        */
    CancelEditCostCenter() {

        var costCenterRowModel = new CostCenterRowModel();

        costCenterRowModel.CostCenterId = $("#entryEditFrmCostCenter").find("#CostCenterId").val();
        costCenterRowModel.Description = $("#entryEditFrmCostCenter").find("#Description").val();
        costCenterRowModel.PercentageAmount = lastCostCenterPercentageValue.toString().replace(",", ".");
        costCenterRowModel.PercentageAmountFormated = lastCostCenterPercentageValue;
        $('#tblCostCenter').UifDataTable('editRow', costCenterRowModel, costCenterCurrentEditIndex);
        rowSelected.CostCenters[costCenterCurrentEditIndex] = costCenterRowModel; //actualizo el registro en la variable rowSelected

        $("#entryEditFrmCostCenter").find("#CostCenterId").val("");
        $("#entryEditFrmCostCenter").find("#Description").val("");
        $("#entryEditFrmCostCenter").find("#PercentageAmount").val("");
        $("#entryEditFrmCostCenter").formReset();
    }


    /**
        * Permite visualizar el modal de ingreso de análisis.
        *
        */
    RowAddAnalysis() {
        $("#entryModalAddFrmAnalysis").find("#KeyFields").html("");

        if (rowSelected == undefined) {
            $("#recordAlert").UifAlert('show', Resources.SelectedRecordBeforeContinuing, "warning");
        } else {
            $("#recordAlert").UifAlert('hide');
            $('#entryModalAddFrmAnalysis').appendTo("body").UifModal('showLocal', Resources.AnalysisAdd); //anexa el modal al cuerpo de la aplicación, evita el bloqueo de la pantalla
            $("#entryAddFrmAnalysis").appendTo("body"); //anexa el formulario dentro del modal al cuerpo de la aplicación
            $("#entryModalAddFrmAnalysis").appendTo("#entryAddFrmAnalysis"); //anexa el modal al formulario para que no se pierdan los elementos de validación del formulario.
        }
    }

    /**
        * Permite eliminar un análisis.
        *
        * @param {String} event    - Eliminar.
        * @param {Object} data     - Objeto con valores del análisis seleccionado.
        * @param {Number} position - Indice del análisis seleccionado.
        */
    RowDeleteAnalysis(event, data, position) {
        $("#entryDeleteModalWarning").UifModal('showLocal', Resources.Warning);

        tablePosition = position;
        tableTypeId = 2;
    }

    /**
        * Permite editar un análisis.
        *
        * @param {String} event    - Editar.
        * @param {Object} data     - Objeto con valores del análisis seleccionado.
        * @param {Number} position - Indice del análisis seleccionado.
        */
    RowEditAnalysis(event, data, position) {
        $("#entryModalEditFrmAnalysis").find("#KeyFields").html("");
        entryAnalysisEdit = $("#entryModalEditFrmAnalysis").detach();
        analysisCurrentEditIndex = position;

        entryAnalysisEdit.appendTo("body").UifModal('showLocal', Resources.Edit); //anexa el modal al cuerpo de la aplicación, evita el bloqueo de la pantalla
        $("#entryEditFrmAnalysis").appendTo("body"); //anexa el formulario dentro del modal al cuerpo de la aplicación
        $("#entryModalEditFrmAnalysis").appendTo("#entryEditFrmAnalysis"); //anexa el modal al formulario para que no se pierdan los elementos de validación del formulario.

        $("#entryEditFrmAnalysis").find("#AnalysisId").val(data.AnalysisId);
        $("#entryEditFrmAnalysis").find("#AnalysisConceptId").val(data.AnalysisConceptId);
        setTimeout(function () {
            $("#entryEditFrmAnalysis").find("#AnalyissConceptId").val(data.AnalysisConceptId);
        }, 1000);
        $("#entryEditFrmAnalysis").find("#Key").val(data.Key);
        $("#entryEditFrmAnalysis").find("#Description").val(data.Description);

        var controller = GL_ROOT + "Base/GetPaymentConceptsByAnalysisCode?analysisCodeId=" + data.AnalysisId;
        $("#entryModalEditFrmAnalysis").find("#AnalysisConceptId").UifSelect({ source: controller, selectedId: data.AnalysisConceptId });

        setTimeout(function () {
            MainLedgerEntry.CreateKeyByAnalysisConcept(data.AnalysisConceptId);
        }, 1000);
    }

    /**
        * Permite grabar un movimiento en la tabla de análisis.
        *
        */
    SaveAddAnalysis() {
        $("#entryAddFrmAnalysis").validate();
        if ($("#entryAddFrmAnalysis").valid()) {
            var keyValue = "";
            for (var i = 0; i < keyLength; i++) {
                keyValue += $("#entryAddFrmAnalysis").find("#Key" + i).val() + "|";
            }

            if (keyValue.indexOf("undefined") == 0 || keyValue == "" || keyValue.indexOf("|") == 0) {
                $("#entryAddFrmAnalysis").find("#analysisAlert").UifAlert('show', Resources.EnterAnalysisConceptKey, "warning");
                return;
            }

            keyValue = keyValue.substring(0, keyValue.length - 1);
            $("#entryAddFrmAnalysis").find("#Key").val(keyValue);

            var analysisRowModel = new AnalysisRowModel();

            analysisRowModel.AnalysisId = $("#entryAddFrmAnalysis").find("#AnalysisId").val();
            analysisRowModel.AnalysisDescription = $("#entryAddFrmAnalysis").find("#AnalysisId option:selected").text();
            analysisRowModel.AnalysisConceptId = $("#entryAddFrmAnalysis").find("#AnalysisConceptId").val();
            analysisRowModel.AnalysisConceptDescription = $("#entryAddFrmAnalysis").find("#AnalysisConceptId option:selected").text();
            analysisRowModel.Key = $("#entryAddFrmAnalysis").find("#Key").val();
            analysisRowModel.Description = $("#entryAddFrmAnalysis").find("#Description").val();

            if (!MainLedgerEntry.IsDuplicatedAnalysis(analysisRowModel.AnalysisId, analysisRowModel.AnalysisConceptId, analysisRowModel.Key)) {
                rowSelected.Analysis.push(analysisRowModel);

                $('#tblAnalysis').UifDataTable('addRow', analysisRowModel);
                $("#entryAddFrmAnalysis").formReset();
                $('#entryModalAddFrmAnalysis').UifModal('hide');
                $("#entryModalAddFrmAnalysis").find("#analysisAlert").UifAlert('hide');
            } else {
                $("#entryModalAddFrmAnalysis").find("#analysisAlert").UifAlert('show', Resources.DuplicatedRecord, "warning");
            }
        }
    }

    /**
        * Permite grabar un movimiento en la tabla de análisis.
        *
        */
    SaveEditAnalysis() {
        $("#entryEditFrmAnalysis").validate();
        if ($("#entryEditFrmAnalysis").valid()) {
            var keyValue = "";
            for (var i = 0; i < keyLength; i++) {
                keyValue += $("#entryEditFrmAnalysis").find("#Key" + i).val() + "|";
            }

            if (keyValue.indexOf("undefined") == 0 || keyValue == "" || keyValue.indexOf("|") == 0) {
                $("#entryEditFrmAnalysis").find("#analysisAlert").UifAlert('show', Resources.EnterAnalysisConceptKey, "warning");
                return;
            }

            keyValue = keyValue.substring(0, keyValue.length - 1);
            $("#entryEditFrmAnalysis").find("#Key").val(keyValue);

            var analysisRowModel = new AnalysisRowModel();

            analysisRowModel.AnalysisId = $("#entryEditFrmAnalysis").find("#AnalysisId").val();
            analysisRowModel.AnalysisDescription = $("#entryEditFrmAnalysis").find("#AnalysisId option:selected").text();
            analysisRowModel.AnalysisConceptId = $("#entryEditFrmAnalysis").find("#AnalysisConceptId").val();
            analysisRowModel.AnalysisConceptDescription = $("#entryEditFrmAnalysis").find("#AnalysisConceptId option:selected").text();
            analysisRowModel.Key = $("#entryEditFrmAnalysis").find("#Key").val();
            analysisRowModel.Description = $("#entryEditFrmAnalysis").find("#Description").val();

            if (!MainLedgerEntry.IsDuplicatedAnalysis(analysisRowModel.AnalysisId, analysisRowModel.AnalysisConceptId, analysisRowModel.Key)) {

                $('#tblAnalysis').UifDataTable('editRow', analysisRowModel, analysisCurrentEditIndex);
                rowSelected.Analysis[analysisCurrentEditIndex] = analysisRowModel; //actualizo el registro en la variable rowSelected
                $("#entryEditFrmAnalysis").formReset();
                $('#entryModalEditFrmAnalysis').UifModal('hide');
                $("#entryModalEditFrmAnalysis").find("#analysisAlert").UifAlert('hide');
            } else {
                $("#entryModalEditFrmAnalysis").find("#analysisAlert").UifAlert('show', Resources.DuplicatedRecord, "warning");
            }
        }
    }

    /**
        * Permite cerrar el modal de agregación de análisis.
        *
        */
    CancelAddAnalysis() {
        $("#entryAddFrmAnalysis").formReset();
    }

    /**
        * Permite cerrar el modal de edición de análisis.
        *
        */
    CancelEditAnalysis() {
        $("#entryEditFrmAnalysis").formReset();
    }

    /**
        * Permite visualizar el modal de inserción de postfechados.
        *
        */
    RowAddPostDated() {
        if (rowSelected == undefined) {
            $("#recordAlert").UifAlert('show', Resources.SelectedRecordBeforeContinuing, "warning");
        } else {
            $("#recordAlert").UifAlert('hide');
            $('#entryModalAddFrmPostdated').appendTo("body").UifModal('showLocal', Resources.PostDatedAdd); //anexa el modal al cuerpo de la aplicación, evita el bloqueo de la pantalla
            $("#entryAddFrmPostdated").appendTo("body"); //anexa el formulario dentro del modal al cuerpo de la aplicación
            $("#entryModalAddFrmPostdated").appendTo("#entryAddFrmPostdated"); //anexa el modal al formulario para que no se pierdan los elementos de validación del formulario.
        }
    }

    /**
        * Permite eliminar un postfechado.
        *
        * @param {String} event    - Eliminar.
        * @param {Object} data     - Objeto con valores del postfechado seleccionado.
        * @param {Number} position - Indice del postfechado seleccionado.
        */
    RowDetelePostDated(event, data, position) {

        $("#entryDeleteModalWarning").UifModal('showLocal', Resources.Warning);

        tablePosition = position;
        tableTypeId = 3;
    }

    /**
        * Permite editar un postfechado.
        *
        * @param {String} event    - Editar.
        * @param {Object} data     - Objeto con valores del postfechado seleccionado.
        * @param {Number} position - Indice del postfechado seleccionado.
        */
    RowEditPostDated(event, data, position) {

        entryPostDatedEdit = $("#entryModalEditFrmPostdated").detach();
        postdatedCurrentEditIndex = position;

        entryPostDatedEdit.appendTo("body").UifModal('showLocal', Resources.Edit); //anexa el modal al cuerpo de la aplicación, evita el bloqueo de la pantalla
        $("#entryEditFrmPostdated").appendTo("body"); //anexa el formulario dentro del modal al cuerpo de la aplicación
        $("#entryModalEditFrmPostdated").appendTo("#entryEditFrmPostdated"); //anexa el modal al formulario para que no se pierdan los elementos de validación del formulario.

        $("#entryEditFrmPostdated").find("#PostDateTypeId").val(data.PostDateTypeId);
        $("#entryEditFrmPostdated").find("#CurrencyId").val(data.CurrencyId);
        postdatedExchangeRate = data.ExchangeRateFormated;
        $("#entryEditFrmPostdated").find("#DocumentNumber").val(data.DocumentNumber);
        $("#entryEditFrmPostdated").find("#IssueAmount").val(data.IssueAmountFormated);
        $("#entryEditFrmPostdated").find("#LocalAmount").val(data.LocalAmountFormated);
    }

    /**
        * Permite grabar un movimiento en la tabla de postfechados.
        *
        */
    SaveAddPostDated() {
        $("#entryAddFrmPostdated").validate();
        if ($("#entryAddFrmPostdated").valid()) {
            var postDatedRowModel = new PostDatedRowModel();

            postDatedRowModel.PostDateTypeId = $("#entryAddFrmPostdated").find("#PostDateTypeId").val();
            postDatedRowModel.PostDateTypeDescription = $("#entryAddFrmPostdated").find("#PostDateTypeId option:selected").text();
            postDatedRowModel.DocumentNumber = $("#entryAddFrmPostdated").find("#DocumentNumber").val();
            postDatedRowModel.CurrencyId = $("#entryAddFrmPostdated").find("#CurrencyId").val();
            postDatedRowModel.CurrencyDescription = $("#entryAddFrmPostdated").find("#CurrencyId option:selected").text();
            postDatedRowModel.ExchangeRate = postdatedExchangeRate.toString().replace(",", ".");
            postDatedRowModel.ExchangeRateFormated = postdatedExchangeRate;
            postDatedRowModel.IssueAmount = $("#entryAddFrmPostdated").find("#IssueAmount").val().replace(",", ".");
            postDatedRowModel.IssueAmountFormated = $("#entryAddFrmPostdated").find("#IssueAmount").val();
            postDatedRowModel.LocalAmount = $("#entryAddFrmPostdated").find("#LocalAmount").val().replace(",", ".");
            postDatedRowModel.LocalAmountFormated = $("#entryAddFrmPostdated").find("#LocalAmount").val();

            if (!MainLedgerEntry.IsDuplicatedPostDated(postDatedRowModel.PostDateTypeId, postDatedRowModel.DocumentNumber)) {
                rowSelected.Postdated.push(postDatedRowModel);

                $('#tblPostdated').UifDataTable('addRow', postDatedRowModel);
                $("#entryAddFrmPostdated").formReset();
                $('#entryModalAddFrmPostdated').UifModal('hide');
                $('#entryModalAddFrmPostdated').find("#postdatedAlert").UifAlert('hide');
            } else {
                $('#entryModalAddFrmPostdated').find("#postdatedAlert").UifAlert('show', Resources.DuplicatedRecord, "warning");
            }
        }
    }

    /**
        * Permite grabar un movimiento en la tabla de postfechados.
        *
        */
    SaveEditPostDated() {
        $("#entryEditFrmPostdated").validate();
        if ($("#entryEditFrmPostdated").valid()) {
            var postDatedRowModel = new PostDatedRowModel();

            postDatedRowModel.PostDateTypeId = $("#entryEditFrmPostdated").find("#PostDateTypeId").val();
            postDatedRowModel.PostDateTypeDescription = $("#entryEditFrmPostdated").find("#PostDateTypeId option:selected").text();
            postDatedRowModel.DocumentNumber = $("#entryEditFrmPostdated").find("#DocumentNumber").val();
            postDatedRowModel.CurrencyId = $("#entryEditFrmPostdated").find("#CurrencyId").val();
            postDatedRowModel.CurrencyDescription = $("#entryEditFrmPostdated").find("#CurrencyId option:selected").text();
            postDatedRowModel.ExchangeRate = postdatedExchangeRate.toString().replace(",", ".");
            postDatedRowModel.ExchangeRateFormated = postdatedExchangeRate;
            postDatedRowModel.IssueAmount = $("#entryEditFrmPostdated").find("#IssueAmount").val().replace(",", ".");
            postDatedRowModel.IssueAmountFormated = $("#entryEditFrmPostdated").find("#IssueAmount").val();
            postDatedRowModel.LocalAmount = $("#entryEditFrmPostdated").find("#LocalAmount").val().replace(",", ".");
            postDatedRowModel.LocalAmountFormated = $("#entryEditFrmPostdated").find("#LocalAmount").val();

            $('#tblPostdated').UifDataTable('editRow', postDatedRowModel, postdatedCurrentEditIndex);
            rowSelected.Postdated[postdatedCurrentEditIndex] = postDatedRowModel; //actualizo el registro en la variable rowSelected
            $("#entryEditFrmPostdated").formReset();
            $('#entryModalEditFrmPostdated').UifModal('hide');
        }
    }

    /**
        * Permite cerrar el modal de ingreso de postfechados.
        *
        */
    CancelAddPostDated() {
        $("#entryAddFrmPostdated").formReset();
    }

    /**
        * Permite cerrar el modal de edición de postfechados.
        *
        */
    CancelEditPostDated() {
        $("#entryEditFrmPostdated").formReset();
    }

    /**
        * Permite grabar el asiento de mayor con cabecera, detalle, análisis, centros de costos y postfechados.
        *
        */
    SaveLedgerEntry() {
        $("#mainForm").validate();
        if ($("#mainForm").valid()) {
            if (MainLedgerEntry.ValidateCreditsAndDebits()) {
                if (MainLedgerEntry.ValidateAllCostCenters() == false) {
                    $("#entryMainAlert").UifAlert('show', Resources.Not100percentCostCenter, "warning");
                }
                else {
                    if (MainLedgerEntry.ValidateAllAnalysis() == false) {
                        $("#entryMainAlert").UifAlert('show', Resources.MsgConceptAnalysisAssociate, "warning");
                        return;
                    }
                  
                    lockScreen();

                    dataTable = $('#entryTable').UifDataTable('getData');
                    $.ajax({
                        type: "POST",
                        url: GL_ROOT + "LedgerEntry/SaveLedgerEntry",
                        data: JSON.stringify({
                            "ledgerEntryModel": MainLedgerEntry.SetDataLedgerEntry()
                        }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8"
                    }).done(function (data) {
                        if (data.success == false) {
                            $("#entryMainAlert").UifAlert('show', data.result, "danger");
                        } else {

                            $('#entryTable').dataTable().fnClearTable();
                            $('#tblCostCenter').dataTable().fnClearTable();
                            $('#tblAnalysis').dataTable().fnClearTable();
                            $('#tblPostdated').dataTable().fnClearTable();
                            MainLedgerEntry.ClearFieldsEntry();
                            MainLedgerEntry.UnlockFields();
                            $("#entryMainAlert").UifAlert('show', Resources.EntrySuccessfullySaved + " " + data.result, "success");
                            $("#printEntryConfirmationText").html("<p class= 'text-info'>" + Resources.EntrySuccessfullySaved + " " + data.result + "</p>" + "<br/>" + Resources.PrintReport + ' ?');
                            $('#printEntryConfirmationModal').UifModal('showLocal', " ");
                            printDataEntryNumber = data.result;
                            MainLedgerEntry.InitializeDataLedgerEntry();
                        }

                        unlockScreen();
                    });

                }

            } else {
                $("#entryMainAlert").UifAlert('show', Resources.UnbalancedEntry, "warning");
            }
        }
    }

    /**
        * Permite seleccionar si desea imprimir o no el asiento de mayor generado.
        *
        */
    ChoosePrint() {

        if (dataTable != null) {
            MainLedgerEntry.ShowReportEntry(dataTable, printDataEntryNumber);
            setTimeout(function () {
                copyModal.modal('hide');
                printDataEntryNumber = 0;
                dataTable = null;
            }, 2000);
        }
    }

    /**
        * Permite seleccionar si desea imprimir o no el asiento de mayor generado.
        *
        */
    ConfirmPrintLedgerEntry() {
        if (dataTable != null) {
            MainLedgerEntry.ShowReportEntry(dataTable, printDataEntryNumber);
            setTimeout(function () {
                $("#printEntryConfirmationModal").UifModal('hide');
                printDataEntryNumber = 0;
                dataTable = null;
            }, 2000);
        }
    }

    /**
        * Permite ingresar solo números en el importe en el modal de inserción.
        *
        * @param {String} event    - KeyPress.
        */
    KeyPressAddAmount(event) {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }

    /**
        * Permite ingresar solo números en el importe en el modal de edición.
        *
        * @param {String} event    - KeyPress.
        */
    KeyPressEditAmount(event) {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }

    /**
        * Permite ingresar solo números en el importe de porcentaje en el modal de inserción de centro de costo.
        *
        * @param {String} event    - KeyPress.
        */
    KeyPressAddPercentageAmount(event) {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }

    /**
        * Permite ingresar solo números en el importe de porcentaje en el modal de edición de centro de costo.
        *
        * @param {String} event    - KeyPress.
        */
    KeyPressEditPercentageAmount(event) {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }

    /**
        * Permite ingresar solo números en el campo de importe de postfechados en modal de inserción.
        *
        * @param {String} event    - KeyPress.
        */
    KeyPressAddIssueAmount(event) {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }

    /**
        * Permite ingresar solo números en el campo número docucmento en el modal de edición de asiento.
        *
        * @param {String} event    - KeyPress.
        */
    KeyPressEditDocumentNumber(event) {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }

    /**
        * Permite ingresar solo números en el campo de número de documento de postfechados en modal de inserción.
        *
        * @param {String} event    - KeyPress.
        */
    KeyPressAddDocumentNumber(event) {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }

    /**
        * Permite ingresar solo números en el en el campo de importe en modal de edición.
        *
        * @param {String} event    - KeyPress.
        */
    KeyPressEditIssueAmount(event) {
        if (event.keyCode != 8 && event.keyCode != 46 && event.keyCode != 37 && event.keyCode != 39 && event.keyCode != 9) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            } //Números
        }
    }

    /**
        * Permite buscar los datos del pagador en modal de inserción.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del pagador seleccionado.
        */
    AutocompleteAddPayerDocumentNumber(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.IndividualId > 0) {
                documentNumberEntry = selectedItem.DocumentNumber;
                $("#btnEntrySaveAdd").attr("disabled", false);
                $("#entryModalAdd").find("#entryPayerName").val(selectedItem.Name);
                $("#entryModalAdd").find("#IndividualId").val(selectedItem.IndividualId);
                $("#entryModalAdd").find("#entryPersonAlert").UifAlert('hide');
            } else {
                $('#entryModalAdd').find("#entryPersonAlert").UifAlert('show', Resources.PersonNotFound, "warning");
                $("#entryModalAdd").find("#IndividualId").val("");
                $("#entryModalAdd").find("#entryPayerName").val("");
            }
        }
    }

    /**
        * Permite validar la existencia del pagador en modal de inserción.
        *
        * @param {String} event        - Seleccionar.
        */
    BlurAddPayerDocumentNumber(event) {
        MainLedgerEntry.ValidatePayerDocumentNumberEntryAdd();
    }

    /**
        * Permite buscar los datos del pagador en modal de edición.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del pagador seleccionado.
        */
    AutocompleteEditPayerDocumentNumber(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.IndividualId > 0) {
                documentNumberEntry = selectedItem.DocumentNumber;
                $("#btnEntrySaveEdit").attr("disabled", false);
                $("#entryModalEdit").find("#entryPayerName").val(selectedItem.Name);
                $("#entryModalEdit").find("#IndividualId").val(selectedItem.IndividualId);
                $("#entryModalEdit").find("#entryPersonAlert").UifAlert('hide');
            } else {
                $('#entryModalEdit').find("#entryPersonAlert").UifAlert('show', Resources.PersonNotFound, "warning");
                $("#entryModalEdit").find("#IndividualId").val("");
                $("#entryModalEdit").find("#entryPayerName").val("");
            }
        }
    }

    /**
        * Permite validar la existencia del pagador en modal de edición.
        *
        * @param {String} event        - Seleccionar.
        */
    BlurEditPayerDocumentNumber(event) {
        MainLedgerEntry.ValidatePayerDocumentNumberEntryEdit();
    }

    /**
        * Permite buscar los conceptos contables en modal de inserción.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del concepto contable seleccionado.
        */
    AutocompleteAddAccountingConcept(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.Id > 0) {

                var accountingAccountId = selectedItem.AccountingAccount.AccountingAccountId;
                if (accountingAccountId > 0) {
                    $('#entryModalAdd').find("#AccountingAccountId").val(accountingAccountId);
                    $('#entryModalAdd').find("#conceptDescription").val(selectedItem.Description);
                    $('#entryModalAdd').find("#accountingAccountNumber").val(selectedItem.AccountingAccount.Number);
                    $('#entryModalAdd').find("#accountingAccountDescription").val(selectedItem.AccountingAccount.Description);
                    $("#entryModalAdd").find("#entryModalAlert").UifAlert('hide');
                } else {
                    $("#entryModalAdd").find("#entryModalAlert").UifAlert('show', Resources.AccountingConceptNotAssociatedAccount, "info");
                    $('#entryModalAdd').find("#AccountingAccountId").val(accountingAccountId);
                    $('#entryModalAdd').find("#accountingAccountNumber").val('');
                    $('#entryModalAdd').find("#accountingAccountDescription").val('');
                    $('#entryModalAdd').find("#conceptDescription").val(selectedItem.DescriptionPaymentConcept);
                }
            } else {
                $("#entryModalAdd").find("#entryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
                $('#entryModalAdd').find("#AccountingAccountId").val('');
                $('#entryModalAdd').find("#conceptDescription").val('');
                $('#entryModalAdd').find("#accountingAccountNumber").val('');
                $('#entryModalAdd').find("#conceptDescription").val('');
                $('#entryModalAdd').find("#accountingAccountDescription").val('');
            }
        } else {
            $('#entryModalAdd').find("#conceptId").val('');
            $('#entryModalAdd').find("#conceptDescription").val('');
        }
    }

    /**
        * Permite buscar los conceptos contables en modal de edición.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del concepto contable seleccionado.
        */
    AutocompleteEditAccountingConcept(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.Id > 0) {
                var accountingAccountId = selectedItem.AccountingAccount.AccountingAccountId;
                if (accountingAccountId > 0) {
                    $('#entryModalEdit').find("#AccountingAccountId").val(accountingAccountId);
                    $('#entryModalEdit').find("#conceptDescription").val(selectedItem.Description);
                    $('#entryModalEdit').find("#accountingAccountNumber").val(selectedItem.AccountingAccount.Number);
                    $('#entryModalEdit').find("#accountingAccountDescription").val(selectedItem.AccountingAccount.Description);
                    $("#entryModalEdit").find("#entryModalAlert").UifAlert('hide');
                } else {
                    $("#entryModalEdit").find("#entryModalAlert").UifAlert('show', Resources.AccountingConceptNotAssociatedAccount, "info");
                    $('#entryModalEdit').find("#AccountingAccountId").val(accountingAccountId);
                    $('#entryModalEdit').find("#accountingAccountNumber").val('');
                    $('#entryModalEdit').find("#accountingAccountDescription").val('');
                    $('#entryModalEdit').find("#conceptDescription").val(selectedItem.DescriptionPaymentConcept);
                }
            } else {
                $("#entryModalEdit").find("#entryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
                $('#entryModalEdit').find("#AccountingAccountId").val('');
                $('#entryModalEdit').find("#conceptDescription").val('');
                $('#entryModalEdit').find("#accountingAccountNumber").val('');
                $('#entryModalEdit').find("#conceptDescription").val('');
                $('#entryModalEdit').find("#accountingAccountDescription").val('');
            }
        }
    }

    /**
        * Permite buscar los conceptos contables por descripción en modal de inserción.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del concepto contable seleccionado.
        */
    AutocompleteAddAccountingConceptDescription(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.Id > 0) {
                var accountingAccountId = selectedItem.AccountingAccount.AccountingAccountId;
                $("#entryModalAdd").find("#AccountingAccountId").val(accountingAccountId);
                $("#entryModalAdd").find("#conceptId").val(selectedItem.Id);
                $("#entryModalAdd").find("#accountingAccountNumber").val(selectedItem.AccountingAccount.Number);
                $("#entryModalAdd").find("#accountingAccountDescription").val(selectedItem.AccountingAccount.Description);
                $("#entryModalAdd").find("#entryModalAlert").UifAlert('hide');
            } else {
                $("#entryModalAdd").find('#conceptId').val('');
                $("#entryModalAdd").find('#conceptDescription').val('');
            }
        } else {
            $("#entryModalAdd").find("#entryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
            $("#conceptId").val('');
            $("#accountingAccountNumber").val('');
            $("#accountingAccountDescription").val('');
        }
    }

    /**
        * Permite buscar los conceptos contables por descripción en modal de edición.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del pagador seleccionado.
        */
    AutocompleteEditAccountingConceptDescription(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.Id > 0) {
                var accountingAccountId = selectedItem.AccountingAccount.AccountingAccountId;
                $('#entryModalEdit').find("#AccountingAccountId").val(accountingAccountId);
                $('#entryModalEdit').find("#conceptId").val(selectedItem.Id);
                $('#entryModalEdit').find("#accountingAccountNumber").val(selectedItem.AccountingAccount.Number);
                $('#entryModalEdit').find("#accountingAccountDescription").val(selectedItem.AccountingAccount.Description);
                $('#entryModalEdit').find("#entryModalAlert").UifAlert('hide');
            } else {
                $('#entryModalEdit').find("#conceptId").val('');
                $('#entryModalEdit').find("#conceptDescription").val('');
            }
        } else {
            $("#entryModalEdit").find("#entryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
            $("#entryModalEdit").find("#conceptId").val('');
            $("#entryModalEdit").find("#accountingAccountNumber").val('');
            $("#entryModalEdit").find("#accountingAccountDescription").val('');
        }
    }

    /**
        * Permite buscar las cuentas contables en modal de inserción.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la cuenta contable seleccionada.
        */
    AutocompleteAddAccountingAccount(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.AccountingAccountId > 0) {
                var accountingAccountId = selectedItem.AccountingAccountId;
                $("#AccountingAccountId").val(accountingAccountId);
                $("#conceptId").val(selectedItem.PaymentConceptCode);
                $("#conceptDescription").val(selectedItem.DescriptionPaymentConcept);
                $("#accountingAccountDescription").val(selectedItem.AccountName);
                $("#entryModalAdd").find("#entryModalAlert").UifAlert('hide');
            } else {
                $("#entryModalAdd").find("#entryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
                $("#conceptId").val('');
                $("#conceptDescription").val('');
                $("#accountingAccountDescription").val('');
            }
        }
    }

    /**
        * Permite buscar las cuentas contables en modal de edición.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la cuenta contable seleccionada.
        */
    AutocompleteEditAccountingAccount(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.AccountingAccountId > 0) {
                var accountingAccountId = selectedItem.AccountingAccountId;
                $("#AccountingAccountId").val(accountingAccountId);
                $("#conceptId").val(selectedItem.PaymentConceptCode);
                $("#conceptDescription").val(selectedItem.DescriptionPaymentConcept);
                $("#accountingAccountDescription").val(selectedItem.AccountName);
                $("#entryModalEdit").find("#entryModalAlert").UifAlert('hide');
            } else {
                $("#entryModalEdit").find("#entryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
                $("#conceptId").val('');
                $("#conceptDescription").val('');
                $("#accountingAccountDescription").val('');
            }
        }
    }

    /**
        * Permite obtener los puntos de venta al seleccionar una sucursal.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la sucursal seleccionada.
        */
    ItemSelectedBranch(event, selectedItem) {
        if (selectedItem.Id > 0) {
            var controller = GL_ROOT + "Base/GetSalePointsbyBranch?branchId=" + selectedItem.Id;
            $("#ddlSalePoints").UifSelect({
                source: controller
            });
        } else{
            $("#ddlSalePoints").UifSelect();
        }
    }

    /**
        * Permite obtener la tasa de cambio al seleccionar la moneda.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la moneda seleccionada.
        */
    ItemSelectedCurrency(event, selectedItem) {
        if (selectedItem.Id == 0) {
            $("#exchange").val('1');
            $("#exchangeAlert").UifAlert('hide');
            return;
        }
        if (selectedItem.Id > -1) {
            $.ajax({
                url: GL_ROOT + "Base/GetExchangeByCurrency",
                data: {
                    "currencyId": selectedItem.Id
                },
                success: function (data) {
                    if (data.length > 0) {
                        $("#exchangeAlert").UifAlert('hide');
                        $("#entryModalAdd").find("#exchange").val(parseFloat(data));
                        $("#entryModalEdit").find("#exchange").val(parseFloat(data));
                    } else {
                        $("#exchangeAlert").UifAlert('show', Resources.ExchangeRateNotFound, "info");
                        $("#exchange").val('');
                    }
                },
                error: function (result) {
                    $("#exchange").val('');
                    $("#exchangeAlert").UifAlert('show', Resources.ExchangeRateNotFound, "info");
                }
            });
        } else {
            $("#exchange").val();
        }
    }

    /**
        * Permite obtener la tasa de cambio al seleccionar la moneda en modal postfechados en inserción.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la moneda seleccionada.
        */
    ItemSelectedAddCurrency(event, selectedItem) {
        if (selectedItem.Id == 0) {
            postdatedExchangeRate = 1;

            if ($("#entryAddFrmPostdated").find("#IssueAmount").val() != "" || $("#entryAddFrmPostdated").find("#IssueAmount").val() != undefined) {
                var total = 0;
                total = ($("#entryAddFrmPostdated").find("#IssueAmount").val() * postdatedExchangeRate).toFixed(2);
                $("#entryAddFrmPostdated").find("#LocalAmount").val(total);
            }
            $("#entryModalAddFrmPostdated").find("#postdatedAlert").UifAlert('hide');
            return;
        }
        if (selectedItem.Id > -1) {
            $.ajax({
                url: GL_ROOT + "Base/GetExchangeByCurrency",
                data: {
                    "currencyId": selectedItem.Id
                },
                success: function (data) {
                    if (data.length > 0) {
                        $("#entryModalAddFrmPostdated").find("#postdatedAlert").UifAlert('hide');
                        postdatedExchangeRate = parseFloat(data);
                        if ($("#entryAddFrmPostdated").find("#IssueAmount").val() != "" || $("#entryAddFrmPostdated").find("#IssueAmount").val() != undefined) {
                            var total = 0;
                            total = ($("#entryAddFrmPostdated").find("#IssueAmount").val() * postdatedExchangeRate).toFixed(2);
                            $("#entryAddFrmPostdated").find("#LocalAmount").val(total);
                        }
                    } else {
                        $("#entryModalAddFrmPostdated").find("#postdatedAlert").UifAlert('show', Resources.ExchangeRateNotFound, "info");
                        postdatedExchangeRate = 0;
                    }
                },
                error: function (result) {
                    postdatedExchangeRate = 0;
                    $("#entryModalAddFrmPostdated").find("#postdatedAlert").UifAlert('show', Resources.ExchangeRateNotFound, "info");
                }
            });
        } else {
            postdatedExchangeRate = 0;
        }
    }

    /**
        * Permite calcular el importe en moneda local al modificar el importe en modal de inserción.
        *
        * @param {String} event        - Perdel el foco.
        */
    BlurAddIssueAmount(event) {
        if ($("#entryAddFrmPostdated").find("#IssueAmount").val() != "" || $("#entryAddFrmPostdated").find("#IssueAmount").val() != undefined) {
            var total = 0;
            total = ($("#entryAddFrmPostdated").find("#IssueAmount").val() * postdatedExchangeRate).toFixed(2);
            $("#entryAddFrmPostdated").find("#LocalAmount").val(total);
        }
    }

    /**
        * Permite obtener la tasa de cambio al seleccionar la moneda en postfechados en edición.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la moneda seleccionada.
        */
    ItemSelectedEditCurrency(event, selectedItem) {
        if (selectedItem.Id == 0) {
            postdatedExchangeRate = 1;

            if ($("#entryEditFrmPostdated").find("#IssueAmount").val() != "" || $("#entryEditFrmPostdated").find("#IssueAmount").val() != undefined) {
                var total = 0;
                total = ($("#entryEditFrmPostdated").find("#IssueAmount").val() * postdatedExchangeRate).toFixed(2);
                $("#entryEditFrmPostdated").find("#LocalAmount").val(total);
            }

            $("#entryModalEditFrmPostdated").find("#postdatedAlert").UifAlert('hide');
            return;
        }
        if (selectedItem.Id > -1) {
            $.ajax({
                url: GL_ROOT + "Base/GetExchangeByCurrency",
                data: {
                    "currencyId": selectedItem.Id
                },
                success: function (data) {
                    if (data.length > 0) {
                        $("#entryModalEditFrmPostdated").find("#postdatedAlert").UifAlert('hide');
                        postdatedExchangeRate = parseFloat(data);

                        if ($("#entryEditFrmPostdated").find("#IssueAmount").val() != "" || $("#entryEditFrmPostdated").find("#IssueAmount").val() != undefined) {
                            var total = 0;
                            total = ($("#entryEditFrmPostdated").find("#IssueAmount").val() * postdatedExchangeRate).toFixed(2);
                            $("#entryEditFrmPostdated").find("#LocalAmount").val(total);
                        }
                    } else {
                        $("#entryModalEditFrmPostdated").find("#postdatedAlert").UifAlert('show', Resources.ExchangeRateNotFound, "info");
                        postdatedExchangeRate = 0;
                    }
                },
                error: function (result) {
                    postdatedExchangeRate = 0;
                    $("#entryModalEditFrmPostdated").find("#postdatedAlert").UifAlert('show', Resources.ExchangeRateNotFound, "info");
                }
            });
        } else {
            postdatedExchangeRate = 0;
        }
    }

    /**
        * Permite calcular el importe en moneda local cuando se modifica el importe en modal de edición.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la sucursal seleccionada.
        */
    BlurEditIssueAmount(event) {
        if ($("#entryEditFrmPostdated").find("#IssueAmount").val() != "" || $("#entryEditFrmPostdated").find("#IssueAmount").val() != undefined) {
            var total = 0;
            total = ($("#entryEditFrmPostdated").find("#IssueAmount").val() * postdatedExchangeRate).toFixed(2);
            $("#entryEditFrmPostdated").find("#LocalAmount").val(total);
        }
    }

    /**
        * Permite obtener las cuentas contables en modal de inserción.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la cuenta contable seleccionada.
        */
    AutocompleteAddAccountingAccountNumber(event, selectedItem) {
        var data = JSON.stringify(selectedItem);
        $('#entryModalAdd').find('#accountingAccountDescription').val(selectedItem.Description);
        $('#entryModalAdd').find("#AccountingAccountId").val(selectedItem.AccountingAccountId);
        $('#entryModalAdd').find("#conceptId").val("");
        $('#entryModalAdd').find("#conceptDescription").val("");
    }

    /**
        * Permite obtener las cuentas contables en modal de edición.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la cuenta contable seleccionada.
        */
    AutocompleteEditAccountingAccountNumber(event, selectedItem) {
        var data = JSON.stringify(selectedItem);
        $('#entryEditForm').find('#accountingAccountDescription').val(selectedItem.Description);
        $('#entryEditForm').find("#AccountingAccountId").val(selectedItem.AccountingAccountId);
        $('#entryEditForm').find("#conceptId").val("");
        $('#entryEditForm').find("#conceptDescription").val("");
    }

    /**
        * Permite obtener los centros de costos en modal de inserción.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del centro de costo seleccionado.
        */
    AutocompleteAddCostCenter(event, selectedItem) {
        var data = JSON.stringify(selectedItem);
        $('#entryAddFrmCostCenter').find('#Description').val(selectedItem.Description);
    }

    /**
        * Permite obtener los centros de costos en modal de edición.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del centro de costo seleccionado.
        */
    AutocompleteEditCostCenter(event, selectedItem) {
        var data = JSON.stringify(selectedItem);
        $('#entryEditFrmCostCenter').find('#Description').val(selectedItem.Description);
    }

    /**
        * Permite obtener los centros de costo por descripción en modal de inserción.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del centro de costo seleccionado.
        */
    AutocompleteAddCostCenterDescription(event, selectedItem) {
        var data = JSON.stringify(selectedItem);
        $('#entryAddFrmCostCenter').find('#CostCenterId').val(selectedItem.CostCenterId);
    }

    /**
        * Permite obtener los centros de costo por descripción en modal de edición.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del centro de costo seleccionado.
        */
    AutocompleteEditCostCenterDescription(event, selectedItem) {
        var data = JSON.stringify(selectedItem);
        $('#entryEditFrmCostCenter').find('#CostCenterId').val(selectedItem.CostCenterId);
    }

    /**
        * Permite obtener los conceptos contables por análisis en modal de inserción.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la cuenta contable seleccionada.
        */
    ItemSelectedAddAnalysis(event, selectedItem) {
        $("#entryModalAddFrmAnalysis").find("#KeyFields").html("");
        if (selectedItem.Id > 0) {
            var controller = GL_ROOT + "Base/GetPaymentConceptsByAnalysisCode?analysisCodeId=" + selectedItem.Id;
            $("#entryModalAddFrmAnalysis").find("#AnalysisConceptId").UifSelect({
                source: controller
            });
        } else
            $("#entryModalAddFrmAnalysis").find("#AnalysisConceptId").UifSelect();
    }

    /**
        * Permite obtener los conceptos contables por análisis en modal de edición.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores de la cuenta contable seleccionada.
        */
    ItemSelectedEditAnalysis(event, selectedItem) {
        $("#entryModalEditFrmAnalysis").find("#KeyFields").html("");
        if (selectedItem.Id > 0) {
            var controller = GL_ROOT + "Base/GetPaymentConceptsByAnalysisCode?analysisCodeId=" + selectedItem.Id;
            $('#entryModalEditFrmAnalysis').find("#AnalysisConceptId").UifSelect({
                source: controller
            });
        } else
            $('#entryModalEditFrmAnalysis').find("#AnalysisConceptId").UifSelect();
    }

    /**
        * Permite eliminar el registro seleccionado en modall de eliminación.
        *
        */
    DeleteWarningAccept() {
        if ($("#ViewBagOptionId").val() == "Entry") {
            $("#entryDeleteModalWarning").UifModal('hide');

            MainLedgerEntry.Delete(tableTypeId, tablePosition);
            $("#entryMainAlert").UifAlert('show', Resources.RecordSuccessfullyDeleted, "success");

            // Validación solo para centros de costos
            $("#btnAcceptMessageModal").click(function () {
                var costCenterData = $("#tblCostCenter").UifDataTable('getData');
                if (costCenterData.length > 0) {
                    if (tableTypeId == 1) {
                        if (CostCenterPercentageTotal() != 100) {
                            $("#entryMainAlert").UifAlert('show', Resources.SumPercentagesCostCentersDifferent, "info");
                        }
                    }
                }
            });
        }
    }

    /**
        * Permite aceptar el asiento tipo seleccionado.
        *
        */
    AcceptEntryTypewarning() {
        if (isEntryType == 1){
            if (entryTypeId != 0) {
                accounts = [];
                $("#EntryTypeModalWarning").UifModal("hide");
                $('#entryTable').dataTable().fnClearTable();
                MainLedgerEntry.AddEntryType(entryTypeId);
            }
        }else{
            entryTypeId = 0;
            accounts = [];
            $('#entryTable').dataTable().fnClearTable();
            $("#EntryTypeModalWarning").UifModal("hide");
            $('#btnAddMovement').trigger('click');
        }
    }


    /**
        * Obtiene las columnas que conforman la clave de análisis al seleccionar un concepto de análisis.
        *
        * @param {Object} event        - Seleccionar registro.
        * @param {Object} selectedItem - Información del registro seleccionado.
        */
    ItemSelectedAddAnalysisConcept(event, selectedItem) {
        $("#entryAddFrmAnalysis").find("#analysisAlert").UifAlert('hide');
        $("#entryAddFrmAnalysis").find("#KeyFields").html("");

        if (selectedItem.Id > 0) {
            $.ajax({
                type: "GET",
                url: GL_ROOT + 'Base/GetKeysByAnalysisAndConceptId',
                data: {
                    "analysisId": $("#entryAddFrmAnalysis").find("#AnalysisId").val(),
                    "analysisConceptId": selectedItem.Id
                },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.length > 0) {
                        keyLength = data.length;
                        MainLedgerEntry.CreateTexBoxDinamic(data, 0, "");
                    }
                    else {
                        $("#entryAddFrmAnalysis").find("#analysisAlert").UifAlert('show', Resources.MessageValidationKeysAnalysisConcept, "warning");
                    }
                }
            });
        } else {
            $("#entryAddFrmAnalysis").find("#KeyFields").html("");
        }
    }

    /**
        * Obtiene las columnas que conforman la clave de análisis al seleccionar un concepto de análisis.
        *
        * @param {Object} event        - Seleccionar registro.
        * @param {Object} selectedItem - Información del registro seleccionado.
        */
    ItemSelectedEditAnalysisConcept(event, selectedItem) {
        $("#entryEditFrmAnalysis").find("#analysisAlert").UifAlert('hide');
        $("#entryEditFrmAnalysis").find("#KeyFields").html("");

        if (selectedItem.Id > 0) {
            $.ajax({
                type: "GET",
                url: GL_ROOT + 'Base/GetKeysByAnalysisAndConceptId',
                data: {
                    "analysisId": $("#entryEditFrmAnalysis").find("#AnalysisId").val(),
                    "analysisConceptId": selectedItem.Id
                },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.length > 0) {
                        keyLength = data.length;
                        MainLedgerEntry.CreateTexBoxDinamic(data, 1, $("#entryEditFrmAnalysis").find("#Key").val());
                    }
                    else {
                        $("#entryEditFrmAnalysis").find("#analysisAlert").UifAlert('show', Resources.MessageValidationKeysAnalysisConcept, "warning");
                    }
                }
            });
        } else {
            $("#entryEditFrmAnalysis").find("#KeyFields").html("");
        }
    }

    /*--------------------------------------------------------------------------------------------------------------------------*/
    /*                                                 DEFINICIÓN DE FUNCIONES                                                  */
    /*--------------------------------------------------------------------------------------------------------------------------*/

    /**
        * Valida todos los centros de costo.
        *
        */
    static ValidateAllCostCenters() {
        var result = false;
        var data = $("#entryTable").UifDataTable('getData');
        var totalCostCenters = 0;

        for (var i = 0; i < data.length; i++) {
            if (data[i].CostCenters.length > 0) {
                for (var j = 0; j < data[i].CostCenters.length; j++) {
                    totalCostCenters = totalCostCenters + parseFloat(data[i].CostCenters[j].PercentageAmountFormated);
                }
                if (totalCostCenters < 100 || totalCostCenters > 100) {
                    totalCostCenters = 0;
                    result = false;
                    break;
                } else {
                    result = true;
                }
            } else {
                result = true;
            }
            totalCostCenters = 0;
        }
        return result;
    }



    /**
        * Valida porcentaje de centros de costos.
        *
        */
    static CostCenterPercentageTotal() {
        var totalPercentage = 0;
        var costCenterData = $("#tblCostCenter").UifDataTable('getData');

        if (costCenterData.length > 0) {
            for (var i = 0; i < costCenterData.length; i++) {
                totalPercentage = totalPercentage + parseFloat(costCenterData[i].PercentageAmount.replace(",", "."));
            }
        }
        return totalPercentage;
    }

    /**
        * Valida centro de costos previamente ingresados.
        *
        */
    static IsDuplicatedCostCenters(costCenterId) {
        var result = false;
        var count = 0;
        var costCenterData = $("#tblCostCenter").UifDataTable('getData');

        if (costCenterData.length > 0) {
            for (var i = 0; i < costCenterData.length; i++) {
                if (costCenterData[i].CostCenterId == costCenterId)
                    count = count + 1;
            }
        }

        if (count > 0)
            result = true;
        return result;
    }





    /**
    * Valida todos los Analisis tengan concepto 
    *DanC 2018-06-14
    */
    static ValidateAllAnalysis() {
        var result = false;
        var data = $("#entryTable").UifDataTable('getData');
        //var analysisConcept_Id = 0;
        for (var i = 0; i < data.length; i++) {
            if (data[i].Analysis.length > 0) {
                for (var j = 0; j < data[i].Analysis.length; j++) {

                    if (data[i].Analysis[j].AnalysisConceptId == 0) {
                        result = false;
                        return result;
                    }

                }
                result = true;

            } else {
              result = true;
            }
        }
        return result;
    }
    


    /**
        * Valida analisis previamente ingresados.
        * 
        */
    static IsDuplicatedAnalysis(analysisCodeId, analysisConceptId, key) {
        var result = false;
        var count = 0;
        var analysisData = $("#tblAnalysis").UifDataTable('getData');

        if (analysisData.length > 0) {
            for (var i = 0; i < analysisData.length; i++) {
                if (analysisData[i].AnalysisId == analysisCodeId && analysisData[i].AnalysisConceptId == analysisConceptId && analysisData[i].Key == key)
                    count = count + 1;
            }
        }

        if (count > 0)
            result = true;

        return result;
    }

    /**
        * Valida postfechados previamente ingresados.
        *
        */
    static IsDuplicatedPostDated(postDatedTypeId, documentNumber) {
        var result = false;
        var count = 0;
        var postdatedData = $("#tblPostdated").UifDataTable('getData');

        if (postdatedData.length > 0) {
            for (var i = 0; i < postdatedData.length; i++) {
                if (postdatedData[i].PostDateTypeId == postDatedTypeId && postdatedData[i].DocumentNumber == documentNumber)
                    count = count + 1;
            }
        }

        if (count > 0)
            result = true;
        return result;
    }

    /**
        * Limpia los campos del formulario principal.
        *
        */
    static ClearFieldsEntry() {
        //$("#ddlEntryTypes").removeAttr("disabled");
        //$("#ddlBranches").val("");
        //$("#ddlSalePoints").val("");
        $("#ddlAccountingCompanies").val(Resources.DefaultCompanyValue);
        $("#ddlDestinations").val("");
        $("#ddlAccountingMovementTypes").val("");
        $("#DailyEntryDescription").val("");
        MainLedgerEntry.GetAccountingDate(Resources.AccountingModuleDateId);
        $("#ddlCurrencies").val("");
        $("#exchange").val("");
        isEntryType = 0;
        entryTypeId = 0;
        accounts = [];
    }

    /**
        * Limpia los campos del modal.
        *
        */
    static ClearModalFields() {
        $("#entryAddForm").find("#AccountingAccountId").val("");
        $("#entryAddForm").find("#accountingAccountNumber").val("");
        $("#entryAddForm").find("#accountingAccountDescription").val("");
        $("#entryAddForm").find("#AccountingNatureId").val("");
        $("#entryAddForm").find("#Amount").val("");
        $("#entryAddForm").find("#IndividualId").val("");
        $("#entryAddForm").find("#entryPayerDocumentNumber").val("");
        $("#entryAddForm").find("#entryPayerName").val("");
        $("#entryAddForm").find("#conceptId").val("");
        $("#entryAddForm").find("#conceptDescription").val("");
        $("#entryAddForm").find("#BankReconciliationId").val("");
        $("#entryAddForm").find("#ReceiptNumber").val("");
        $("#entryAddForm").find("#receiptDate").val("");
        $("#entryAddForm").find("#Description").val("");
    }

    /**
        * Permite eliminar los registros en tablas de centros de costos/analisis/postfechados.
        *
        * 1 - Centro de Costos
        * 2 - Analisis
        * 3 - Postfechados
        * 4 - Movimientos
        */
    static Delete(tableId, position) {
        // Borro centros de costos
        if (tableId == 1) {

            $('#tblCostCenter').UifDataTable('deleteRow', position);
            rowSelected.CostCenters.splice(position, 1);
        }

        // Borro análisis
        if (tableId == 2) {
            $('#tblAnalysis').UifDataTable('deleteRow', position);
            rowSelected.Analysis.splice(position, 1);
        }

        // Borro postfechados
        if (tableId == 3) {
            $('#tblPostdated').UifDataTable('deleteRow', position);
            rowSelected.Postdated.splice(position, 1);
        }

        // Borro movimientos
        if (tableId == 4) {
            $('#entryTable').UifDataTable('deleteRow', position);

            // Limpio tabla de centro de costos
            $('#tblCostCenter').dataTable().fnClearTable();

            // Limpio tabla de analásis
            $('#tblAnalysis').dataTable().fnClearTable();

            // Limpio tabla de postfechados
            $('#tblPostdated').dataTable().fnClearTable();

            // Deselecciono la fila
            rowSelected = null;

            if ($('#entryTable').UifDataTable('getData').length == 0) {
                MainLedgerEntry.UnlockFields();
            }
        }
    }

    /**
        * Valida y setea combo de Compañía.
        *
        */
    static ValidateCompanyEntry() {
        if ($("#ddlAccountingCompanies").val() != "" && $("#ddlAccountingCompanies").val() != null) {

            if (parseInt($("#ViewBagParameterMulticompany").val()) == 0) {

                $("#ddlAccountingCompanies").attr("disabled", true);
            } else {
                $("#ddlAccountingCompanies").attr("disabled", false);
            }
            clearInterval(timeEntry);
        }
    }

    /**
        * Llena el combo de asientos tipo.
        *
        */
    static LoadEntryTypes() {
        var loadEntryTypes = new Promise(function (resolve, reject) {
            $.ajax({
                type: 'Post',
                url: GL_ROOT + "Entry/GetEntryTypeModels",
                success: function (data) {
                    resolve(data);
                }
            });
        });
        loadEntryTypes.then(function (data) {
            if (data.success) {
                var items = $("#entryTypes").empty();
                for (var x in data.result) {
                    items.append('<li value="' + data.result[x].EntryTypeId + '"><a data-toggle="dropdown" href="#">' + data.result[x].EntryTypeDescription + '</a></li>');
                }
            }
        });
    }

    /**
        * Carga la fecha actual desde el servidor.
        *
        */
    static GetDate() {
        if ($("#ViewBagImputationType").val() == undefined &&
            $("#ViewBagParameterMulticompanyPayment").val() == undefined &&
            $("#ViewBagBillControlId").val() == undefined) {

            $.ajax({
                url: GL_ROOT + "Base/GetDate",
                success: function (data) {
                    $("#Date").val(data);
                }
            });
        }
    }

    /**
        * Obtiene la fecha contable.
        *
        */
    static GetAccountingDate(moduleDateId) {
        $.ajax({
            url: GL_ROOT + "Base/GetAccountingDate",
            data: {
                "moduleDateId": moduleDateId
            },
            success: function (data) {
                $("#Date").val(data);
            }
        });
    }

    /**
        * Visualiza el reporte en pantalla.
        *
        */
    static ShowReportEntry(dataTable, entryNumber) {

        window.open(GL_ROOT + "LedgerEntry/GetLedgerEntryByCriteria?branchId=" + dataTable[0].BranchId +
            '&date=' + dataTable[0].Date + '&entryNumber=' + entryNumber + '&destinationId=' +
            dataTable[0].DestinationId + '&accountingMovementTypeId=' + dataTable[0].AccountingMovementTypeId,
            'mywindow', 'fullscreen=yes, scrollbars=auto');
    }

    /**
        * Setea el punto de venta de default.
        *
        */
    static SetSalePointEntry() {
        if ($('#ddlBranches').val() > 0) {
            var controller = GL_ROOT + "Base/GetSalePointsbyBranch?branchId=" + $('#ddlBranches').val();
            $("#ddlSalePoints").UifSelect({ source: controller });

            // Setea el punto de venta de default
            setTimeout(function () {
                $("#ddlSalePoints").val($("#ViewBagSalePointBranchUserDefault").val());
            }, 500);
        }
    }

    /**
        * Bloqueo de campos en formulario principal.
        *
        */
    static LockFields() {
        $("#ddlBranches").attr("disabled", true);
        $("#ddlSalePoints").attr("disabled", true);
        $("#ddlAccountingCompanies").attr("disabled", "disabled");
        $("#ddlDestinations").attr("disabled", true);
        $("#ddlAccountingMovementTypes").attr("disabled", true);
    }

    /**
        * Debloqueo de campos en formulario principal.
        *
        */
    static UnlockFields() {
        $("#ddlEntryTypes").removeAttr("disabled");
        $("#ddlBranches").removeAttr("disabled");
        $("#ddlSalePoints").removeAttr("disabled");
        $("#ddlDestinations").removeAttr("disabled");
        $("#ddlAccountingMovementTypes").removeAttr("disabled");
        $("#ddlCurrencies").removeAttr("disabled");
    }

    /**
        * Validacion de Débitos y créditos.
        *
        */
    static ValidateCreditsAndDebits() {
        var result = false;
        var debits = 0;
        var credits = 0;

        var data = $("#entryTable").UifDataTable('getData');

        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].AccountingNatureId == 2) { //débito
                    debits = debits + parseFloat(((parseFloat(data[i].ExchangeRate.toString().replace(",", ".")) * parseFloat(data[i].Amount.toString().replace(",", ".")))).toFixed(2));
                }
                if (data[i].AccountingNatureId == 1) { //crédito
                    credits = credits + parseFloat(((parseFloat(data[i].ExchangeRate.toString().replace(",", ".")) * parseFloat(data[i].Amount.toString().replace(",", ".")))).toFixed(2));
                }
            }
            if (credits == debits) {
                result = true;
            }
        }

        return result;
    }

    /**
        * Agrega asiento tipo a la tabla de movimientos.
        *
        */
    static AddEntryType(entryTypeId) {
        MainLedgerEntry.GetEntryTypeAccountingsByEntryType(entryTypeId).then(function (entryTypeAccountings) {
            if (entryTypeAccountings.success) {
                if (entryTypeAccountings.result.length > 0) {
                    for (var x in entryTypeAccountings.result) {

                        var rowModel = new RowModel();

                        rowModel.BranchId = $("#mainForm").find("#ddlBranches").val();
                        rowModel.SalePointId = $("#mainForm").find("#ddlSalePoints").val();
                        rowModel.CompanyId = $("#mainForm").find("#ddlAccountingCompanies").val();
                        rowModel.DestinationId = $("#mainForm").find("#ddlDestinations").val();
                        rowModel.ExchangeRate = entryTypeAccountings.result[x].ExchangeRate;
                        rowModel.ExchangeRateFormated = 0;
                        rowModel.Amount = 0;
                        rowModel.AmountFormated = 0;
                        rowModel.IndividualId = 0;
                        rowModel.DocumentNumber = 0;
                        rowModel.Name = "";
                        rowModel.BankReconciliationId = 0;
                        rowModel.BankReconciliationDescription = "";
                        rowModel.ReceiptNumber = "";
                        rowModel.ReceiptDate = "";
                        rowModel.AccountingMovementTypeId = entryTypeAccountings.result[x].AccountingMovementTypeId;
                        rowModel.Date = $("#mainForm").find("#Date").val();
                        rowModel.AccountingNatureId = entryTypeAccountings.result[x].AccountingNatureId;
                        rowModel.AccountingNatureDescription = entryTypeAccountings.result[x].AccountingNatureDescription;
                        rowModel.Description = entryTypeAccountings.result[x].Description;
                        rowModel.AccountingAccountId = entryTypeAccountings.result[x].AccountingAccountId;
                        rowModel.AccountingAccountNumber = entryTypeAccountings.result[x].AccountingAccountNumber;
                        rowModel.AccountingAccountDescription = entryTypeAccountings.result[x].AccountingAccountDescription;
                        rowModel.CurrencyId = entryTypeAccountings.result[x].CurrencyId;
                        rowModel.CurrencyDescription = entryTypeAccountings.result[x].CurrencyDescription;
                        rowModel.ConceptId = entryTypeAccountings.result[x].PaymentConceptCd;
                        rowModel.ConceptDescription = entryTypeAccountings.result[x].PaymentConceptDescription;

                        var costCenterModel = new CostCenterRowModel();
                        costCenterModel.CostCenterId = entryTypeAccountings.result[x].CostCenterId;
                        costCenterModel.Description = entryTypeAccountings.result[x].CostCenterDescription;
                        costCenterModel.PercentageAmount = "0";
                        costCenterModel.PercentageAmountFormated = "0";

                        rowModel.CostCenters.push(costCenterModel);

                        var analysisModel = new AnalysisRowModel();
                        analysisModel.AnalysisId = entryTypeAccountings.result[x].AnalysisId;
                        analysisModel.AnalysisDescription = entryTypeAccountings.result[x].AnalysisDescription;
                        analysisModel.AnalysisConceptId = 0;
                        analysisModel.AnalysisConceptDescription = "";
                        analysisModel.Key = "";
                        analysisModel.Description = "";

                        rowModel.Analysis.push(analysisModel);

                        accounts.push(rowModel);
                    }

                    if (accounts.length > 0) {
                        for (var i = 0; i < accounts.length; i++) {
                            $('#entryTable').UifDataTable('addRow', accounts[i]);
                        }
                        $("#ddlCurrencies").val(accounts[0].CurrencyId);
                        $("#ddlCurrencies").trigger('change');
                    }
                }
            }
        });
    }

    /**
        * Valida el que se ingrese el pagador en modal de inserción.
        *
        */
    static ValidatePayerDocumentNumberEntryAdd() {
        if (documentNumberEntry != $("#entryModalAdd").find("#entryPayerDocumentNumber").val()) {
            $('#entryModalAdd').find("#entryPersonAlert").UifAlert('show', Resources.NoRegisteredPerson, "warning");
            $("#btnEntrySaveAdd").attr("disabled", true);
        } else {
            $("#entryModalAdd").find("#entryPersonAlert").UifAlert('hide');
            $("#btnEntrySaveAdd").attr("disabled", false);
        }
    }

    /**
        * Valida el que se ingrese el pagador en modal de edición.
        *
        */
    static ValidatePayerDocumentNumberEntryEdit() {
        if (documentNumberEntry != $("#entryModalEdit").find("#entryPayerDocumentNumber").val()) {
            $('#entryModalEdit').find("#entryPersonAlert").UifAlert('show', Resources.NoRegisteredPerson, "warning");
            $("#btnEntrySaveEdit").attr("disabled", true);
        } else {
            $("#entryModalEdit").find("#entryPersonAlert").UifAlert('hide');
            $("#btnEntrySaveEdit").attr("disabled", false);
        }
    }

    /**
        * Obtiene los asientos tipo.
        *
        */
    static GetEntryTypeAccountingsByEntryType(entryTypeId) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                type: "Post",
                url: GL_ROOT + "Entry/GetEntryTypeAccountingsByEntryType?entryTypeId=" + entryTypeId,
                success: function (entryTypeAccountings) {
                    resolve(entryTypeAccountings)
                }
            });
        });
    }

    /**
        * Setea el modelo para pasar al controlador.
        *
        */
    static SetDataLedgerEntry() {
        oLedgerEntryModel.AccountingCompanyId = $("#ddlAccountingCompanies").val();
        oLedgerEntryModel.AccountingModuleId = 0;
        oLedgerEntryModel.AccountingMovementTypeId = $("#ddlAccountingMovementTypes").val();
        oLedgerEntryModel.BranchId = $("#ddlBranches").val();
        oLedgerEntryModel.Date = $("#Date").val();
        oLedgerEntryModel.Description = $("#DailyEntryDescription").val();
        oLedgerEntryModel.EntryDestinationId = $("#ddlDestinations").val();
        oLedgerEntryModel.LedgerEntryId = 0;
        //items
        var items = $("#entryTable").UifDataTable('getData');

        if (items.length > 0) {
            for (var i = 0; i < items.length; i++) {
                oLedgerEntryModel.LedgerEntryItems.push(items[i]);
            }
        }

        oLedgerEntryModel.SalePointId = $("#ddlSalePoints").val();

        return oLedgerEntryModel;
    }

    /**
        * Crea inputs dinámicos en base a parametrización de análisis de conceptos.
        *
        * @param {Object} keys - Nombres de campos del registro seleccionado.
        * @param {Number} edit - Edición del registro seleccionado.
        * @param {Object} data - Valores de campos del registro seleccionado.
    */
    static CreateTexBoxDinamic(keys, edit, data) {
        var field = "";
        var divRow = "<div class='uif-row'>";
        var divColumn = "<div class='uif-col-4'>";
        var endDiv = "</div>";
        var response = data.split("|");

        for (var i = 0; i < keys.length; i++) {
            field = divColumn;
            field += "<label>" + keys[i].ColumnDescription + "</label>";

            if (edit == 1) {
                if (response[i] == undefined) {
                    field += "<input type='text' size='20' id='Key" + i + "' name='Key" + i + "' value='' />";
                }
                else {
                    field += "<input type='text' size='20' id='Key" + i + "' name='Key" + i + "' value='" + response[i] + "' />";
                }

                field += endDiv;
                $("#entryEditFrmAnalysis").find("#KeyFields").append(field);
            }
            else {
                field += "<input type='text' size='20' id='Key" + i + "' name='" + i + "' />";
                field += endDiv;
                $("#entryAddFrmAnalysis").find("#KeyFields").append(field);
            }
        }
    }

    /**
        * Crea inputs dinámicos en base a parametrización de análisis de conceptos.
        *
        * @param {Object} keys - Nombres de campos del registro seleccionado.
        * @param {Number} edit - Edición del registro seleccionado.
        * @param {Object} data - Valores de campos del registro seleccionado.
        */
    static CreateTexBoxDinamicOriginal(keys, edit, data) {
        var field = "";
        var divRow = "<div class='uif-row'>";
        var divColumn = "<div class='uif-col-6'>";
        var endDiv = "</div>";
        var response = data.split("|");

        for (var i = 0; i < keys.length; i++) {
            if (i == 0 || i == 2) {
                field = divRow + divColumn;
            }
            else {
                field += divColumn;
            }
            field += "<label>" + keys[i].ColumnDescription + "</label>";

            if (edit == 1) {
                if (response[i] == undefined) {
                    field += "<input type='text' size='20' id='Key" + i + "' name='Key" + i + "' value='' />";
                }
                else {
                    field += "<input type='text' size='20' id='Key" + i + "' name='Key" + i + "' value='" + response[i] + "' />";
                }

                field += endDiv;
                $("#entryEditFrmAnalysis").find("#KeyFields").append(field);
            }
            else {
                field += "<input type='text' size='20' id='Key" + i + "' name='" + i + "' />";
                field += endDiv;
                if (i == 1 || i == 3 || (i == keys.length - 1)) {
                    field += endDiv;
                    $("#entryAddFrmAnalysis").find("#KeyFields").append(field);
                }
            }
        }
    }

    /**
        * Crea dinámicamente los campos de la clave parametrizados para un concepto de análiss.
        *
        * @param {Number} analysisConceptId - Código de concepto de análisis.
        */
    static CreateKeyByAnalysisConcept(analysisConceptId) {
        $("#entryEditFrmAnalysis").find("#analysisAlert").UifAlert('hide');
        $("#entryEditFrmAnalysis").find("#KeyFields").html("");

        if (analysisConceptId > 0) {
            $.ajax({
                type: "GET",
                url: GL_ROOT + 'Base/GetKeysByAnalysisAndConceptId',
                data: {
                    "analysisId": $("#entryEditFrmAnalysis").find("#AnalysisId").val(),
                    "analysisConceptId": analysisConceptId
                },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.length > 0) {
                        keyLength = data.length;
                        MainLedgerEntry.CreateTexBoxDinamic(data, 1, $("#entryEditFrmAnalysis").find("#Key").val());
                    }
                    else {
                        $("#entryEditFrmAnalysis").find("#analysisAlert").UifAlert('show', Resources.MessageValidationKeysAnalysisConcept, "warning");
                    }
                }
            });
        } else {
            $("#entryEditFrmAnalysis").find("#KeyFields").html("");
        }
    }

    /**
        * Inicializa el modelo después de pasar al controlador.
        *
        */
    static InitializeDataLedgerEntry() {
        oLedgerEntryModel = {
            LedgerEntryId: 0,
            AccountingCompanyId: 0,
            AccountingMovementTypeId: 0,
            AccountingModuleId: 0,
            BranchId: 0,
            SalePointId: 0,
            EntryDestinationId: 0,
            Description: null,
            Date: null,
            LedgerEntryItems: []
        };

        oLedgerEntryItemModel = {
            LedgerEntryItemId: 0,
            CurrencyId: 0,
            AccountingAccountId: 0,
            AccountingAccountNumber: null,
            BankReconciliationId: 0,
            ReceiptDate: null,
            ReceiptNumber: 0,
            AccountingNatureId: 0,
            Description: null,
            ExchangeRate: 0,
            Amount: 0,
            LocalAmount: 0,
            IndividualId: 0,
            DocumentNumber: null,
            EntryTypeId: 0,
            CostCenters: [],
            Analysis: [],
            Postdated: []
        };

        oAnalysisModel = {
            AnalysisId: 0,
            AnalysisDescription: null,
            AnalysisConceptId: 0,
            AnalysisConceptDescription: null,
            Key: null,
            Description: null
        };

        oCostCenterModel = {
            CostCenterId: 0,
            Description: null,
            PercentageAmount: 0
        };

        oPostDatedModel = {
            PostDateTypeId: 0,
            PostDateTypeDescription: null,
            DocumentNumber: 0,
            CurrencyId: 0,
            CurrencyDescription: null,
            ExchangeRate: 0,
            IssueAmount: 0,
            LocalAmount: 0
        };
    }

}