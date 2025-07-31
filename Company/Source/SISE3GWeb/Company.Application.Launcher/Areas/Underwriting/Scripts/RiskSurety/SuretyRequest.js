class RiskSuretyRequest {

    static GetCiaRiskByTemporalId(temporalId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetCiaRiskByTemporalId',
            data: JSON.stringify({ temporalId: temporalId }),
            dataType: "json",

            contentType: "application/json; charset=utf-8"
        });
    }

    static LoadAggregate(individualId, prefixCd, issueDate) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetAvailableAmountByIndividualId',
            data: JSON.stringify({ individualId: individualId, prefixCd: prefixCd, issueDate: issueDate }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetRiskSuretyById(temporalId, id) {
        return $.ajax({
            async: false,
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetRiskSuretyById',
            data: JSON.stringify({ temporalId: temporalId, id: id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetRiskSuretyCoveragesByProductIdGroupCoverageId(riskData) {

        var result = JSON.stringify({ riskModel: riskData })
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetCoveragesByProductIdGroupCoverageId',
            data: result,
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static SaveRisk(riskData, riskDataCoverages, dynamicProperties, validate) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/SaveRisk',
            data: JSON.stringify({ riskModel: riskData, coverages: riskDataCoverages, dynamicProperties: dynamicProperties, validate: validate }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static UpdatePolicyComponents(policyId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/UpdatePolicyComponents',
            data: JSON.stringify({ policyId: glbPolicy.Id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetIndividualDetailsByIndividualId(individualId, customerType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/GetIndividualDetailsByIndividualId',
            data: JSON.stringify({ individualId: individualId, customerType: customerType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDetailsByIndividualId(individualId, customerType) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/Underwriting/CompanyGetNotificationByIndividualId',
            data: JSON.stringify({ individualId: individualId, customerType: customerType }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static DeleteRisk(policyId, riskId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/DeleteRisk',
            data: JSON.stringify({ temporalId: policyId, riskId: riskId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static ExcludeCoverage(temporalId, riskId, riskCoverageId, description) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/ExcludeCoverage',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, riskCoverageId: riskCoverageId, description: description }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static QuotationCoverage(coverageModel, coverageData, CoveragesSurety) {
        return $.ajax({
            type: "POST",

            url: rootPath + 'Underwriting/RiskSurety/QuotationCoverage',
            data: JSON.stringify({ coverageModel: coverageModel, coverage: coverageData, RunRulesPost: true, listCompanyCoverage: CoveragesSurety }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static QuotationCoverages(temporalId, riskId, coverages, contractVale,policyid) {
        return $.ajax({
            type: "POST",

            url: rootPath + 'Underwriting/RiskSurety/QuotationCoverages',
            data: JSON.stringify({ temporalId: temporalId, riskId: riskId, listCompanyCoverage: coverages, contractVale: contractVale, policyId: policyid}),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetPremium(riskData, riskDataCoverages, dynamicProperties, contractObject) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetPremium',
            data: JSON.stringify({ riskModel: riskData, coverages: riskDataCoverages, dynamicProperties: dynamicProperties, contractObject: contractObject }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static RunRules(policyId, ruleSetId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/RunRules',
            data: JSON.stringify({ tempId: glbPolicy.Id, ruleSetId: ruleSetId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetGroupCoverages(productId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetGroupCoverages',
            data: JSON.stringify({ productId: productId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }
    static GetCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId, coveragesAdd) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetCoveragesByProductIdGroupCoverageIdPrefixId',
            data: JSON.stringify({ productId: productId, groupCoverageId: groupCoverageId, prefixId: prefixId, coveragesAdd: coveragesAdd }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static IsConsortiumindividualId(InsuredId) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/IsConsortiumindividualId',
            data: JSON.stringify({ individualId: InsuredId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static IsConsortiumindividualIdTempRisk(InsuredId) {
        return $.ajax({
            async: false,
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/IsConsortiumindividualId',
            data: JSON.stringify({ individualId: InsuredId }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetAvailableCumulus(InsuredId, prefixCd, issueDate) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetAvailableCumulus',
            data: JSON.stringify({ individualId: InsuredId, prefixCd: prefixCd, issueDate: issueDate }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    ///region Combos
    static GetSuretyContractCategories() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetSuretyContractCategories',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetSuretyContractTypes() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetSuretyContractTypes',
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    //Metodo de consulta de parametro de validacion de obligatoriedad de contragarantias
    static GetValCrossGuarantee() {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/IsGuaranteeMandatory',
            contentType: "application/json; charset=utf-8"
        });
    }

    static LoadRiskSuretyCombos(glbPolicy) {
        return $.ajax({
            type: "POST",
            url: rootPath + 'Underwriting/RiskSurety/GetRiskSureyLists',
            data: JSON.stringify({ productId: glbPolicy.Product.Id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCountries() {
        return $.ajax({
            type: 'GET',
            url: rootPath + "Underwriting/RiskSurety/GetCountries",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetStates(idCountry) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Underwriting/RiskSurety/GetStatesByCountry",
            data: JSON.stringify({ idCountry: idCountry }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetCities(countryId, stateId) {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Underwriting/RiskSurety/GetCitiesByCountryIdStateId",
            data: JSON.stringify({ countryId: countryId, stateId: stateId }),
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
        });
    }

    static GetDefaultCountry() {
        return $.ajax({
            type: 'POST',
            url: rootPath + "Underwriting/RiskSurety/GetDefaultCountry",
            dataType: 'json',
            contentType: "application/json; charset=utf-8"
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
}