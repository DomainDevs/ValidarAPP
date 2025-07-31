class ExtensionRequest{

    static GetCurrentSummaryByEndorsementId(endorsementId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetCurrentStatusPolicyByEndorsementIdIsCurrent?endorsementId=' + endorsementId + '&isCurrent=' + false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetTemporalById(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/GetTemporalById',
            data: JSON.stringify({ id: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateTemporal(extensionModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + glbPolicy.EndorsementController + '/CreateTemporal',
            dataType: 'json',
            data: JSON.stringify({ extensionModel: extensionModel }),
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateEndorsement() {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/CreateEndorsement',
            data: JSON.stringify({ temporalId: glbPolicy.Id, policyNumber: glbPolicy.DocumentNumber }),
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetExtensionReasons() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Endorsement/Extension/GetExtensionReasons',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPaymentPlanScheduleByPolicyEndorsementId(PolicyId, EndorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetPaymentPlanScheduleByPolicyEndorsementId',
            data: JSON.stringify({ policyId: PolicyId, endorsementId: EndorsementId }),
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

    static GetSummaryByTemporalId(tempId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Printing/Printer/GetSummaryByTemporalId',
            data: JSON.stringify({ temporalId: tempId, temporalAutho: false }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}