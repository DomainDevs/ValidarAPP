class ContractObjectRequest {
    static GetBranches() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetBranches',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetPrefixes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetValidateOriginPolicy(documentNumber, prefixId, branchId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetValidateOriginPolicy',
            data: JSON.stringify({ documentNumber: documentNumber, prefixId: prefixId, branchId: branchId }),
            dataType: 'json',
            contentType: 'Application/json; charset=utf-8'
        });
    }

    static GetCompanyEndorsementsByFilterPolicy(BranchId, PrefixId, PolicyNumber, isCurrent) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Search/GetCompanyEndorsementsByFilterPolicy?branchId=' + BranchId + '&prefixId=' + PrefixId + '&policyNumber=' + PolicyNumber + '&current=' + isCurrent,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCurrentPolicyByEndorsementId(endorsementId, isCurrent) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetCurrentPolicyByEndorsementId?endorsementId=' + endorsementId + '&isCurrent=' + isCurrent,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetRiskByPolicyId(policyId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/ContractObject/GetRiskByPolicyId',
            data: JSON.stringify({ policyId: policyId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveContractObjectPolicyId(endorsementId, riskId, textRisk, textPolicy) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/ContractObject/SaveContractObjectPolicyId',
            data: JSON.stringify({ endorsementId: endorsementId, riskId: riskId, textRisk: textRisk, textPolicy: textPolicy}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveLog(endoChangeText) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/ContractObject/SaveLog',
            data: JSON.stringify({ endoChangeText: endoChangeText}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTextsByNameLevelIdConditionLevelId(name, levelId, conditionLevelId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetTextsByNameLevelIdConditionLevelId',
            data: JSON.stringify({ name: name, levelId: levelId, conditionLevelId: conditionLevelId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}
