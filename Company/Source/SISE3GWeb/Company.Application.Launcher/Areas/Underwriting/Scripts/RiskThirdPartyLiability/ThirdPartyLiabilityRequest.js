class ThirdPartyLiabilityRequest {

    static GetRiskById(endorsementType, temporalId, id) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetRiskById',
            data: JSON.stringify({ endorsementType: endorsementType, temporalId: temporalId, id: id }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, temporalType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetInsuredsByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType, customerType: customerType, temporalType: temporalType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPremium(policyId, riskData, coverages, dynamicProperties) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetPremium',
            data: JSON.stringify({ policyId: policyId, riskModel: riskData, coverages: coverages, dynamicProperties: dynamicProperties }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/ExcludeCoverage',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, riskCoverageId: riskCoverageId, description: description }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static SaveRisk(temporalId, riskData, coverages, dynamicProperties) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/SaveRisk',
            data: JSON.stringify({ temporalId: temporalId, riskModel: riskData, coverages: coverages, dynamicProperties: dynamicProperties }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetIndividualDetailsByIndividualId(individualId, customerType) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/Underwriting/GetIndividualDetailsByIndividualId',
            data: JSON.stringify({ individualId: individualId, customerType: customerType }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static DeleteRisk(temporalId, riskId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/DeleteRisk',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetModelsByMakeId(makeId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetModelsByMakeId',
            data: JSON.stringify({ makeId: makeId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetVersionsByMakeIdModelId(makeId, modelId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetVersionsByMakeIdModelId',
            data: JSON.stringify({ makeId: makeId, modelId: modelId }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static RunRules(temporalId, ruleSetId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/RunRules',
            data: JSON.stringify({ temporalId: temporalId, ruleSetId: ruleSetId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static UpdatePolicyComponents(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/UpdatePolicyComponents',
            data: JSON.stringify({ policyId: policyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRisksByTemporalId(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetRisksByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCoveragesByProductIdGroupCoverageId(temporalId, productId, groupCoverageId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetCoveragesByProductIdGroupCoverageId',
            data: JSON.stringify({ temporalId: temporalId, productId: productId, groupCoverageId: groupCoverageId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetYearsByMakeIdModelIdVersionId(makeId, modelId, versionId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetYearsByMakeIdModelIdVersionId',
            data: JSON.stringify({ makeId: makeId, modelId: modelId, versionId: versionId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetGroupCoverages(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetGroupCoverages',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetServicesTypeByProduct(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetServicesTypeByProduct',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetDeductiblesByProductId(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetDeductiblesByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRatingZonesByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetRatingZonesByPrefixId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCiaRiskByTemporalId(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetCiaRiskByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }  
    static GetCargoTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/GetCargoTypes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

   



}

class RiskTPLAdditionalDataRequest {

    static SaveAdditionalData(riskId, additionalData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskThirdPartyLiability/SaveAdditionalData',
            data: JSON.stringify({ riskId: riskId, additionalDataModel: additionalData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

  
}

