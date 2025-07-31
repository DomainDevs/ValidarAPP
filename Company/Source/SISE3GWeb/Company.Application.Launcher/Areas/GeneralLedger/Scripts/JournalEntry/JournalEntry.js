$(() => {
    new JournalEntry();
});

class JournalEntryModel {
    constructor() {
        this.Id = 0;
        this.BranchId = 0;
        this.SalePointId = 0;
        this.AccountingCompanyId = 0;
        this.Description = "";
        this.Date = "";
        this.JournalEntryItems = [];
    }
};

class JournalEntryItemModel {
    constructor() {
        this.Id = 0;
        this.CurrencyId = 0;
        this.CurrencyDescription = "";
        this.AccountingConceptId = 0;
        this.AccountingConceptDescription = "";
        this.AccountingAccountId = 0;
        this.AccountingAccountNumber = "";
        this.AccountingAccountName = "";
        this.BankReconciliationId = 0;
        this.BankReconciliationDescription = "";
        this.ReceiptNumber = 0;
        this.ReceiptDate = "";
        this.AccountingNatureId = 0;
        this.AccountingNatureDescription = "";
        this.Description = "";
        this.Amount = 0;
        this.ExchangeRate = 0;
        this.IndividualId = 0;
        this.DocumentNumber = "";
        this.PersonName = "";
        this.CostCenters = [];
        this.Analysis = [];
        this.Postdated = [];
    }
};

class CostCenterModel {
    constructor() {
        this.Id = 0;
        this.CostCenterId = 0;
        this.Description = "";
        this.PercentageAmount = 0;
    };
};

class AnalysisModel {
    constructor() {
        this.AnalysisId;
        this.AnalysisDescription;
        this.AnalysisConceptId;
        this.AnalysisConceptDescription;
        this.Key;
        this.Description;
    };
};

class PostdatedModel {
    constructor() {
        this.PostDateTypeId;
        this.PostDateTypeDescription;
        this.DocumentNumber;
        this.CurrencyId;
        this.CurrencyDescription;
        this.ExchangeRate;
        this.IssueAmount;
        this.LocalAmount;
    };
};

var time;
var datePromise;
var accountingDatePromise;
var rowSelected;
var currentEditIndex = -1;
var costCenterCurrentEditIndex = -1;
var analysisCurrentEditIndex = -1;
var postDatedCurrentEditIndex = -1;
var tablePosition = -1;
var tableTypeId = 0;
var keyLength;
var postdatedExchangeRate = 0;
var printData;
var thirdAccountingUsed = parseInt($("#ViewBagThirdAccountingUsed").val());
var accountingAccountDescription = "";



class JournalEntry extends Uif2.Page {
    getInitialState() {
        checkAccountingDateControlIsShown();
        datePromise.then(function (isShown) {
            if (isShown) {
                clearTimeout(time);
                getAccountingDateDailyEntry($("#ViewBagModuleId").val());
                accountingDatePromise.then(function (accountingDate) {
                    $("#Date").val(accountingDate);
                });
            }
        });

        setTimeout(function () {
            try {
                SetSalePointEntry();
            } catch (e) { }
        }, 400);

        setTimeout(function () {
            $("#AccountingCompanyId").attr("disabled", "disabled");
        }, 1000);
    }

    bindEvents() {
        $('#tblDailyEntries').on('rowAdd', this.LoadEntryModal);
        $("#journalEntryModal").find("#AccountingAccountNumber").on("itemSelected", this.AccountingAccountAutocomplete);
        $("#journalEntryModal").find("#AccountingConceptId").on("itemSelected", this.AccountingConceptAutocomplete);
        $("#journalEntryModal").on("itemSelected", "#AccountingConceptDescription", this.AutocompleteAccountingConceptDescription);
        $("#journalEntryModal").find("#CurrencyId").on("itemSelected", this.LoadCurrencyExchangeRate);
        $("#journalEntryModal").find("#DocumentNumber").on("itemSelected", this.LoadPayerData);
        $("#journalEntryModal").find("#journalEntryModalSave").on("click", this.SaveItem);
        $("#journalEntryModal").find("#journalEntryModalCancel").on("click", this.ClearEntryModal);
        $("#journalEntryModal").find("#Amount").on("blur", this.setFormatAmount);
        $('#tblDailyEntries').on('rowEdit', this.EditItem);
        $('#tblDailyEntries').on('rowSelected', this.SelectEntry);
        $('#tblDailyEntries').on('rowDeselected', this.DeselectEntry);
        $('#tblDailyEntries').on('rowDelete', this.deleteEntry);
        $("#DeleteConfirmationModal").find("#deleteConfirmationAccept").on("click", this.Delete)
        $('#tblCostCenter').on('rowAdd', this.AddCostCenter);
        $('#tblCostCenter').on('rowEdit', this.EditCostCenter);
        $('#tblCostCenter').on('rowDelete', this.DeleteCostCenter);
        $("#costCenterModal").find("#CostCenterId").on("itemSelected", this.CostCenterIdAutocomplete)
        $("#costCenterModal").find("#Description").on("itemSelected", this.CostCenterDescriptionAutocomplete)
        $("#costCenterModal").find("#costCenterModalSave").on("click", this.SaveCostCenter)
        $('#tblAnalysis').on('rowAdd', this.AddAnalysis);
        $('#tblAnalysis').on('rowEdit', this.EditAnalysis);
        $('#tblAnalysis').on('rowDelete', this.DeleteAnalysis);
        $('#analysisModal').find("#AnalysisId").on("itemSelected", this.LoadConcepts)
        $('#analysisModal').find("#AnalysisConceptId").on("itemSelected", this.LoadAnalysConcept)
        $('#analysisModal').find("#analysisModalSave").on("click", this.SaveAnalysis)
        $("#postDatedModal").find("#CurrencyId").on("itemSelected", this.LoadPostDatedCurrencyExchangeRate);
        $("#postDatedModal").find("#IssueAmount").on("blur", this.SetPostDatedLocalAmount);
        $("#tblPostdated").on('rowAdd', this.AddPostDated);
        $("#tblPostdated").on('rowEdit', this.EditPostDated);
        $("#tblPostdated").on('rowDelete', this.DeletePostDated);
        $('#postDatedModal').find("#postDatedModalSave").on("click", this.SavePostDated)
        $("#SaveEntry").on("click", this.SaveEntry)
        $("#costCenterModal").find("#costCenterModalCancel").on("click", this.ClearCostCenterModal);
        $("#analysisModal").find("#analysisModalCancel").on("click", this.ClearAnalysisModal);
        $("#postDatedModal").find("#postDatedModalCancel").on("click", this.ClearPostDatedModal);
        $("#btnChoose").on("click", this.PrintReport);
        $("#BranchId").on("itemSelected", this.ItemSelectedBranch);
        $("#SalePointId").on("binded", this.BindedSalePoint);
        $('#journalEntryModal').on('closed.modal', this.closedModal);
    }

