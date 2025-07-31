class TransportCreditNoteRequest {

    static GetMaximumPremiumPercetToReturn(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'TransportCreditNote/GetMaximumPremiumPercetToReturn',
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
            url: rootPath + 'TransportCreditNote/GetEndorsementType',
            data: JSON.stringify({ policyId: policyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetEndorsementWithPremium(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'TransportCreditNote/GetEndorsmentsWithPremium',
            data: JSON.stringify({ policyId: policyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiksByPolicyIdByEndorsementId(policyId, endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'TransportCreditNote/GetRiksByPolicyIdByEndorsementId',
            data: JSON.stringify({ policyId: policyId, endorsementId: endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTransportsByPolicyIdByEndorsementId(policyId, endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'TransportCreditNote/GetTransportsByPolicyIdByEndorsementId',
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
            url: rootPath + 'TransportCreditNote/GetCompanyCoveragesByPolicyIdEndorsementIdRiskId',
            data: JSON.stringify({ policyId: policyId, endorsementId: endorsementId, riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static CreateTemporal(creditNoteModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'TransportCreditNote/CreateTemporal',
            data: JSON.stringify({ creditNoteModel: creditNoteModel }),
            dataType: "json",
            contentType: 'application/json; charset=utf-8'
        });
    }
    static CreateEndorsement() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'TransportCreditNote/CreateEndorsement',
            data: JSON.stringify({ temporalId: glbPolicy.Id, policyNumber: glbPolicy.DocumentNumber }),
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTemporalById(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'TransportCreditNote/GetTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}