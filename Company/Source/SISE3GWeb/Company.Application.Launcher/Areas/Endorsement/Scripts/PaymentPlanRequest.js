class PaymentPlanRequest {
    static GetCurrentSummary(Data) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetCurrentStatusPolicyByEndorsementIdIsCurrent?endorsementId=' + Data + '&isCurrent=' + false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetPaymentPlanIdByProductId(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetPaymentPlanIdByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetQuotasbyPaymentPlanId(paymentPlanId, summary, currentFrom, currentTo, issueDate) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetQuotasbyPaymentPlanId',
            data: JSON.stringify({ paymentPlanId: paymentPlanId, summary: summary, currentFrom: currentFrom, currentTo: currentTo, issueDate: issueDate }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }    

}