    closedModal() {
        $('#AccountingConceptId').UifAutoComplete('clean');
        $('#AccountingConceptDescription').UifAutoComplete('clean');
        $('#DocumentNumber').UifAutoComplete('clean');
        $('#AccountingAccountNumber').UifAutoComplete('clean');
    }
    static LockDocumentNumberAdd() {

        if (thirdAccountingUsed == 0) {
            $("#journalEntryModalForm").find('#DocumentNumber').attr("disabled", true);
        } else {
            $("#journalEntryModalForm").find('#DocumentNumber').attr("disabled", false);
        }
    }


    LoadEntryModal() {
        $("#mainAlert").UifAlert('hide');
        $("#mainForm").validate();
        if ($("#mainForm").valid()) {
            $('#journalEntryModal').UifModal('showLocal', Resources.AddMovement);
            JournalEntry.LockDocumentNumberAdd();

        }
    }
    AccountingAccountAutocomplete(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.AccountingAccountId > 0) {
                $('#journalEntryModal').find("#AccountingAccountId").val(selectedItem.AccountingAccountId);
                $("#journalEntryModal").find("#AccountingConceptId").val(" ");
                $("#journalEntryModal").find("#AccountingConceptDescription").val(" ");
                $('#journalEntryModal').find("#AccountingAccountDescription").val(selectedItem.Number + " - " + selectedItem.Description);
                $('#journalEntryModal').find("#AccountingAccountName").val(selectedItem.Description);
                $('#journalEntryModal').find("#journalEntryModalAlert").UifAlert('hide');
            } else {
                $("#journalEntryModal").find("#journalEntryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
                $('#journalEntryModal').find("#AccountingConceptId").val('');
                $('#journalEntryModal').find("#AccountingConceptDescription").val('');
                $('#journalEntryModal').find("#AccountingAccountDescription").val('');
                $('#journalEntryModal').find("#AccountingAccountName").val('');
            }
        }
    }

    /**
        * Permite buscar los conceptos contables en modal de inserción.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del concepto contable seleccionado.
        */
    AccountingConceptAutocomplete(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.Id > 0) {

                var accountingAccountId = selectedItem.AccountingAccount.AccountingAccountId;
                if (accountingAccountId > 0) {
                    $('#journalEntryModal').find("#AccountingAccountId").val(accountingAccountId);
                    $('#journalEntryModal').find("#AccountingConceptDescription").val(selectedItem.Description);
                    $('#journalEntryModal').find("#AccountingAccountNumber").val(selectedItem.AccountingAccount.Number);
                    $('#journalEntryModal').find("#AccountingAccountDescription").val(selectedItem.AccountingAccount.Number + " - " + selectedItem.AccountingAccount.Description);
                    accountingAccountDescription = selectedItem.AccountingAccount.Description;
                    $("#journalEntryModal").find("#journalEntryModalAlert").UifAlert('hide');
                } else {
                    $("#journalEntryModal").find("#journalEntryModalAlert").UifAlert('show', Resources.AccountingConceptNotAssociatedAccount, "info");
                    $('#journalEntryModal').find("#AccountingAccountId").val(accountingAccountId);
                    $('#journalEntryModal').find("#AccountingAccountNumber").val('');
                    $('#journalEntryModal').find("#AccountingAccountDescription").val('');
                    $('#journalEntryModal').find("#AccountingConceptDescription").val(selectedItem.DescriptionPaymentConcept);
                }
            } else {
                $("#journalEntryModal").find("#journalEntryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
                $('#journalEntryModal').find("#AccountingAccountId").val('');
                $('#journalEntryModal').find("#AccountingConceptDescription").val('');
                $('#journalEntryModal').find("#AccountingAccountNumber").val('');
                $('#journalEntryModal').find("#AccountingConceptDescription").val('');
                $('#journalEntryModal').find("#AccountingAccountDescription").val('');
            }
        } else {
            $('#journalEntryModal').find("#AccountingConceptId").val('');
            $('#journalEntryModal').find("#AccountingConceptDescription").val('');
        }
    }

    /**
        * Permite buscar los conceptos contables por descripción en modal de inserción.
        *
        * @param {String} event        - Seleccionar.
        * @param {Object} selectedItem - Objeto con valores del concepto contable seleccionado.
        */
    AutocompleteAccountingConceptDescription(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.Id > 0) {
                var accountingAccountId = selectedItem.AccountingAccount.AccountingAccountId;
                $("#journalEntryModal").find("#AccountingAccountId").val(accountingAccountId);
                $("#journalEntryModal").find("#AccountingConceptId").val(selectedItem.Id);
                $("#journalEntryModal").find("#AccountingAccountNumber").val(selectedItem.AccountingAccount.Number);
                $("#journalEntryModal").find("#AccountingAccountDescription").val(selectedItem.AccountingAccount.Number + " - " + selectedItem.AccountingAccount.Description);
                $("#journalEntryModal").find("#journalEntryModalAlert").UifAlert('hide');
            } else {
                $("#journalEntryModal").find('#AccountingConceptId').val('');
                $("#journalEntryModal").find('#AccountingConceptDescription').val('');
            }
        } else {
            $("#journalEntryModal").find("#journalEntryModalAlert").UifAlert('show', Resources.DataNotFound, "info");
            $("#journalEntryModal").find("#AccountingConceptId").val('');
            $("#journalEntryModal").find("#AccountingAccountNumber").val('');
            $("#journalEntryModal").find("#AccountingAccountDescription").val('');
        }
    }

    LoadCurrencyExchangeRate(event, selectedItem) {
        if (selectedItem.Id != "") {
            getExchangeByCurrency(selectedItem.Id).then(function (currencyData) {
                $("#ExchangeRate").val(currencyData);
            });

            if (selectedItem.Id == "0") {
                $("#Amount").attr("maxlength", 14);
            } else {
                $("#Amount").attr("maxlength", 10);
            }

        } else {
            $("#ExchangeRate").val("");
        }
    }
    LoadPayerData(event, selectedItem) {
        if (selectedItem != null) {
            if (selectedItem.IndividualId > 0) {
                $("#journalEntryModal").find("#PayerName").val(selectedItem.Name);
                $("#journalEntryModal").find("#IndividualId").val(selectedItem.IndividualId);
                $("#journalEntryModal").find("#personAlert").UifAlert('hide');
                $("#journalEntryModal").find("#journalEntryModalAlert").UifAlert('hide');
            } else {
                $('#journalEntryModal').find("#personAlert").UifAlert('show', Resources.PersonNotFound, "warning");
                $("#journalEntryModal").find("#IndividualId").val("");
                $("#journalEntryModal").find("#PayerName").val("");
                $("#journalEntryModal").find("#payerDocumentNumber").val("");
            }
        }
    }
    SaveItem() {
        if ($("#journalEntryModal").find("#AccountingAccountId").val() > 0) {
            if (thirdAccountingUsed == 0 || $("#journalEntryModal").find("#IndividualId").val() > 0 || $("#journalEntryModal").find("#IndividualId").val() != "") {
                $("#journalEntryModalForm").validate();
                var lastAmount = $("#journalEntryModal").find("#Amount").val();
                var clearAmount = ClearFormatCurrency($("#journalEntryModal").find("#Amount").val());
                var validAmount = clearAmount.split(".");
                if (validAmount[1] == 0) {
                    clearAmount = validAmount[0];
                }
                $("#journalEntryModal").find("#Amount").val(clearAmount.trim());
                if ($("#journalEntryModalForm").valid()) {

                    var journalEntryItemModel = new JournalEntryItemModel();

                    $("#journalEntryModal").find("#Amount").val(lastAmount);

                    journalEntryItemModel.CurrencyId = $("#journalEntryModal").find("#CurrencyId").val()
                    journalEntryItemModel.CurrencyDescription = $("#journalEntryModal").find("#CurrencyId option:selected").text();
                    journalEntryItemModel.AccountingConceptId = $("#journalEntryModal").find("#AccountingConceptId").val();
                    journalEntryItemModel.AccountingConceptDescription = $("#journalEntryModal").find("#AccountingConceptDescription").val();
                    journalEntryItemModel.AccountingAccountId = $("#journalEntryModal").find("#AccountingAccountId").val();
                    journalEntryItemModel.AccountingAccountNumber = $("#journalEntryModal").find("#AccountingAccountNumber").val();
                    journalEntryItemModel.AccountingAccountName = accountingAccountDescription;
                    journalEntryItemModel.BankReconciliationId = $("#journalEntryModal").find("#BankReconciliationId").val();
                    journalEntryItemModel.BankReconciliationDescription = $("#journalEntryModal").find("#BankReconciliationId option:selected").text();
                    journalEntryItemModel.ReceiptNumber = $("#journalEntryModal").find("#ReceiptNumber").val();
                    journalEntryItemModel.ReceiptDate = $("#journalEntryModal").find("#ReceiptDate").val();
                    journalEntryItemModel.AccountingNatureId = $("#journalEntryModal").find("#AccountingNatureId").val();
                    journalEntryItemModel.AccountingNatureDescription = $("#journalEntryModal").find("#AccountingNatureId option:selected").text();
                    journalEntryItemModel.Description = $("#journalEntryModal").find("#Description").val();
                    journalEntryItemModel.Amount = $("#journalEntryModal").find("#Amount").val();
                    journalEntryItemModel.ExchangeRate = $("#journalEntryModal").find("#ExchangeRate").val();
                    journalEntryItemModel.IndividualId = $("#journalEntryModal").find("#IndividualId").val();
                    journalEntryItemModel.DocumentNumber = $("#journalEntryModal").find("#DocumentNumber").val();
                    journalEntryItemModel.PersonName = $("#journalEntryModal").find("#PayerName").val();

                    if (currentEditIndex > -1) {
                        $('#tblDailyEntries').UifDataTable('editRow', journalEntryItemModel, currentEditIndex);
                    } else {
                        $('#tblDailyEntries').UifDataTable('addRow', journalEntryItemModel);
                    }
                    $('#AccountingConceptId').UifAutoComplete('clean');
                    $("#journalEntryModalForm").formReset();
                    $('#journalEntryModal').UifModal('hide');
                    $('#tblCostCenter').dataTable().fnClearTable();
                    $('#tblAnalysis').dataTable().fnClearTable();
                    $('#tblPostdated').dataTable().fnClearTable();
                    rowSelected = null;
                    currentEditIndex = -1;
                }
                else {
                    $("#journalEntryModal").find("#Amount").val(lastAmount);
                }
            } else {

                $('#journalEntryModal').find("#personAlert").UifAlert('show', Resources.NoRegisteredPerson, "warning");
            }
        } else {

            $('#journalEntryModal').find("#journalEntryModalAlert").UifAlert('show', Resources.NoEnteredAccountingAccount, "warning");
        }
    }
    ClearEntryModal() {
        ClearModalFields();
        $("#journalEntryModalForm").formReset();
    }

    setFormatAmount() {
        $("#journalEntryModal").find("#journalEntryModalAlert").UifAlert('hide');
        if ($("#journalEntryModal").find("#Amount").val() != "") {
            var entryAmount = ClearFormatCurrency($("#journalEntryModal").find("#Amount").val()).trim();
            $("#journalEntryModal").find("#Amount").val("$ " + NumberFormatSearch(entryAmount.trim(), "2", ".", ","));
        }
    }

    EditItem(event, data, position) {

        $("#journalEntryModal").find("#personAlert").UifAlert('hide');
        $("#journalEntryModal").find("#journalEntryModalAlert").UifAlert('hide');

        currentEditIndex = position;

        $("#journalEntryModal").UifModal('showLocal', Resources.Edit);

        $("#journalEntryModal").find("#AccountingAccountId").val(data.AccountingAccountId);
        $("#journalEntryModal").find("#AccountingAccountNumber").val(data.AccountingAccountNumber);
        $("#journalEntryModal").find("#AccountingAccountDescription").val(data.AccountingAccountNumber + " - " + data.AccountingAccountName);
        $("#journalEntryModal").find("#AccountingAccountName").val(data.AccountingAccountName);
        $("#journalEntryModal").find("#AccountingNatureId").val(data.AccountingNatureId);
        $("#journalEntryModal").find("#CurrencyId").val(data.CurrencyId);
        $("#journalEntryModal").find("#CurrencyId").trigger("change");
        $("#journalEntryModal").find("#AccountingConceptId").val(data.AccountingConceptId);
        $("#journalEntryModal").find("#AccountingConceptDescription").val(data.AccountingConceptDescription);
        $("#journalEntryModal").find("#Amount").val(data.Amount);

        if ($("#journalEntryModal").find("#Amount").val().indexOf("$") == -1) {
            $("#journalEntryModal").find("#Amount").val("$ " + NumberFormatSearch($("#journalEntryModal").find("#Amount").val(), "2", ".", ","));
        }


        $("#journalEntryModal").find("#IndividualId").val(data.IndividualId);
        $("#journalEntryModal").find("#DocumentNumber").val(data.DocumentNumber);
        $("#journalEntryModal").find("#PayerName").val(data.PersonName);
        $("#journalEntryModal").find("#Description").val(data.Description);
        $("#journalEntryModal").find("#BankReconciliationId").val(data.BankReconciliationId);
        $("#journalEntryModal").find("#ReceiptNumber").val(data.ReceiptNumber);
        $("#journalEntryModal").find("#ReceiptDate").val(data.ReceiptDate);

        rowSelected = data;
        //Lleno los centros de costos asociados
        if (rowSelected.CostCenters.length > 0) {
            $('#tblCostCenter').dataTable().fnClearTable();
            for (var i = 0; i < rowSelected.CostCenters.length; i++) {
                $('#tblCostCenter').UifDataTable('addRow', rowSelected.CostCenters[i]);
            }

            //valido que los centros de costos lleguen al 100%
            if (CostCenterPercentageTotal() < 100) {
                $("#mainAlert").UifAlert('show', Resources.SumPercentagesCostCentersDifferent, "warning");
            }
        } else {
            $('#tblCostCenter').dataTable().fnClearTable();
        }

        //lleno los analisis asociados
        if (rowSelected.Analysis.length > 0) {
            $('#tblAnalysis').dataTable().fnClearTable();
            for (var j = 0; j < rowSelected.Analysis.length; j++) {
                $('#tblAnalysis').UifDataTable('addRow', rowSelected.Analysis[j]);
            }
        } else {
            $('#tblAnalysis').dataTable().fnClearTable();
        }

        //lleno los postfechados asociados
        if (rowSelected.Postdated.length > 0) {
            $('#tblPostdated').dataTable().fnClearTable();
            for (var k = 0; k < rowSelected.Postdated.length; k++) {
                $('#tblPostdated').UifDataTable('addRow', rowSelected.Postdated[k]);
            }
        } else {
            $('#tblPostdated').dataTable().fnClearTable();
        }
    }
    SelectEntry(event, data) {
        rowSelected = data;

        //Lleno los centros de costos asociados
        if (rowSelected.CostCenters.length > 0) {
            $('#tblCostCenter').dataTable().fnClearTable();
            for (var i = 0; i < rowSelected.CostCenters.length; i++) {
                $('#tblCostCenter').UifDataTable('addRow', rowSelected.CostCenters[i]);
            }

            //valido que los centros de costos lleguen al 100%
            if (CostCenterPercentageTotal() < 100) {
                $("#mainAlert").UifAlert('show', Resources.SumPercentagesCostCentersDifferent, "warning");
            }
        } else {
            $('#tblCostCenter').dataTable().fnClearTable();
        }

        //lleno los analisis asociados
        if (rowSelected.Analysis.length > 0) {
            $('#tblAnalysis').dataTable().fnClearTable();
            for (var j = 0; j < rowSelected.Analysis.length; j++) {
                $('#tblAnalysis').UifDataTable('addRow', rowSelected.Analysis[j]);
            }
        } else {
            $('#tblAnalysis').dataTable().fnClearTable();
        }

        //lleno los postfechados asociados
        if (rowSelected.Postdated.length > 0) {
            $('#tblPostdated').dataTable().fnClearTable();
            for (var k = 0; k < rowSelected.Postdated.length; k++) {
                $('#tblPostdated').UifDataTable('addRow', rowSelected.Postdated[k]);
            }
        } else {
            $('#tblPostdated').dataTable().fnClearTable();
        }
    }

    deleteEntry(event, data, position) {
        $('#DeleteConfirmationModal').UifModal('showLocal', Resources.DeleteRecord);
        tablePosition = position;
        tableTypeId = 4;
    }
    DeselectEntry(event, data, position) {
        rowSelected = undefined;        
    }


    Delete() {
        $('#modalDelete').UifModal('hide');
        $("#DeleteConfirmationModal").UifModal('hide');
        DeleteRecord(tableTypeId, tablePosition);
        $("#mainAlert").UifAlert('show', Resources.RecordSuccessfullyDeleted, "success");
    }
    AddCostCenter() {
        if (rowSelected == undefined) {
            $("#recordAlert").UifAlert('show', Resources.SelectedRecordBeforeContinuing, "warning");
        } else {
            $("#recordAlert").UifAlert('hide');
            $("#mainAlert").UifAlert('hide');
            $('#costCenterModal').UifModal('showLocal', Resources.AddCostCenter);
        }
    }
    EditCostCenter(event, data, position) {
        costCenterCurrentEditIndex = position;

        $("#costCenterModal").UifModal('showLocal', Resources.Edit);
        $("#costCenterModal").find("#CostCenterId").val(data.CostCenterId);
        $("#costCenterModal").find("#Description").val(data.Description);
        $("#costCenterModal").find("#PercentageAmount").val(data.PercentageAmount);
    }
    DeleteCostCenter(event, data, position) {
        $('#DeleteConfirmationModal').UifModal('showLocal', Resources.DeleteRecord);
        tablePosition = position;
        tableTypeId = 1;
    }
    CostCenterIdAutocomplete(event, selectedItem) {
        $('#costCenterModal').find('#Description').val(selectedItem.Description);
    }
    CostCenterDescriptionAutocomplete(event, selectedItem) {
        $('#costCenterModal').find('#CostCenterId').val(selectedItem.CostCenterId);
    }
    SaveCostCenter() {
        $("#costCenterModalForm").validate();
        if ($("#costCenterModalForm").valid()) {

            var costCenterModel = new CostCenterModel();

            costCenterModel.CostCenterId = $("#costCenterModal").find("#CostCenterId").val();
            costCenterModel.Description = $("#costCenterModal").find("#Description").val();
            costCenterModel.PercentageAmount = $("#costCenterModal").find("#PercentageAmount").val().replace(",", ".");
            //costCenterRowModel.PercentageAmountFormated = $("#costCenterModal").find("#PercentageAmount").val();

            if (costCenterCurrentEditIndex > -1) {
                $('#tblCostCenter').UifDataTable('editRow', costCenterModel, costCenterCurrentEditIndex);
                rowSelected.CostCenters[costCenterCurrentEditIndex] = costCenterModel; //actualizo el registro en la variable rowSelected

                if (CostCenterPercentageTotal() > 100) {
                    $("#costCenterModal").find("#costCenterAlert").UifAlert('show', Resources.SumPercentagesCostCentersDifferent, "danger");
                } else {
                    $("#costCenterModalForm").formReset();
                    $('#costCenterModal').UifModal('hide');
                    $("#costCenterModal").find("#costCenterAlert").UifAlert('hide');
                    costCenterCurrentEditIndex = -1;
                }
            } else {
                if (!IsDuplicatedCostCenters(costCenterModel.CostCenterId)) {
                    $("#costCenterModal").find("#costCenterAlert").UifAlert('hide');
                    if ((CostCenterPercentageTotal() + parseFloat(parseFloat(costCenterModel.PercentageAmount.replace(",", ".")).toFixed(2))) <= 100) {

                        rowSelected.CostCenters.push(costCenterModel);

                        $('#tblCostCenter').UifDataTable('addRow', costCenterModel);
                        $("#costCenterModalForm").formReset();
                        $('#costCenterModal').UifModal('hide');
                        $("#costCenterModal").find("#costCenterAlert").UifAlert('hide');
                        costCenterCurrentEditIndex = -1;
                    } else {
                        $("#costCenterModal").find("#costCenterAlert").UifAlert('show', Resources.ValidateCostCenterPercentage, "danger");
                    }
                } else {
                    $("#costCenterModal").find("#costCenterAlert").UifAlert('show', Resources.DuplicatedRecord, "danger");
                }
            }
        }
    }
    AddAnalysis() {
        $("#analysisModal").find("#KeyFields").html("");

        if (rowSelected == undefined) {
            $("#recordAlert").UifAlert('show', Resources.SelectedRecordBeforeContinuing, "warning");
        } else {
            $("#recordAlert").UifAlert('hide');
            $('#analysisModal').UifModal('showLocal', Resources.AnalysisAdd);
        }
    }
    EditAnalysis(event, data, position) {
        analysisCurrentEditIndex = position;

        $("#analysisModal").find("#KeyFields").html("");
        $('#analysisModal').UifModal('showLocal', Resources.Edit);
        $("#analysisModal").find("#AnalysisId").val(data.AnalysisId);
        $("#analysisModal").find("#AnalysisConceptId").val(data.AnalysisConceptId);
        $("#analysisModal").find("#AnalysisConceptId").trigger("change");
        setTimeout(function () {
            $("#analysisModal").find("#AnalysisConceptId").val(data.AnalysisConceptId);
        }, 1000);
        $("#analysisModal").find("#Key").val(data.Key);
        $("#analysisModal").find("#Description").val(data.Description);
    }
    DeleteAnalysis(event, data, position) {
        $('#DeleteConfirmationModal').UifModal('showLocal', Resources.DeleteRecord);
        tablePosition = position;
        tableTypeId = 2;
    }
    LoadConcepts(event, selectedItem) {
        $("#analysisModal").find("#analysisAlert").UifAlert('hide');
        $("#analysisModal").find("#KeyFields").html("");

        if (selectedItem.Id > 0) {
            var controller = GL_ROOT + "Base/GetPaymentConceptsByAnalysisCode?analysisCodeId=" + selectedItem.Id;
            $("#analysisModal").find("#AnalysisConceptId").UifSelect({ source: controller });
        } else {
            $("#analysisModal").find("#AnalysisConceptId").UifSelect();
        }
    }
    LoadAnalysConcept(event, selectedItem) {
        $("#analysisModal").find("#analysisAlert").UifAlert('hide');
        $("#analysisModal").find("#KeyFields").html("");

        if (selectedItem.Id > 0) {
            $.ajax({
                type: "GET",
                url: GL_ROOT + 'Base/GetKeysByAnalysisAndConceptId',
                data: {
                    "analysisId": $("#analysisModal").find("#AnalysisId").val(),
                    "analysisConceptId": selectedItem.Id
                },
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.length > 0) {
                        keyLength = data.length;
                        CreateTexBoxDinamic(data, 0, "");
                    }
                    else {
                        $("#analysisModal").find("#analysisAlert").UifAlert('show', Resources.MessageValidationKeysAnalysisConcept, "warning");
                    }
                }
            });
        } else {
            $("#analysisModal").find("#KeyFields").html("");
        }
    }
    SaveAnalysis() {
        $("#analysisModalForm").validate();
        if ($("#analysisModalForm").valid()) {

            var keyValue = "";
            for (var i = 0; i < keyLength; i++) {
                keyValue += $("#analysisModal").find("#Key" + i).val() + "|";
            }

            if (keyValue.indexOf("undefined") == 0 || keyValue == "" || keyValue.indexOf("|") == 0) {
                $("#analysisModal").find("#analysisAlert").UifAlert('show', Resources.EnterAnalysisConceptKey, "warning");
                return;
            }

            keyValue = keyValue.substring(0, keyValue.length - 1);
            $("#analysisModal").find("#Key").val(keyValue);

            var analysisModel = new AnalysisModel();

            analysisModel.AnalysisId = $("#analysisModal").find("#AnalysisId").val();
            analysisModel.AnalysisDescription = $("#analysisModal").find("#AnalysisId option:selected").text();
            analysisModel.AnalysisConceptId = $("#analysisModal").find("#AnalysisConceptId").val();
            analysisModel.AnalysisConceptDescription = $("#analysisModal").find("#AnalysisConceptId option:selected").text();
            analysisModel.Key = $("#analysisModal").find("#Key").val();
            analysisModel.Description = $("#analysisModal").find("#Description").val();

            if (analysisCurrentEditIndex > -1) {
                // if (!IsDuplicatedAnalysis(analysisModel.AnalysisId, analysisModel.AnalysisConceptId, analysisModel.Key)) {

                $('#tblAnalysis').UifDataTable('editRow', analysisModel, analysisCurrentEditIndex);
                rowSelected.Analysis[analysisCurrentEditIndex] = analysisModel; //actualizo el registro en la variable rowSelected
                $("#dailyEntryEditFrmAnalysis").formReset();
                $('#dailyEntryModalEditFrmAnalysis').UifModal('hide');
                $("#dailyEntryModalEditFrmAnalysis").find("#analysisAlert").UifAlert('hide');
                // } else {
                //     $("#dailyEntryModalEditFrmAnalysis").find("#analysisAlert").UifAlert('show', Resources.DuplicatedRecord, "warning");
                // }

            } else {
                if (!IsDuplicatedAnalysis(analysisModel.AnalysisId, analysisModel.AnalysisConceptId, analysisModel.Key)) {
                    rowSelected.Analysis.push(analysisModel);

                    $('#tblAnalysis').UifDataTable('addRow', analysisModel);
                    $("#analysisModalForm").formReset();
                    $('#analysisModal').UifModal('hide');
                    $("#analysisModal").find("#analysisAlert").UifAlert('hide');
                } else {
                    $("#analysisModal").find("#analysisAlert").UifAlert('show', Resources.DuplicatedRecord, "warning");
                }
            }
        }
    }
    LoadPostDatedCurrencyExchangeRate(event, selectedItem) {
        if (selectedItem.Id != "") {
            getExchangeByCurrency(selectedItem.Id).then(function (currencyData) {
                postdatedExchangeRate = currencyData;
                $("#postDatedModal").find("#IssueAmount").trigger("blur");
            });
        } else {
            postdatedExchangeRate = 0;
            $("#postDatedModal").find("#IssueAmount").trigger("blur");
        }
    }
    SetPostDatedLocalAmount() {
        if ($("#postDatedModal").find("#IssueAmount").val() != "" || $("#postDatedModal").find("#IssueAmount").val() != undefined) {
            var total = 0;
            total = ($("#postDatedModal").find("#IssueAmount").val() * postdatedExchangeRate).toFixed(2);
            $("#postDatedModal").find("#LocalAmount").val(total);
        }
    }
    AddPostDated() {
        if (rowSelected == undefined) {
            $("#recordAlert").UifAlert('show', Resources.SelectedRecordBeforeContinuing, "warning");
        } else {
            $("#recordAlert").UifAlert('hide');
            $('#postDatedModal').UifModal('showLocal', Resources.PostDatedAdd);
        }
    }
    EditPostDated(event, data, position) {
        postDatedCurrentEditIndex = position;

        $("#postDatedModal").UifModal('showLocal', Resources.Edit);
        $("#dailyEntryEditFrmPostdated").find("#PostDateTypeId").val(data.PostDateTypeId);
        $("#dailyEntryEditFrmPostdated").find("#CurrencyId").val(data.CurrencyId);
        postdatedExchangeRate = data.ExchangeRateFormated;
        $("#dailyEntryEditFrmPostdated").find("#DocumentNumber").val(data.DocumentNumber);
        $("#dailyEntryEditFrmPostdated").find("#IssueAmount").val(data.IssueAmountFormated);
        $("#dailyEntryEditFrmPostdated").find("#LocalAmount").val(data.LocalAmountFormated);
    }
    DeletePostDated(event, data, position) {
        $('#DeleteConfirmationModal').UifModal('showLocal', Resources.DeleteRecord);
        tablePosition = position;
        tableTypeId = 3;
    }
    SavePostDated() {
        $("#postDatedModalForm").validate();
        if ($("#postDatedModalForm").valid()) {
            var postdatedModel = new PostdatedModel();

            postdatedModel.PostDateTypeId = $("#postDatedModal").find("#PostDateTypeId").val();
            postdatedModel.PostDateTypeDescription = $("#postDatedModal").find("#PostDateTypeId option:selected").text();
            postdatedModel.DocumentNumber = $("#postDatedModal").find("#DocumentNumber").val();
            postdatedModel.CurrencyId = $("#postDatedModal").find("#CurrencyId").val();
            postdatedModel.CurrencyDescription = $("#postDatedModal").find("#CurrencyId option:selected").text();
            postdatedModel.ExchangeRate = postdatedExchangeRate.toString().replace(",", ".");
            postdatedModel.IssueAmount = $("#postDatedModal").find("#IssueAmount").val().replace(",", ".");
            postdatedModel.LocalAmount = $("#postDatedModal").find("#LocalAmount").val().replace(",", ".");

            if (postDatedCurrentEditIndex > -1) {
                $('#tblPostdated').UifDataTable('editRow', postdatedModel, postDatedCurrentEditIndex);
                rowSelected.Postdated[postDatedCurrentEditIndex] = postdatedModel; //actualizo el registro en la variable rowSelected
                $("#dailyEntryEditFrmPostdated").formReset();
                $('#dailyEntryModalEditFrmPostdated').UifModal('hide');
            } else {
                if (!IsDuplicatedPostDated(postdatedModel.PostDateTypeId, postdatedModel.DocumentNumber)) {
                    rowSelected.Postdated.push(postdatedModel);

                    $('#tblPostdated').UifDataTable('addRow', postdatedModel);
                    $("#postDatedModalForm").formReset();
                    $('#postDatedModal').UifModal('hide');
                    $('#postDatedModal').find("#postdatedAlert").UifAlert('hide');
                } else {
                    $('#postDatedModal').find("#postdatedAlert").UifAlert('show', Resources.DuplicatedRecord, "warning");
                }
            }
        }
    }
    SaveEntry() {
        $("#mainAlert").UifAlert('hide');
        $("#mainForm").validate();
        if ($("#mainForm").valid()) {
            if (ValidateCostCenterAndAccount()) {
                if (ValidateCreditsAndDebits()) {
                    if (ValidateAllCostCenters()) {
                        $.blockUI({
                            css: {
                                border: 'none',
                                padding: '15px',
                                backgroundColor: '#000',
                                '-webkit-border-radius': '10px',
                                '-moz-border-radius': '10px',
                                opacity: .5,
                                color: '#fff'
                            },
                            message: "<h1>" + Resources.MessageWaiting + "</h1>"
                        });

                        var journalEntryModel = new JournalEntryModel();
                        journalEntryModel.Id = 0;
                        journalEntryModel.BranchId = $("#BranchId").val();
                        journalEntryModel.SalePointId = $("#SalePointId").val();
                        journalEntryModel.AccountingCompanyId = $("#AccountingCompanyId").val();
                        journalEntryModel.Description = $("#Description").val();
                        journalEntryModel.Date = $("#Date").val();

                        var items = $("#tblDailyEntries").UifDataTable('getData');

                        if (items.length > 0) {
                            for (var i = 0; i < items.length; i++) {

                                var journalEntryItemModel = new JournalEntryItemModel();
                                journalEntryItemModel.CurrencyId = items[i].CurrencyId;
                                journalEntryItemModel.AccountingAccountId = items[i].AccountingAccountId;
                                journalEntryItemModel.BankReconciliationId = items[i].BankReconciliationId;
                                journalEntryItemModel.ReceiptNumber = items[i].ReceiptNumber;
                                journalEntryItemModel.ReceiptDate = items[i].ReceiptDate;
                                journalEntryItemModel.AccountingNatureId = items[i].AccountingNatureId;
                                journalEntryItemModel.Description = items[i].Description;
                                journalEntryItemModel.Amount = ReplaceDecimalPoint((ClearFormatCurrency(items[i].Amount)));
                                journalEntryItemModel.ExchangeRate = ReplaceDecimalPoint(ClearFormatCurrency(items[i].ExchangeRate));
                                journalEntryItemModel.IndividualId = items[i].IndividualId;
                                journalEntryItemModel.CostCenters = items[i].CostCenters;
                                journalEntryItemModel.Analysis = items[i].Analysis;
                                journalEntryItemModel.Postdated = items[i].Postdated;
                                journalEntryModel.JournalEntryItems.push(journalEntryItemModel);
                            }
                        }

                        SaveJournalEntry(journalEntryModel).then(function (journalEntry) {
                            if (journalEntry.success == false) {
                                $("#mainAlert").UifAlert('show', journalEntry.result, "danger");
                                ClearFields();
                                $.unblockUI();
                            } else {
                                $('#tblDailyEntries').dataTable().fnClearTable();
                                $('#tblCostCenter').dataTable().fnClearTable();
                                $('#tblAnalysis').dataTable().fnClearTable();
                                $('#tblPostdated').dataTable().fnClearTable();

                                $("#mainAlert").UifAlert('show', Resources.EntrySuccessfullySaved + " " + journalEntry.result, "success");
                                $("#chooseMessageTitle").html("");
                                $("#chooseMessageModal").html(
                                    "<p class= 'text-info'>" + Resources.EntrySuccessfullySaved + " " + journalEntry.result + "</p>" + "<br/>" + Resources.PrintReport + ' ?');
                                $('#btnNoChoose').hide();
                                $('#modalChoose').appendTo("body").modal('show');
                                ClearFields();
                                $.unblockUI();
                                printData = journalEntry.result;
                            }
                        });
                    } else {
                        $("#mainAlert").UifAlert('show', Resources.Not100percentCostCenter, "warning");
                    }
                } else {
                    $("#mainAlert").UifAlert('show', Resources.UnbalancedEntry, "warning");
                }
            }
        }
    }
    ClearCostCenterModal() {
        $("#costCenterModal").find("#CostCenterId").val("");
        $("#costCenterModal").find("#Description").val("");
        $("#costCenterModal").find("#PercentageAmount").val("");
        $("#costCenterModalForm").formReset();
        costCenterCurrentEditIndex = -1;
    }
    ClearAnalysisModal() {
        $("#analysisModal").find("#AnalysisId").val("");
        $("#analysisModal").find("#AnalysisConceptId").val("");
        $("#analysisModal").find("#Description").val("");
        $("#analysisModalForm").formReset();
        analysisCurrentEditIndex = -1;
    }
    ClearPostDatedModal() {
        $("#postDatedModal").find("#PostDateTypeId").val("");
        $("#postDatedModal").find("#CurrencyId").val("");
        $("#postDatedModal").find("#DocumentNumber").val("");
        $("#postDatedModal").find("#IssueAmount").val("");
        $("#postDatedModal").find("#LocalAmount").val("");
        postdatedExchangeRate = 0;
        $("#postDatedModalForm").formReset();
        postDatedCurrentEditIndex = -1;
    }
    PrintReport() {
        showReportDailyEntry(printData);
        $('#modalChoose').modal('hide');
        printData = 0;
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
            $("#SalePointId").UifSelect({
                source: controller
            });
        } else {
            $("#SalePointId").UifSelect();
        }
    }

    /**
        * Setea el punto de venta por default una vez que esta cargado.
        *
        */
    BindedSalePoint() {
        //$("#SalePointId").val($("#ViewBagSalePointBranchUserDefault").val());
        $("#SalePointId").val("");
    }
}

