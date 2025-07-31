
class TaxConceptsExpenses extends Uif2.Page {
    getInitialState() {
        TaxConceptsExpenses.LoadBranches();
        TaxConceptsExpenses.LoadTaxes();
        $("#listText").UifListView({
            displayTemplate: "#templateAccounting",
            source: null,
            selecttionType: 'single',
            height: 400
        });
    }
    bindEvents() {
        $('#btnAccept').click(TaxConceptsExpenses.AddTaxConceptsExpenses);
        $("#selectBranch").on("itemSelected", TaxConceptsExpenses.LoadAccountingConceptsByBranchId);
        $("#selectTax").on("itemSelected", TaxConceptsExpenses.LoadTaxCategories);
        $("#selectConcept").on("itemSelected", TaxConceptsExpenses.LoadAccountingConceptTaxesByConceptIdBranchId);
        $("#btnSave").click(TaxConceptsExpenses.CreateAccountingConceptTaxes);
        $('#btnNew').click(TaxConceptsExpenses.Clear);
        $('#btnClose').click(TaxConceptsExpenses.CancelView);
        
    }

    static LoadBranches() {
        TaxConceptsExpensesRequests.GetBranchs().done(function (data) {
            if (data.result) {
                $("#selectBranch").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', {
                    type: 'danger', message: data.result, autoclose: true
                });
            }
        });
    }

    static LoadAccountingConceptTaxesByConceptIdBranchId() {
        var branchId = $("#selectBranch").UifSelect("getSelected");
        var accountingConcenptId = $("#selectConcept").UifSelect("getSelected");
        $("#listText").UifListView("clear");
        TaxConceptsExpensesRequests.GetAccountingConceptTaxesByAccountingConceptIdBranchId(accountingConcenptId, branchId).done(function (data) {
            if (data.success) {
                $.each(data.result, function (index, value) {
                    $("#listText").UifListView("addItem", this);
                });
            }
            else {
                $.UifNotify('show', {
                    type: 'danger', message: data.result, autoclose: true
                });
            }
        });
    }

    static LoadAccountingConceptsByBranchId() {
        var branchId = $("#selectBranch").UifSelect("getSelected");
        TaxConceptsExpensesRequests.GetAccountingConceptsByBranchId(parseInt(branchId)).done(function (data) {
            if (data.result) {
                $("#selectConcept").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', {
                    type: 'danger', message: data.result, autoclose: true
                });
            }
        });
    }

    static LoadTaxes() {
        TaxConceptsExpensesRequests.GetTaxes().done(function (data) {
            if (data.result) {
                $("#selectTax").UifSelect({ sourceData: data.result });
            } else {
                $.UifNotify('show', {
                    type: 'danger', message: data.result, autoclose: true
                });
            }
        });
    }

    static LoadTaxCategories() {
        var selectTax = $("#selectTax").UifSelect("getSelected");
        TaxConceptsExpensesRequests.GetTaxCategoriesByTaxId(selectTax).done(function (data) {
            if (data.result) {
                $("#selectCategory").UifSelect({ sourceData: data.result });
            }
            else {
                $.UifNotify('show', {
                    type: 'danger', message: data.result, autoclose: true
                });
            }
        });
    }

    static AddTaxConceptsExpenses() {
        if (TaxConceptsExpenses.Validate()) {
            var conceptsExpenses = {};
            conceptsExpenses.BranchId = $("#selectBranch").UifSelect("getSelected");
            conceptsExpenses.AccountingConceptId = $("#selectConcept").UifSelect("getSelected");
            conceptsExpenses.TaxId = $("#selectTax").UifSelect("getSelected");
            conceptsExpenses.TaxDescription = $("#selectTax").UifSelect("getSelectedText");
            conceptsExpenses.TaxCategoryId = $("#selectCategory").UifSelect("getSelected");
            conceptsExpenses.TaxCategoryDescription = $("#selectCategory").UifSelect("getSelectedText");
            conceptsExpenses.EnableAddExpense = $('#chkAutomatic').prop('checked') ? 1 : 0;
            conceptsExpenses.EnableAutomatic = $('#chkExpense').prop('checked') ? 1 : 0;
            conceptsExpenses.AccountingConceptTaxId = 0;
            TaxConceptsExpenses.InsertConceptsExpenses(conceptsExpenses);
        }

    }

    static InsertConceptsExpenses(conceptsExpenses) {
        var taxConceptsExpenses = $('#listText').UifListView("getData");
        var taxConceptsFilter = taxConceptsExpenses.filter(function (item) {
            return item.BranchId === conceptsExpenses.BranchId
                && item.AccountingConceptId === conceptsExpenses.AccountingConceptId
                && item.TaxId === conceptsExpenses.TaxId
                && item.TaxCategoryId === conceptsExpenses.TaxCategoryId
        });
        if (taxConceptsFilter.length > 0) {
            $.UifNotify('show', {
                type: 'danger', message: Resources.Language.ErrorExpansesTaxes, autoclose: true
            });
        }
        else {
            $("#listText").UifListView("addItem", conceptsExpenses);
            $("#selectTax").UifSelect("setSelected", "");
            $("#selectCategory").UifSelect("setSelected", "");
            $("#chkAutomatic").prop("checked", false);
            $("#chkExpense").prop("checked", false);
        }
    }

    static CreateAccountingConceptTaxes() {
        var taxConceptsExpenses = $('#listText').UifListView("getData");
        if (taxConceptsExpenses.length > 0) {
            TaxConceptsExpensesRequests.CreateAccountingConceptTaxes(taxConceptsExpenses).done(function (data) {
                if (data.success) {
                    $.UifNotify('show', {
                        type: 'success', message: Resources.Language.SaveTaxesExpenses, autoclose: true
                    });
                }
                else {
                    $.UifNotify('show', {
                        type: 'danger', message: data.result, autoclose: true
                    });
                }
            });
        }
    }

    static Clear() {
        $("#selectTax").UifSelect("setSelected", "");
        $("#selectCategory").UifSelect("setSelected", "");
        $("#selectBranch").UifSelect("setSelected", "");
        $("#selectConcept").UifSelect("setSelected", "");
        $("#chkAutomatic").prop("checked", false);
        $("#chkExpense").prop("checked", false);
        $("#listText").UifListView("clear");
    }
    static Validate() {
        var error = "";
        if ($("#selectTax").UifSelect("getSelected") === "") {
            error = error + Resources.Language.LabelBranch + '<br>';
        }
        if ($("#selectCategory").UifSelect("getSelected") === "" || $("#selectCategory").UifSelect("getSelected") === null) {
            error = error + Resources.Language.LabelExistConcept + '<br>';
        }
        if ($("#selectBranch").UifSelect("getSelected") === "") {
            error = error + Resources.Language.LabelTaxes + '<br>';
        }
        if ($("#selectConcept").UifSelect("getSelected") === "" || $("#selectConcept").UifSelect("getSelected") === null) {
            error = error + Resources.Language.LabelCategory + '<br>';
        }
        if (error != "") {
            $.UifNotify('show', { 'type': 'info', 'message': AppResourcesPerson.LabelInformative + ":<br>" + error })
            return false;
        } else {
            return true;
        }
    }
    static CancelView() {
        window.location = rootPath + "Home/Index";
    }
}