class TaxConceptsExpensesRequests {
    static GetBranchs() {
        return $.ajax({
            type: 'GET',
            url: rootPath + 'Parametrization/TaxConceptsExpenses/GetBranchs',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            async: false
        });
    }

    static GetAccountingConceptsByBranchId(branchId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/TaxConceptsExpenses/GetAccountingConceptsByBranchId',
            data: JSON.stringify({ branchId: branchId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            async: false
        });
    }

    static GetTaxes() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/TaxConceptsExpenses/GetTaxes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            async: false
        });
    }

    static GetTaxCategoriesByTaxId(taxId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/TaxConceptsExpenses/GetTaxCategoriesByTaxId',
            data: JSON.stringify({ taxId: taxId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            async: false
        });
    }

    static CreateAccountingConceptTaxes(accountingConceptTaxesDTO) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/TaxConceptsExpenses/CreateAccountingConceptTaxes',
            data: JSON.stringify({ accountingConceptTaxesDTO: accountingConceptTaxesDTO }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            async: false
        });
    }

    static GetAccountingConceptTaxesByAccountingConceptIdBranchId(accountingConceptId, branchId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Parametrization/TaxConceptsExpenses/GetAccountingConceptTaxesByAccountingConceptIdBranchId',
            data: JSON.stringify({ accountingConceptId: accountingConceptId, branchId: branchId  }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            async: false
        });
    }
}