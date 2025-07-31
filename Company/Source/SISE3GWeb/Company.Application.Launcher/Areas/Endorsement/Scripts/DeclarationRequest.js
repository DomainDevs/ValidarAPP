
class DeclarationRequest {

    static GetDeclarationEndorsementByPolicyId(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/GetDeclarationEndorsementByPolicyId',
            data: JSON.stringify({ policyId: policyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }

    static GetCompanyEndorsementByEndorsementTypeId(endorsementTypeId, PolicyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/GetCompanyEndorsementByEndorsementTypeId',
            data: JSON.stringify({ endorsementTypeId: endorsementTypeId, PolicyId: PolicyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }

    static GetPolicyByPrefixIdBranchIdPolicyNumber(BranchId, PrefixId, PolicyNumber) {
        return $.ajax({
            type: 'POST',
            url: rootPath + glbPolicy.EndorsementController + '/GetPolicyByPrefixIdBranchIdPolicyNumber',
            dataType: 'json',
            data: JSON.stringify({ prefixId: PrefixId, branchId: BranchId, policyNumber: PolicyNumber }),
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetRisksByPolicyId(PolicyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/GetRisksByPolicyId',
            data: JSON.stringify({ PolicyId: PolicyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredObjectsByRiskId(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + "/GetInsuredObjectsByRiskId",
            data: JSON.stringify({ riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateEndorsement(temporalId, policyNumber) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/CreateEndorsement',
            data: JSON.stringify({ temporalId: glbPolicy.Id.trim(), policyNumber:glbPolicy.DocumentNumber }),
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskByRiskId(Risk) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/GetRiskByRiskId',
            data: JSON.stringify({ Risk: Risk }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }

    static GetCurrentSummaryByEndorsementId(endorsementId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetCurrentStatusPolicyByEndorsementIdIsCurrent?endorsementId=' + endorsementId + '&isCurrent=' + false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateTemporal(declarationModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + glbPolicy.EndorsementController +  '/CreateTemporal',
            dataType: 'json',
            data: JSON.stringify({ declarationModel: declarationModel }),
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateEndorsement() {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/CreateEndorsement',
            data: JSON.stringify({ temporalId: glbPolicy.Id, policyNumber:glbPolicy.DocumentNumber }),
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTemporalById(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + '/GetTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static EvalRiskByInsuredObject(policyNumber, riskId, insuredObjectId) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController +  '/EndorsementByRiskByInsuredObjectId',
            data: JSON.stringify({ policyId: policyNumber, riskId: riskId, insuredObjectId: insuredObjectId }),
            contentType: "application/json; charset=utf-8"
        });
    }
}