//funcion que obtiene la fecha contable
function getAccountingDateDailyEntry(moduleDateId) {
    return accountingDatePromise = new Promise(function (resolve, reject) {
        $.ajax({
            url: GL_ROOT + "Base/GetAccountingDate",
            data: { "moduleDateId": moduleDateId },
            success: function (accountingDate) {
                resolve(accountingDate);
            }
        });
    });
}

//funcion que comprueba si el control de la fecha contable está cargado.
function checkAccountingDateControlIsShown() {
    var isShown;
    return datePromise = new Promise(function (resolve, reject) {
        time = setInterval(function () {
            if ($('#Date').is(":visible")) {
                isShown = true;
                resolve(isShown);
            }
        }, 3);
    });
}

//funcion que obtiene el importe del cambio a partir del codigo de moneda.
function getExchangeByCurrency(currencyId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: GL_ROOT + "Base/GetExchangeByCurrency",
            data: { "currencyId": currencyId },
            success: function (currencyData) {
                resolve(currencyData)
            }
        });
    });
}

//funcion que limpia los campos del modal de asiento
function ClearModalFields() {
    $("#journalEntryModal").find("#CurrencyId").val("");
    $("#journalEntryModal").find("#AccountingConceptId").val("");
    $("#journalEntryModal").find("#AccountingConceptDescription").val("");
    $("#journalEntryModal").find("AccountingAccountId").val("");
    $("#journalEntryModal").find("#AccountingAccountNumber").val("");
    $("#journalEntryModal").find("#AccountingAccountName").val("");
    $("#journalEntryModal").find("#BankReconciliationId").val("");
    $("#journalEntryModal").find("#ReceiptNumber").val("");
    $("#journalEntryModal").find("#ReceiptDate").val("");
    $("#journalEntryModal").find("#AccountingNatureId").val("");
    $("#journalEntryModal").find("#Description").val("");
    $("#journalEntryModal").find("#Amount").val("");
    $("#journalEntryModal").find("#ExchangeRate").val("");
    $("#journalEntryModal").find("#IndividualId").val("");
    $("#journalEntryModal").find("#DocumentNumber").val("");
    $("#journalEntryModal").find("#PayerName").val("");
    currentEditIndex = -1;
}

