class SearchRequest {

    static GetTemporalEndorsementByPolicyId(policyId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Endorsement/EndorsementBase/GetTemporalEndorsementByPolicyId?policyId=' + policyId,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }


    static GetCurrentStatusPolicyByEndorsementIdIsCurrent(endorsementId, isCurrent) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetCurrentStatusPolicyByEndorsementIdIsCurrent?endorsementId=' + endorsementId + '&isCurrent=' + isCurrent,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetEndorsementPolicyInformation(endorsementId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetEndorsementPolicyInformation',
            data: JSON.stringify({ endorsementId: endorsementId, isCurrent: false }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetListEndorsement(branchId, prefixId, policyNumber) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Search/GetListEndorsement',
            data: JSON.stringify({ branchId: branchId, prefixId: prefixId, policyNumber: policyNumber }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetEndorsementController(prefixId, productId, endorsementType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Search/GetEndorsementControllerByPrefixIdProductIdEndorsementType',
            data: JSON.stringify({ prefixId: prefixId, productId: productId, endorsementType: endorsementType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetTemporalPolicyByPolicyId(policyId, endorsementType, endorsementController) {
        return $.ajax({
            type: "POST",
            url: rootPath + endorsementController + '/GetTemporalPolicyByPolicyId',
            data: JSON.stringify({ policyId: policyId, endorsementType: endorsementType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SendToExcel(policyModel, licensePlate) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Collective/Collective/GenerateFileToExport',
            dataType: 'json',
            data: JSON.stringify({ policy: policyModel, licensePlate: licensePlate }),
            contentType: 'application/json; charset=utf-8'
        });
    }
    static CreateTemporal(renewalModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + glbPolicy.EndorsementController + '/CreateTemporal',
            dataType: 'json',
            data: JSON.stringify({ renewalModel: renewalModel }),
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetPolicyRenewal(policyModel) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetPolicyRenewal',
            dataType: 'json',
            data: JSON.stringify({ renewalModel: policyModel }),
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetPrefixEndoEnabled(prefixId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetPrefixEndoEnabledByPrefixId?prefixId=' + prefixId,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static DeleteTemporal(documentNum, prefixId, branchId) {
        var dataEndorsement = JSON.stringify({
            operationId: 0, documentNum: documentNum,
            prefixId: parseInt(prefixId),
            branchId: parseInt(branchId)
        })
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/DeleteTemporalByOperationId',
            data: dataEndorsement,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRiskByPolicyIdByRiskDescription(policyId, endorsementId, riskDescription) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Search/GetRiskByPolicyIdByRiskDescription',
            data: JSON.stringify({ policyId: policyId, endorsementId: endorsementId, riskDescription: riskDescription }),
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


    static GetEndorsementsByPrefixIdBranchIdPolicyNumberCompany(BranchId, PrefixId, PolicyNumber, type) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Search/GetEndorsementsByPrefixIdBranchIdPolicyNumberCompany?branchId=' + BranchId + '&prefixId=' + PrefixId + '&policyNumber=' + PolicyNumber + '&current=' + type,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    //static ValidationAccessAndHierarchy() {
    //    return $.ajax({
    //        type: "POST",
    //        url: rootPath + 'Search/ValidationAccessAndHierarchy',
    //        dataType: 'json',
    //        contentType: 'application/json; charset=utf-8'
    //    });
    //}

    static ValidateCoveragePostContractual(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Search/ValidateCoveragePostContractual',
            data: JSON.stringify({ Policyid: policyId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetBranches() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + 'Search/GetBranches'
        });

    }

    static makeDeclarationEndorsement(PolicyId) {
        return $.ajax({
            type: "POST",
            url: rootPath +   glbPolicy.EndorsementController + '/CanMakeDeclarationEndorsement',
            data: JSON.stringify({ policyId: parseInt(PolicyId) }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPrefixes() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + 'Search/GetPrefixes',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });

    }
    
    static GetBranches() {
        return $.ajax({
            type: "POST",
            data: {},
            url: rootPath + 'Search/GetBranches',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });

    }
    
    static GetInsuredObjectsByRiskId(riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath +   glbPolicy.EndorsementController + "/GetInsuredObjectsByRiskId",
            data: JSON.stringify({ riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ValidateDeclarativeInsuredObjects(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + glbPolicy.EndorsementController + "/ValidateDeclarativeInsuredObjects",
            data: JSON.stringify({ policyId: policyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    
    static GetTemporalByIdTemporalType(id, temporalType, policieId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetTemporalByIdTemporalType',
            data: JSON.stringify({ id: id, temporalType: temporalType, policieId: policieId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetIvaParameterById() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Search/GetParameterIvaById',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static makeEndorsement(PolicyId) {
        return $.ajax({
            type: "POST",
            url: rootPath +   glbPolicy.EndorsementController + '/CanMakeEndorsement',
            data: JSON.stringify({ policyId: parseInt(PolicyId) }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ValidateDeleteTemporal(id) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/ValidateDeleteTemporal',
            data: JSON.stringify({EndorsementId: id}),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
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
    static GetCompanyEndorsementsByFilterPolicy(BranchId, PrefixId, PolicyNumber,isCurrent) {
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
    static ValidateEndorsement(objectModel) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Search/ValidateEndorsement',
            data: JSON.stringify({ policyBaseModel: objectModel }),
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
    static GetTemporalByPolicyIdEndorsementId(policyId, endorsementId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Search/GetTemporalByPolicyIdEndorsementId',
            data: JSON.stringify({ policyId: policyId, endorsementId: endorsementId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static GetUniquePolicyId() {
        return $.ajax({
            type: "GET",
            url: rootPath + 'Search/GetParameterUniquePolicy',
            data: {},
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}