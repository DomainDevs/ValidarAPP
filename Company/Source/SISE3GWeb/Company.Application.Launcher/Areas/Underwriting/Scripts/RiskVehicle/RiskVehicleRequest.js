class RiskVehicleRequest {

    static GetListServiceType() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetListCompanyServiceType',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskById(endorsementType, temporalId, id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetRiskById',
            data: JSON.stringify({ endorsementType: endorsementType, temporalId: temporalId, id: id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCiaRiskByTemporalId(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetCiaRiskByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetFasecoldaByPlate(objVehicle, objBranch) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetFasecoldaByPlate',
            data: JSON.stringify({ vehicle: objVehicle, branchId: objBranch }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetFasecoldaByCode(code, year) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetFasecoldaByCode',
            data: JSON.stringify({ code: code, year: year }),
            dataType: "json",  
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetFasecoldaCodeByMakeIdModelIdVersionId(makeId, modelId, versionId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetFasecoldaCodeByMakeIdModelIdVersionId',
            data: JSON.stringify({ makeId: makeId, modelId: modelId, versionId: versionId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPriceByMakeIdModelIdVersionId(makeId, modelId, versionId, year) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetPriceByMakeIdModelIdVersionId',
            data: JSON.stringify({ makeId: makeId, modelId: modelId, versionId: versionId, year: year }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByProductIdGroupCoverageId(policyId, groupCoverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetCoveragesByProductIdGroupCoverageId',
            data: JSON.stringify({ policyId: policyId, groupCoverageId: groupCoverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPremium(policyId, riskData, dynamicProperties, additionalData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetPremium',
            data: JSON.stringify({ policyId: policyId, riskModel: riskData, coverages: riskData.Coverages, dynamicProperties: dynamicProperties, accessories: riskData.Accessories, additionalData: additionalData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetMinPremiunRelationProduct(prefixId, productName) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskVehicle/GetMinPremiunRelationByPrefixAndProduct',
            data: JSON.stringify({ PrefixId: prefixId, ProductName: productName }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }

    static SaveRisk(temporalId, riskData, dynamicProperties, additionalData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/SaveRisk',
            data: JSON.stringify({ temporalId: temporalId, riskModel: riskData, coverages: riskData.Coverages, dynamicProperties: dynamicProperties, accessories: riskData.Accessories, additionalData: additionalData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteRisk(policyId, id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/DeleteRisk',
            data: JSON.stringify({ policyId: policyId, id: id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/ExcludeCoverage',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, riskCoverageId: riskCoverageId, description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static RunRules(policyId, ruleSetId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/RunRules',
            data: JSON.stringify({ policyId: policyId, ruleSetId: ruleSetId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRisksByTemporalId(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetRisksByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetModelsByMakeId(makeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetModelsByMakeId',
            data: JSON.stringify({ makeId: makeId }),
            dataType: "json",     
            async: false, 
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetVersionsByMakeIdModelId(makeId, modelId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetVersionsByMakeIdModelId',
            data: JSON.stringify({ makeId: makeId, modelId: modelId }),
            dataType: "json", 
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetYearsByMakeIdModelIdVersionId(makeId, modelId, versionId) {
        return $.ajax({
            type: "POST",
            async: false,
            url: rootPath + 'Underwriting/RiskVehicle/GetYearsByMakeIdModelIdVersionId',
            data: JSON.stringify({ makeId: makeId, modelId: modelId, versionId: versionId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetTypesByTypeId(typeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetTypesByTypeId',
            data: JSON.stringify({ typeId: typeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRatingZonesByPrefixId(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetRatingZonesByPrefixId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetLimitsRcByPrefixIdProductIdPolicyTypeId(prefixId, productId, policyTypeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetLimitsRcByPrefixIdProductIdPolicyTypeId',
            data: JSON.stringify({ prefixId: prefixId, productId: productId, policyTypeId: policyTypeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetGroupCoverages(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetGroupCoverages',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetMakes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetMakes',            
            dataType: "json",   
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetUses() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetUses',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetColors() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetColors',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetListsVehicleRisk() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetColors',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static LoadRiskCombos(viewModel, policyModel) {
        return $.ajax({
            type: "POST",
            //         url: rootPath + 'Underwriting/Underwriting/SaveTemporal',
            url: rootPath + 'Underwriting/RiskVehicle/GetRiskLists',
            data: JSON.stringify({ companyVehicle: viewModel, policyModel: policyModel }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRegularExpression() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetRegularExpression',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class RiskVehicleAdditionalDataRequest {
    static SaveAdditionalData(riskId, additionalData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/SaveAdditionalData',
            data: JSON.stringify({ riskId: riskId, additionalDataModel: additionalData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetFuels() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetFuels',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetBodies() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetBodies',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}

class RiskVehicleCoverageRequest {

    static GetCoverageByCoverageId(coverageId, groupCoverageId, policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetCoverageByCoverageId',
            data: JSON.stringify({ coverageId: coverageId, groupCoverageId: groupCoverageId, policyId: policyId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/ExcludeCoverage',
            data: JSON.stringify({ tempId: temporalId, riskId: riskId, riskCoverageId: riskCoverageId, description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static QuotationCoverage(tempId, riskId, coverageModel, coverageData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/QuotationCoverage',
            data: JSON.stringify({ tempId: tempId, riskId: riskId, coverageModel: coverageModel, coverage: coverageData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveCoverages(policyId, riskId, coverages) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/SaveCoverages',
            data: JSON.stringify({ policyId: policyId, riskId: riskId, coverages: coverages }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId, coveragesAdd) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskVehicle/GetCoveragesByProductIdGroupCoverageIdPrefixId',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, prefixId: prefixId, coveragesAdd: coveragesAdd }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

}