//funcion para eliminación de registros en tablas de centros de costos/analisis/postfechados
// 1 - Centro de Costos
// 2 - Analisis
// 3 - Postfechados
// 4 - Movimientos
function DeleteRecord(tableId, position) {
    //borro centros de costos
    if (tableId == 1) {
        $('#tblCostCenter').UifDataTable('deleteRow', position);
        rowSelected.CostCenters.splice(position, 1);
    }

    //borro analisis
    if (tableId == 2) {
        $('#tblAnalysis').UifDataTable('deleteRow', position);
        rowSelected.Analysis.splice(position, 1);
    }

    //borro postfechados
    if (tableId == 3) {
        $('#tblPostdated').UifDataTable('deleteRow', position);
        rowSelected.Postdated.splice(position, 1);
    }

    //borro movimientos
    if (tableId == 4) {
        $('#tblDailyEntries').UifDataTable('deleteRow', position);

        //limpio tabla de centro de costos
        $('#tblCostCenter').dataTable().fnClearTable();

        //limpio tabla de analásis
        $('#tblAnalysis').dataTable().fnClearTable();

        //limpio tabla de postfechados
        $('#tblPostdated').dataTable().fnClearTable();

        //"de-selecciono" la fila
        rowSelected = null;
        currentEditIndex = -1;
    }
}
//validacion de cuenta contable y centro de costos
function ValidateCostCenterAndAccount() {
    var dataAccount = $("#tblDailyEntries").UifDataTable('getData');
    var dataCostCenter = $("#tblCostCenter").UifDataTable('getData');
    if (dataAccount.length <= 0) {
        $("#mainAlert").UifAlert('show', Resources.NoEnteredAccountingAccount, "warning");
        return false;
    }
    else if (dataCostCenter.length <= 0) {
        $("#mainAlert").UifAlert('show', Resources.CostCenterAdd, "warning");
        return false;
    }
    return true;
}

