class CreditNoteRequest {

    static GetMaximumPremiumPercetToReturn(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'CreditNote/GetMaximumPremiumPercetToReturn',
            data: JSON.stringify({ policyId: policyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEndorsementsByPrefixIdBranchIdPolicyNumber(BranchId, PrefixId, PolicyNumber, type) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'TransporCreditNote/GetEndorsementsByPrefixIdBranchIdPolicyNumber',
            data: JSON.stringify({}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEndorsementTypes(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'CreditNote/GetEndorsementType',
            data: JSON.stringify({ policyId: policyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEndorsementWithPremium(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'CreditNote/GetEndorsmentsWithPremium',
            data: JSON.stringify({ policyId: policyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiksByPolicyIdByEndorsementId(policyId, endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'CreditNote/GetRiksByPolicyIdByEndorsementId',
            data: JSON.stringify({ policyId: policyId, endorsementId: endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTransportsByPolicyIdByEndorsementId(policyId, endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'CreditNote/GetTransportsByPolicyIdByEndorsementId',
            data: JSON.stringify({ policyId: policyId, endorsementId: endorsementId }),
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
    static GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'CreditNote/GetCompanyCoveragesByPolicyIdEndorsementIdRiskId',
            data: JSON.stringify({ policyId: policyId, endorsementId: endorsementId, riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static CreateTemporal(creditNoteModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'CreditNote/CreateTemporal',
            data: JSON.stringify({ creditNoteModel: creditNoteModel }),
            dataType: "json",
            contentType: 'application/json; charset=utf-8'
        });
    }
    static CreateEndorsement(temporalId, policyNumber) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/CreateEndorsement',
            data: JSON.stringify({ temporalId: glbPolicy.Id, policyNumber: glbPolicy.DocumentNumber }),
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTemporalById(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'CreditNote/GetTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}