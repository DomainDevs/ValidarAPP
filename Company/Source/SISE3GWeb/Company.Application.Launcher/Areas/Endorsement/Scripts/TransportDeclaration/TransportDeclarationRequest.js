
class TransportDeclarationRequest 
{

static GetDeclarationEndorsementByPolicyId(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'TransportDeclaration/GetDeclarationEndorsementByPolicyId',
            data: JSON.stringify({ policyId: policyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }
    static GetCompanyEndorsementByEndorsementTypeId(endorsementTypeId, PolicyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'TransportDeclaration/GetCompanyEndorsementByEndorsementTypeId',
            data: JSON.stringify({ endorsementTypeId: endorsementTypeId, PolicyId: PolicyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        })
    }

    static GetPolicyByPrefixIdBranchIdPolicyNumber(BranchId, PrefixId, PolicyNumber) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'TransportDeclaration/GetPolicyByPrefixIdBranchIdPolicyNumber',
            dataType: 'json',
            data: JSON.stringify({ prefixId: PrefixId, branchId: BranchId, policyNumber: PolicyNumber }),
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetTransportsByPolicyId(PolicyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'TransportDeclaration/GetTransportRisksByPolicyId',
            data: JSON.stringify({ PolicyId: PolicyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredObjectsByRiskId(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + "TransportDeclaration/GetInsuredObjectsByRiskId",
            data: JSON.stringify({ riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static CreateEndorsement(temporalId, policyNumber) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'TransportDeclaration/CreateEndorsement',
            data: JSON.stringify({ temporalId: glbPolicy.Id.trim(), policyNumber: glbPolicy.DocumentNumber }),
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTransportByRiskId(Risk) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'TransportDeclaration/GetTransportByRiskId',
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
            url: rootPath + 'TransportDeclaration/CreateTemporal',
            dataType: 'json',
            data: JSON.stringify({ declarationModel: declarationModel }),
            contentType: 'application/json; charset=utf-8'
        });
    }

    static CreateEndorsement() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'TransportDeclaration/CreateEndorsement',
            data: JSON.stringify({ temporalId: glbPolicy.Id, policyNumber: glbPolicy.DocumentNumber }),
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTemporalById(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'TransportDeclaration/GetTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}
