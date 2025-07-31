class TextsRequest {
    static GetTextsByNameLevelIdConditionLevelId(name, levelId, conditionLevelId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetTextsByNameLevelIdConditionLevelId',
            data: JSON.stringify({ name: name, levelId: levelId, conditionLevelId: conditionLevelId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static CreateTexts(hiddenEndorsementController, modificationModel, companyModification) {
        return $.ajax({
            type: "POST",
            url: rootPath + hiddenEndorsementController + '/CreateTexts',
            data: JSON.stringify({ modificationModel: modificationModel, companyModification: companyModification }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetEndorsementsByPrefixIdBranchIdPolicyNumber(BranchId, PrefixId, PolicyNumber, type) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Search/GetEndorsementsByPrefixIdBranchIdPolicyNumber?branchId=' + BranchId + '&prefixId=' + PrefixId + '&policyNumber=' + PolicyNumber + '&current=' + type,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteEndorsementControl(Id) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/DeleteEndorsementControl',
            data: JSON.stringify({ EndorsementId: Id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
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
}