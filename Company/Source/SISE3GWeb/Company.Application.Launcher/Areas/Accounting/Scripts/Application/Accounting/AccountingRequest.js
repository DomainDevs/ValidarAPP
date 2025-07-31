class AccountingRequest {

    static GetBranchs() {
        return $.ajax({
            type: "GET",
            url: ACC_ROOT + 'Common/GetBranchs',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetSalesPointByBranchId(branchUserDefault) {
        return $.ajax({
            type: "GET",
            url: ACC_ROOT + 'Common/GetSalesPointByBranchId?branchId=' + branchUserDefault,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetAccountingCompanies() {
        return $.ajax({
            type: "GET",
            url: ACC_ROOT + 'Common/GetAccountingCompanies',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetNatures() {
        return $.ajax({
            type: "GET",
            url: ACC_ROOT + "Common/GetNatures",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCurrencies() {
        return $.ajax({
            type: "GET",
            url: ACC_ROOT + "Common/GetCurrencies",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCurrenciesnumber(number) {
        return $.ajax({
            type: "GET",
            url: ACC_ROOT + "Parameters/GetAccountintAccountByNumber?query=" + number,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAnalysis() {
        return $.ajax({
            type: "GET",
            url: ACC_ROOT + "Common/GetAnalysis",
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCostCenterByGeneralLedgerId(generalLedgerId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PaymentRequest/GetCostCenterByAccountingAccountId",
            data: JSON.stringify({ accountingAccountId: generalLedgerId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
        });
    }

    static DeleteTempApplicationAccounting(tempApplicationAccountingId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Accounting/DeleteTempApplicationAccounting",
            data: { "tempApplicationAccountingId": tempApplicationAccountingId }
        });
    }
    static GetTempAccountingTransactionItemByTempApplicationId(tempApplicationId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Accounting/GetTempAccountingTransactionItemByTempApplicationId?tempApplicationId=" + tempApplicationId,
        });
    }
    static GetTempAccountingTransactionByTempAccountingApplicationId(tempAccountingApplicationId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Accounting/GetTempAccountingTransactionByTempAccountingApplicationId?tempAccountingApplicationId=" + tempAccountingApplicationId,
        });
    }
    static SaveTempAccountingTransactionRequest(request) {
        return $.ajax({
            type: "POST",
            url: ACC_ROOT + "Accounting/SaveTempAccountingTransactionRequest",
            data: { "accountingTransactionModel": request }
        });
    }
    static GetCurrencyExchangeRate(rateDate, currencyId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Common/GetCurrencyExchangeRate",
            data: { "rateDate": rateDate, "currencyId": currencyId }
        });
    }
    static GetConceptKeysByAnalysisConceptId(analysisConceptId) {
        return $.ajax({
            async: false,
            type: "GET",
            url: ACC_ROOT + "Common/GetConceptKeysByAnalysisConceptId",
            data: {
                "analysisConceptId": analysisConceptId
            },
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static IsBankReconciliation(generalLedgerId) {
        return $.ajax({
            type: "POST",
            async: false,
            url: ACC_ROOT + "Accounting/IsBankReconciliation",
            data: { "generalLedgerId": generalLedgerId }
        });
    }
    static GetTempApplicationAccountingAnalysisByTempAppAccountingId(tempAppAccountingId) {
        return $.ajax({
            type: "GET",
            async: false,
            url: ACC_ROOT + "Accounting/GetTempApplicationAccountingAnalysisByTempAppAccountingId",
            data: { "tempAppAccountingId": tempAppAccountingId }
        });
    }
    static GetTempApplicationAccountingCostCenterByTempAppAccountingId(tempAppAccountingId) {
        return $.ajax({
            type: "GET",
            async: false,
            url: ACC_ROOT + "Accounting/GetTempApplicationAccountingCostCenterByTempAppAccountingId",
            data: { "tempAppAccountingId": tempAppAccountingId }
        });
    }
    static GetAnalysisConceptByAnalysisId(analysisId) {
        return $.ajax({
            type: "GET",
            async: false,
            url: ACC_ROOT + "Common/GetAnalysisConceptByAnalysisId",
            data: { "analysisId": analysisId }
        });
    }
    static CheckoutAnalysisCodeByAnalysisConceptKeyId(analysisConceptKeyId, keyDescription) {
        return $.ajax({
            type: "GET",
            async: false,
            url: ACC_ROOT + "Accounting/CheckoutAnalysisCodeByAnalysisConceptKeyId",
            data: { "analysisConceptKeyId": analysisConceptKeyId, "keyDescription": keyDescription}
        });
    }

}