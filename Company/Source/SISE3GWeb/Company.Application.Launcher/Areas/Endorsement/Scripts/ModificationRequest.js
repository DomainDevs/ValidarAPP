class ModificationRequest {


    static SearchByRiskDescription() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Search/GetRiskByPolicyIdByRiskDescription',
            data: JSON.stringify({ policyId: glbPolicyEndorsement.Endorsement.PolicyId, endorsementId: glbPolicyEndorsement.Endorsement.Id, riskDescription: $('#inputDescriptionRisk').val().trim() }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCurrentSummaryByEndorsementId(endorsementId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetCurrentStatusPolicyByEndorsementIdIsCurrent?endorsementId=' + endorsementId + '&isCurrent=' + false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetTemporalById(endorsementController, temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + endorsementController + '/GetTemporalById',
            data: JSON.stringify({ id: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateTemporal(endorsementController, objectModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + endorsementController + '/CreateTemporal',
            data: JSON.stringify({ modificationModel: objectModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateTemporalModel(endorsementController, objectModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + endorsementController + '/CreateTemporal',
            data: JSON.stringify({ modificationModel: objectModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateEndorsement(endorsementController, temporalId, policyNumber, companyModification) {
        return $.ajax({
            type: "POST",
            url: rootPath + endorsementController + '/CreateEndorsement',
            data: JSON.stringify({ temporalId: temporalId, policyNumber: policyNumber, companyModification: companyModification }),
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetModificationReasons() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Endorsement/Modification/GetModificationReasons',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetModificationType() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Endorsement/Modification/GetModificationType',
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