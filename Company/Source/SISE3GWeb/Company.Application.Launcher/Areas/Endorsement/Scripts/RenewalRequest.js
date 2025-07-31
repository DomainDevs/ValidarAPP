class RenewalRequest {
    static GetCurrentSummaryByEndorsementId(endorsementId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetCurrentStatusPolicyByEndorsementIdIsCurrent?endorsementId=' + endorsementId + '&isCurrent=' + false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetTemporalById(temporalId, hiddenEndorsementController) {
        return $.ajax({
            type: "POST",
            url: rootPath + hiddenEndorsementController + '/GetTemporalById',
            data: JSON.stringify({ id: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static CreateTemporal(renewalModel, hiddenEndorsementController, hiddenIsUnderIdenticalConditions) {
        return $.ajax({
            type: 'POST',
            url: rootPath + hiddenEndorsementController + '/CreateTemporal',
            dataType: 'json',
            data: JSON.stringify({ renewalModel: renewalModel, identicalCondition: hiddenIsUnderIdenticalConditions }),
            contentType: 'application/json; charset=utf-8'
        });
    }
    static CreateEndorsement(hiddenTemporalId, hiddenPolicyNumber, hiddenEndorsementController) {
        return $.ajax({
            type: "POST",
            url: rootPath + hiddenEndorsementController + '/CreateEndorsement',
            data: JSON.stringify({ temporalId: hiddenTemporalId, policyNumber: hiddenPolicyNumber}),
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetExchangeRateByCurrencyId(currencyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetExchangeRateByCurrencyId',
            data: JSON.stringify({ currencyId: currencyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GenerateReportTemporary(tempId, prefixId, operationId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Printing/Printer/GenerateReportTemporary',
            data: JSON.stringify({ temporaryId: tempId, prefixId: prefixId, riskSince: 0, riskUntil: 0, operationId: operationId, tempAuthorization: false }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}