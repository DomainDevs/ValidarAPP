class DialogSearchPoliciesRequest {

    static GetLoadSearchByForPolicies() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Accounting/Common/LoadSearchByForPolicies',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetLoadBranch() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Accounting/ReceiptApplication/LoadBranch',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetLoadPrefix() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Accounting/ReceiptApplication/LoadPrefix',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveTempPremiumReceivableRequest(Data) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PremiumsReceivable/SaveTempPremiumReceivableRequest",
            data: JSON.stringify({ "premiumReceivable": Data }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static UpdTempApplicationPremiumCommission(Data) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PremiumsReceivable/UpdTempApplicationPremiumCommission",
            data: JSON.stringify({ "applicationPremiumCommisionDTO": Data }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static UpdTempApplicationPremiumComponents(Data) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PremiumsReceivable/UpdTempApplicationPremiumComponents",
            data: JSON.stringify({ "updTempApplicationPremiumComponentDTO": Data}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveTempPremiumComponentReceivableRequest(Data, DataCommission, temApplicationId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PremiumsReceivable/SaveTempApplicationPremiumComponents",
            data: JSON.stringify({
                "premiumReceivableItemModel": Data, "": DataCommission,"tempApplicationPremiumId": temApplicationId
            }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    
    static GetTempPremiumReceivableItemByTempImputationId(tempImputationId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PremiumsReceivable/GetTempPremiumReceivableItemByTempImputationId",
            data: JSON.stringify({ "tempImputationId": tempImputationId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetDepositPremiumTransactionByPayerId(payerId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PremiumsReceivable/GetDepositPremiumTransactionByPayerId",
            data: JSON.stringify({ "payerId": payerId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static EchangeRateCollect(currencyId, applyAccountingDate, applyCollecId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PremiumsReceivable/GetEchangeRateByCollect",
            data: JSON.stringify({ "currencyCode": currencyId, "accountingDate": applyAccountingDate, "applicationCollectId": applyCollecId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CalculateExchangeRateTolerance(newRate, idCurrency) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "Billing/CalculateExchangeRateTolerance",
            data: JSON.stringify({ "newRate": ReplaceDecimalPoint(newRate), "currencyId": idCurrency }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTempApplicationPremiumComponentsByTemApp(tempApp) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PremiumsReceivable/GetTempApplicationPremiumComponentsByTemApp",
            data: JSON.stringify({ "tempApp": tempApp }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPremiumComponentsByEndorsementIdQuotaNumber(endorsementId, quota) {
        return $.ajax({
            async: false,
            type: "POST",
            url: rootPath + "UnderWriting/UnderWriting/GetPayerPaymetComponents",
            data: JSON.stringify({ EndorsementId: endorsementId, Quota: quota }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEchangeRateByCurrencyId(currencyId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PremiumsReceivable/GetEchangeRateByCurrencyId",
            data: JSON.stringify({ "currencyCode": currencyId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static DeleteTempUPD(tempPremiumReceivableId) {
        $.ajax({
            async: false,
            url: ACC_ROOT + "PremiumsReceivable/DeleteTempUsedDepositPremiumRequest",
            data: { "tempPremiumReceivableId": tempPremiumReceivableId }
        });
    }
    static DeleteTempPremiumRecievableTransactionItem(tempImputationId, premiumReceivableItemId, isReversion) {

        return $.ajax({
            async: false,
            url: ACC_ROOT + "PremiumsReceivable/DeleteTempPremiumRecievableTransactionItem",
            data: { "tempImputationCode": tempImputationId, "tempPremiumReceivableCode": premiumReceivableItemId, isReversion: isReversion}
        });
    }
    static SearchPolicy(url) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + url,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SearchDepositPremiums(PayerId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PremiumsReceivable/GetDepositPremiumTransactionByPayerId?payerId=" + PayerId,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SearchDisconutedCommission(PolicyId, EndorsementId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PremiumsReceivable/SearhDiscountedCommission",
            data: JSON.stringify({ "endorsementId": EndorsementId, "policyId": PolicyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SearhTempDiscountedCommission(PolicyId, EndorsementId, AppPremiumId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PremiumsReceivable/SearhTempDiscountedCommission",
            data: JSON.stringify({ "endorsementId": EndorsementId, "policyId": PolicyId,"tempApplicationId": AppPremiumId}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static ValidatePolicyComponents(policyId, endorsementId) {
        return $.ajax({
            type: "POST",
            async: false,
            url: ACC_ROOT + "PremiumsReceivable/ValidatePolicyComponents",
            data: { "policyId": policyId, "endorsementId": endorsementId }
        });
    }    
    static GetTemporalApplicationByEndorsementIdPaymentNumber(tempApplicationId, endorsementId, paymentNum) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PremiumsReceivable/GetTemporalApplicationByEndorsementIdPaymentNumber",
            data: {
                "tempApplicationId": tempApplicationId,
                "endorsementId": endorsementId,
                "paymentNum": paymentNum
            }
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
    static GetPolicyPaymentBySearch(searchPolicyPayment) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PremiumsReceivable/PremiumReceivableSearchPolicy",
            data: { "searchPolicyPayment": searchPolicyPayment}
        });
    }

    static showReportExcelPolicyPayments(tempApplicationId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: ACC_ROOT + "PremiumsReceivable/GeneratePolicyPaymentsExcel",
            data: {
                "tempApplicationId": parseInt(tempApplicationId)
            },
        });

    }
}