//Validacion de Débitos y créditos
function ValidateCreditsAndDebits() {
    var result = false;
    var debits = 0;
    var credits = 0;

    var data = $("#tblDailyEntries").UifDataTable('getData');
    if (data.length > 0) {
        for (var i = 0; i < data.length; i++) {

            if (data[i].Amount.toString().indexOf("$") != -1) {
                data[i].Amount = ClearFormatCurrency(data[i].Amount.toString()).trim();
            }

            if (data[i].AccountingNatureId == 2) //debito
                debits = debits + parseFloat(((parseFloat(data[i].ExchangeRate.replace(",", ".")) * parseFloat(data[i].Amount.toString().replace(",", ".")))).toFixed(2));
            if (data[i].AccountingNatureId == 1) //credito
                credits = credits + parseFloat(((parseFloat(data[i].ExchangeRate.replace(",", ".")) * parseFloat(data[i].Amount.toString().replace(",", ".")))).toFixed(2));
        }
        if (credits == debits)
            result = true;
    }
    return result;
}

//Valida todos los centros de costos
function ValidateAllCostCenters() {

    var result = false;
    var data = $("#tblDailyEntries").UifDataTable('getData');
    var totalCostCenters = 0;

    for (var i = 0; i < data.length; i++) {
        if (data[i].CostCenters.length > 0) {
            for (var j = 0; j < data[i].CostCenters.length; j++) {
                //totalCostCenters = totalCostCenters + parseFloat(data[i].CostCenters[j].PercentageAmountFormated);
                totalCostCenters = totalCostCenters + parseFloat(data[i].CostCenters[j].PercentageAmount);
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

//Valida porcentaje de centros de costos
function CostCenterPercentageTotal() {
    var totalPercentage = 0;
    var costCenterData = $("#tblCostCenter").UifDataTable('getData');

    if (costCenterData.length > 0) {
        for (var i = 0; i < costCenterData.length; i++) {
            totalPercentage = totalPercentage + parseFloat(costCenterData[i].PercentageAmount.replace(",", "."));
        }
    }

    return totalPercentage;

}

//Valida centro de costos previamente ingresados
function IsDuplicatedCostCenters(costCenterId) {
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

//Valida analisis previamente ingresados
function IsDuplicatedAnalysis(analysisCodeId, analysisConceptId, key) {
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

//Valida postfechados previamente ingresados
function IsDuplicatedPostDated(postDatedTypeId, documentNumber) {
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

function CreateTexBoxDinamic(keys, edit, data) {
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
            $("#analysisModal").find("#KeyFields").append(field);
        }
        else {
            field += "<input type='text' size='20' id='Key" + i + "' name='" + i + "' />";
            field += endDiv;
            $("#analysisModal").find("#KeyFields").append(field);
        }
    }
}

function CreateTexBoxDinamicOriginal(keys, edit, data) {
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
            $("#analysisModal").find("#KeyFields").append(field);
        }
        else {
            field += "<input type='text' size='20' id='Key" + i + "' name='" + i + "' />";
            field += endDiv;
            if (i == 1 || i == 3 || (i == keys.length - 1)) {
                field += endDiv;
                $("#analysisModal").find("#KeyFields").append(field);
            }
        }
    }
}

function CreateKeyByAnalysisConcept(analysisConceptId) {
    $("#analysisModal").find("#analysisAlert").UifAlert('hide');
    $("#analysisModal").find("#KeyFields").html("");

    if (analysisConceptId > 0) {
        $.ajax({
            type: "GET",
            url: GL_ROOT + 'Base/GetKeysByAnalysisAndConceptId',
            data: {
                "analysisId": $("#analysisModal").find("#AnalysisId").val(),
                "analysisConceptId": analysisConceptId
            },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.length > 0) {
                    keyLength = data.length;
                    CreateTexBoxDinamic(data, 1, $("#analysisModal").find("#Key").val());
                }
                else {
                    $("#analysisModal").find("#analysisAlert").UifAlert('show', Resources.MessageValidationKeysAnalysisConcept, "warning");
                }
            }
        });
    } else {
        $("#analysisModal").find("#KeyFields").html("");
    }
}

//Función que elimina los campos del formulario principal
function ClearFields() {
    $("#BranchId").val($("#ViewBagBranchDefault").val());
    $("#BranchId").trigger("change");
    $("#SalePointId").val("");
    $("#AccountingCompanyId").val(Resources.DefaultCompanyValue);
    $("#Description").val("");
    getAccountingDateDailyEntry($("#ViewBagModuleId").val());
    rowSelected = null;
    $('#tblCostCenter').dataTable().fnClearTable();
    $('#tblAnalysis').dataTable().fnClearTable();
    $('#tblPostdated').dataTable().fnClearTable();
    $('#tblDailyEntries').dataTable().fnClearTable();
}

function SaveJournalEntry(journalEntryModel) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            url: GL_ROOT + "JournalEntry/SaveJournalEntry",
            data: { "journalEntryModel": journalEntryModel },
            success: function (journalEntry) {
                resolve(journalEntry);
            }
        });
    });
}

//funcion que muestra reporte en pantalla
function showReportDailyEntry(entryNumber) {
    window.open(GL_ROOT + "Reports/ShowDailyEntryConsultationReport?entryNumber="
        + entryNumber, 'mywindow', 'fullscreen=yes, scrollbars=auto');
}

/**
        * Setea el punto de venta de default.
        *
        */
function SetSalePointEntry() {
    if ($('#BranchId').val() > 0) {
        var controller = GL_ROOT + "Base/GetSalePointsbyBranch?branchId=" + $('#BranchId').val();
        $("#SalePointId").UifSelect({ source: controller });

        // Setea el punto de venta de default
        setTimeout(function () {
            $("#SalePointId").val($("#ViewBagSalePointBranchUserDefault").val());
        }, 500);
    }
}


