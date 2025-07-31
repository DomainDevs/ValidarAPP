class FinancialPlanRequest {

    static GetEndorsementsByPolicyFilter(policyFilter) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/FinancialPlan/GetEndorsementsByPolicyFilter',
            data: JSON.stringify({ policyFilter: policyFilter }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetBranches() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/FinancialPlan/GetBranches',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/FinancialPlan/GetPrefixes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
     
    static GetPaymentMethod() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/FinancialPlan/GetPaymentMethod',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPoliciesByQuery(prefixId, branchId, query) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/FinancialPlan/GetPoliciesByQuery',
            data: JSON.stringify({ prefixId: prefixId, branchId: branchId, query: query }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPayersByEndorsementId(endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/FinancialPlan/GetPayersByEndorsementId',
            data: JSON.stringify({ endorsementId: endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetFinancialPolicyInfo(endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/FinancialPlan/GetFinancialPolicyInfo',
            data: JSON.stringify({ endorsementId: endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPaymentsScheduleByProductId(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/FinancialPlan/GetPaymentsScheduleByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }   

    static QuotePaymentPlan(filterFinancialPlanDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/FinancialPlan/QuotePaymentPlan',
            data: JSON.stringify({ filterFinancialPlanDTO: filterFinancialPlanDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }  
    static CreatePaymentPlan(filterFinancialPlanDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Accounting/FinancialPlan/CreatePaymentPlan',
            data: JSON.stringify({ filterFinancialPlanDTO: filterFinancialPlanDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }  
}