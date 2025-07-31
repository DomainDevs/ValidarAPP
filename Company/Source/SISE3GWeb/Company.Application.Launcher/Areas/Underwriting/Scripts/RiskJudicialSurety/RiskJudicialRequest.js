class RiskJudicialRequest {

    static GetCoveragesByProductIdGroupCoverageId(policyId, groupCoverageId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetCoveragesByProductIdGroupCoverageId',
            data: JSON.stringify({ policyId: policyId, groupCoverageId: groupCoverageId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


    static GetRisksByTemporalId(temporalId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetRisksByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCiaRiskByTemporalId(temporalId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetCiaRiskByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetGroupCoverages(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetGroupCoverages',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRiskActivities(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetRiskActivitiesByProductId',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCitiesByCountryIdByStateId(countryId, stateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetCitiesByCountryIdStateId',
            data: JSON.stringify({ countryId: countryId, stateId: stateId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCountries() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetCountries',
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetStatesByCountryId(countryId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetStatesByCountryId',
            data: JSON.stringify({ countryId: countryId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCitiesByCountryIdStateId(countryId, stateId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetCitiesByCountryIdStateId',
            data: JSON.stringify({ countryId: countryId, stateId: stateId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static RunRules(temporalId, ruleSetId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/RunRules',
            data: JSON.stringify({ policyId: glbPolicy.Id, ruleSetId: ruleSetId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveRisk(temporalId, riskData, dynamicProperties, additionalData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/SaveRisk',
            data: JSON.stringify({ temporalId: temporalId, riskModel: riskData, coverages: riskData.Coverages, dynamicProperties: dynamicProperties, accessories: riskData.Accessories, additionalData: additionalData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRiskById(endorsementType, temporalId, id) {
        return $.ajax({
            async: false,
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetRiskById',
            data: JSON.stringify({ endorsementType: endorsementType, temporalId: temporalId, id: id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRatingZonesByPrefixIdCountryIdState(prefixId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetRatingZonesByPrefixIdCountryIdStateId',
            data: JSON.stringify({ prefixId: prefixId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/ExcludeCoverage',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, riskCoverageId: riskCoverageId, description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteRisk(policyId, id) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/DeleteRisk',
            data: JSON.stringify({ policyId: policyId, id: id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetPremium(policyId, riskData, dynamicProperties) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetPremium',
            data: JSON.stringify({ policyId: glbPolicy.Id, riskModel: riskData, coverages: riskData.Coverages, dynamicProperties: dynamicProperties }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetArticle(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetArticlesByProduct',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            async: false,
            contentType: "application/json; charset=utf-8"
        });
    }
    static getDaneCode() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetDaneCodeByCountryIdByStateIdByCityId',
            data: JSON.stringify({ countryId: countryId, stateId: stateId, cityId: cityId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCourt() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetCourt',
            data: JSON.stringify(),
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static QuotationCoverage(tempId, riskId, coverageData, coverageModel, riskData, runRules) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/QuotationCoverage',
            data: JSON.stringify({ tempId: tempId, riskId: riskId, coverage: coverageData, coverageModel: coverageModel, riskModel: riskData, runRules: runRules }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/ExcludeCoverage',
            data: JSON.stringify({ tempId: temporalId, riskId: riskId, riskCoverageId: riskCoverageId, description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static SaveCoverages(policyId, riskId, coverages) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/SaveCoverages',
            data: JSON.stringify({ policyId: policyId, riskId: riskId, coverages: coverages }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
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
    static GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType, temporalType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetInsuredsByDescriptionInsuredSearchTypeCustomerType',
            data: JSON.stringify({ description: description, insuredSearchType: insuredSearchType, customerType: customerType, temporalType: temporalType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetHolderAct() {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetInsuredType',
            data: JSON.stringify(),
            dataType: 'json',
            async: false,
            contentType: 'application/json; charset=utf-8',
        });
    }
    static GetCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId, coveragesAdd) {
        return $.ajax({
            type: 'POST',
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetCoveragesByProductIdGroupCoverageIdPrefixId',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, prefixId: prefixId, coveragesAdd: coveragesAdd }),
            dataType: 'json',
            contentType: 'application/json; charset=utf-8'
        });
    }
    static GetRateCoveragesByCoverageIdPolicyId(policyId, coverageId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Underwriting/Underwriting/GetRateCoveragesByCoverageIdPolicyId",
            data: JSON.stringify({ policyId: policyId, coverageId: coverageId }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetInsuredGuaranteeRelationPolicy(guaranteeId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/GetInsuredGuaranteeRelationPolicy',
            data: JSON.stringify({ guaranteeId: guaranteeId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
}

class RiskJudicialSuretyAdditionalDataRequest {

    static SaveAdditionalData(additionalData) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskJudicialSurety/SaveAdditionalData',
            data: JSON.stringify({ additionalDataModel: additionalData }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }


}
