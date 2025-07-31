class SetClaimReserveRequest {
    static GetBranches() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/SetClaimReserve/GetBranches',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/SetClaimReserve/GetPrefixes',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPolicyByEndorsementIdModuleType(endorsementId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/SetClaimReserve/GetPolicyByEndorsementIdModuleType',
            data: JSON.stringify({ endorsementId: endorsementId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetClaimsByPolicyId(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetClaimsByPolicyId',
            data: JSON.stringify({ policyId: parseInt(policyId) }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/SetClaimReserve/GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber',
            data: JSON.stringify({ prefixId: prefixId, branchId: branchId, policyDocumentNumber: policyDocumentNumber, claimNumber: claimNumber }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetReasonsByStatusIdPrefixId(statusId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/Claim/GetReasonsByStatusIdPrefixId',
            data: JSON.stringify({ statusId: statusId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExecuteReserveOperations(claimReserveDTO) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/SetClaimReserve/ExecuteReserveOperations',
            data: JSON.stringify({ claimReserveDTO: claimReserveDTO }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetInsuredAmount(policyId, riskNum, coverageId, coverNum, claimId, subClaimId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/SetClaimReserve/GetInsuredAmount',
            data: JSON.stringify({ policyId: policyId, riskNum: riskNum, coverageId: coverageId, coverNum: coverNum, claimId: claimId, subClaimId: subClaimId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    
    static GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(policyId, riskNum, coverageId, coverNum) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/SetClaimReserve/GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber',
            data: JSON.stringify({ policyId: policyId, riskNum: riskNum, coverageId: coverageId, coverNum: coverNum}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetClaimModifiesByClaimId(claimId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Claims/SetClaimReserve/GetClaimModifiesByClaimId',
            data: JSON.stringify({ claimId: claimId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetExchangeRate(currency) {
        return $.ajax({
            type: "POST",
            url: rootPath + "Claims/SetClaimReserve/GetExchangeRate",
            data: JSON.stringify({ currencyId: currency }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAccountingDate() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Claims/SetClaimReserve/GetAccountingDate',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}